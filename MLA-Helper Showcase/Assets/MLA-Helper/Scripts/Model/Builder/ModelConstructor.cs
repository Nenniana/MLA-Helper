using System;
using System.Collections.Generic;
using System.Linq;
using MLAHelper.Interface;
using MLAHelper.Model.Visuals;
using MLAHelper.ScriptableReferenceSystem.Collection;
using MLAHelper.ScriptableReferenceSystem.Reference;
using Sirenix.OdinInspector;
using Unity.Barracuda;
using UnityEngine;

// Constructs barracuda model based on Onnx Model, builds InputDictionary based on observations and action marks, and executes through GPU worker to get all model layer names and info.
namespace MLAHelper.Model.Builder {
    public class ModelConstructor : MonoBehaviour
    {
        // Event delegate for when a new model has been executed
        public Action NewExecute;

        // Pass-on value for ModelVisualManager
        [SerializeField]
        private Transform visualizationTransform;

        // Pass-on value for ModelVisualManager
        [SerializeField]
        private GameObject layerElementPrefab;

        // Pass-on value for ModelVisualManager
        [SerializeField]
        private GameObject lineElementPrefab;

        // Observation input for model execution
        [SerializeField]
        private ObservationCollection observationCollection;

        // Action mask input for model execution
        [SerializeField]
        private ActionMaskCollection actionMaskCollection;

        // current Onnx Model ML-Agents is being used with
        [SerializeField]
        private NNModel nNModel;

        // Constructed Barracuda model from the nNModel
        private Unity.Barracuda.Model runtimeModel;

        // Worker for model execution
        private IWorker worker;

        // Passthrough entities
        private ModelVisualManager modelVisualManager;
        private LayerConstructor layerConstructor;

        // Input Dictionary for passing action masks and observations to worker for execution
        private Dictionary<string, Tensor> inputDictionary;

        // Public getter, private setter for passthrough
        public Dictionary<string, Tensor> InputDictionary { get => inputDictionary; private set => inputDictionary = value; }
        public ObservationCollection ObservationCollection { get => observationCollection; private set => observationCollection = value; }
        public ActionMaskCollection ActionMaskCollection { get => actionMaskCollection; private set => actionMaskCollection = value; }

        // Initialization of entity, called when MLA-Helper is deployed from Helper Window
        internal void Initialize(LoadAgentHelperWindow helperWindow) {
            nNModel = helperWindow.AgentModel;
            observationCollection = helperWindow.ObservationCollection;
            actionMaskCollection = helperWindow.ActionMaskCollection;
        }

        // Load Onnx model as Barracuda Model
        private Unity.Barracuda.Model CreateRuntimeModel() {
            runtimeModel = ModelLoader.Load(nNModel);

            return runtimeModel;
        }
        
        // Create GPU worker with access to all layer names for full output
        private void CreateWorker() {
            worker = WorkerFactory.CreateWorker(CreateRuntimeModel(), GetLayerNames(), WorkerFactory.Device.GPU);
        }

        private bool CheckForVoids() {
            return nNModel != null && observationCollection != null && actionMaskCollection != null;
        }

        // Start execution on start if designated in settings
        private void Start() {
            if (SettingsHelper.GetSettings().VisualizeOn == MLAHelperSettingsEnums.VisualizeOn.Start) {
                if (CheckForVoids()) {
                    ExecuteNewInput();
                }
            }
        }

        // Create worker, and Start execution on awake if designated in settings
        private void Awake() {
            CreateWorker();

            if (SettingsHelper.GetSettings().VisualizeOn == MLAHelperSettingsEnums.VisualizeOn.Awake) {
                if (CheckForVoids()) {
                    ExecuteNewInput();
                }
            }
        }

        // Request execution from inspector
        [Button]
        public void RequestNewExecute() {
            ExecuteNewInput();
        }

        // Returns full list of all tensor layers in model
        public List<Tensor> RequestFullModel() {
            CheckInputDictionary();
            return GetOutputsByName(GetLayerNames());
        }

        // Build Input Dictionary if null or empty
        private void CheckInputDictionary() {
            if (InputDictionary == null && InputDictionary.Count == 0) {
                BuildInputDictionary();
            }
        }

        // Execute current model. Creates layerConstructor and ModelVisualManager on first pass. Will also Dispose of all current tensors.
        public void ExecuteNewInput() {
            if (layerConstructor == null) {
                layerConstructor = new LayerConstructor(this);
                modelVisualManager = new ModelVisualManager(layerConstructor, layerElementPrefab, lineElementPrefab, visualizationTransform);
                layerConstructor.SetModelVisualManager(modelVisualManager);
            }

            Dispose();
            BuildInputDictionary();
            NewExecute?.Invoke();
        }

        public Layer.Type GetLayerTypeByName(string name) {
            if(runtimeModel.layers.Exists(layer => layer.name == name))
                return runtimeModel.layers.Find(layer => layer.name == name).type;

            return Layer.Type.Reshape;
        }

        public string[] GetLayerInputNames(string name) {
            if (runtimeModel.layers.Exists(layer => layer.name == name))
                return runtimeModel.layers.Find(layer => layer.name == name).inputs;
            
            return null;
        }

        internal Layer.Activation GetActivationType(string name)
        {
            if (runtimeModel.layers.Exists(layer => layer.name == name))
                return runtimeModel.layers.Find(layer => layer.name == name).activation;

            return Layer.Activation.Abs;
        }

        // Return copy of specific execution
        public Tensor GetOutputByName(string name) {
            return worker.Execute(InputDictionary).CopyOutput(name);
        }

        // Return list of tensors for given names, expect memory_size, discrete_action_output_shape and version_number.
        private List<Tensor> GetOutputsByName(string[] names)
        {
            List<Tensor> outputs = new List<Tensor>();
            foreach (string name in names)
            {
                if (string.Compare(name, "memory_size", true) != 0 &&
                    string.Compare(name, "discrete_action_output_shape", true) != 0 &&
                    string.Compare(name, "version_number", true) != 0) 
                {
                    Tensor outputTensor = worker.Execute(InputDictionary).CopyOutput(name);
                    outputs.Add(outputTensor);
                }
            }

            return outputs;
        }

        // Correctly disposes of all tensors and clears InputDictionary
        private void Dispose()
        {
            if (InputDictionary != null && InputDictionary.Count > 0) {
                foreach (var (name,tensor) in InputDictionary ) {
                    tensor.Dispose();
                }

                InputDictionary.Clear();
            }
        }

        // Constructs Input Dictionary based on runtime model inputs array. Creates action masks and observations.
        private void BuildInputDictionary() {
            InputDictionary = new Dictionary<string, Tensor>();

            for (int i = 0; i < runtimeModel.inputs.Count; i++)
            {
                string name = runtimeModel.inputs[i].name;

                if (name != "action_masks")
                    InputDictionary.Add(name, GetObservationTensor(runtimeModel.inputs[i].shape, name));
                else
                    InputDictionary.Add(name, GetObservationTensor(runtimeModel.inputs[i].shape, name, true));
            }
        }

        // Create new Tensor, passthrough for actionMask bool
        private Tensor GetObservationTensor(int[] shape, string name, bool actionMask = false) {
            return new Tensor(
                shape,
                GetAgentFloatArray(shape, name, actionMask)
            );
        }

        // Return Observation or Action mask Tensor
        private float[] GetAgentFloatArray(int[] shape, string name, bool actionMask)
        {
            if (actionMask) return GetAgentActionmaskArray(shape[shape.Count() - 1], name);
            else return GetAgentObservationArray(shape[shape.Count() - 1], name);
        }

        // Checks if ObservationSize and Observation collection size is equal, then builds observation float array
        private float[] GetAgentObservationArray(int observationSize, string name)
        {
            float[] observationArray = new float[observationSize];

            if (observationSize == ObservationCollection.GetFloatArrayLength()) {
                for (int i = 0; i < ObservationCollection.ObservationReferences.Count(); i++) {
                    for (int j = 0; j < ObservationCollection.ObservationReferences[i].GetValue.Count(); j++)
                    {
                        observationArray[i+j] = ObservationCollection.ObservationReferences[i].GetValue[j];
                    }
                }
            }
            else
                Debug.LogError($"Observation size: {observationSize} for {name} layer is not equal to observation array {ObservationCollection.Name} with size {ObservationCollection.GetFloatArrayLength()}.");

            return observationArray;
        }

        // Checks if actionMaskSize and Action mask collection size is equal, then builds action mask float array
        private float[] GetAgentActionmaskArray(int actionMaskSize, string name)
        {
            float[] actionMaskArray = new float[actionMaskSize];

            if (actionMaskSize == ActionMaskCollection.actionMasks.Count()) 
                for (int i = 0; i < actionMaskSize; i++) {
                    GLobalActionMaskReference reference = ActionMaskCollection.actionMasks[i];
                    actionMaskArray[i] = Convert.ToInt32(reference.Value);
                }
            else
                Debug.LogError($"Observation size: {actionMaskSize} for {name} layer is not equal to observation array {ActionMaskCollection.Name} with size {ActionMaskCollection.actionMasks.Count()}.");

            return actionMaskArray;
        }

        // Gets all layer names to enable worker to execute full output
        private string[] GetLayerNames()
        {
            string[] additionalOutputs = new string[runtimeModel.layers.Count];
            for (int i = 0; i < runtimeModel.layers.Count; i++)
            {
                // Debug.Log("Layer name: " + runtimeModel.layers[i].name);
                additionalOutputs[i] = runtimeModel.layers[i].name;
            }

            return additionalOutputs;
        }

        // Dispose OnDestroy
        private void OnDestroy()
        {
            Dispose();
            worker?.Dispose();
        }
    }
}