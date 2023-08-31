using System.Collections.Generic;
using Unity.Barracuda;
using MLAHelper.Model.Structure;
using System;
using System.Linq;
using MLAHelper.Model.Visuals;
using MLAHelper.ScriptableReferenceSystem.Collection;
using MLAHelper.ScriptableReferenceSystem.Reference;

// Constructs and manages layers of a given model, passthrough constructed layers to ModelVisualManager.
namespace MLAHelper.Model.Builder {
    public class LayerConstructor
    {
        // Event delegate for when a layers have been constructed and indexed
        public Action IndexedLayersReady;
        private ModelConstructor modelConstructor;
        private ModelVisualManager modelVisualManager;

        // Indexed layers dictionary
        private Dictionary<string, List<ModelLayerParent>> indexedLayers;

        public Dictionary<string, List<ModelLayerParent>> IndexedLayers { get => indexedLayers; private set => indexedLayers = value; }

        // Constructor
        public LayerConstructor(ModelConstructor fullModelBuilder) {
            this.modelConstructor = fullModelBuilder;

            // Subscribe to the NewExecute event in the fullModelBuilder
            fullModelBuilder.NewExecute += OnModelStructureFinished;
        }

        // Destructor to unsubscribe from the event
        ~LayerConstructor()
        {
            modelConstructor.NewExecute -= OnModelStructureFinished;
        }

        public void SetModelVisualManager(ModelVisualManager modelVisualManager) {
            this.modelVisualManager = modelVisualManager;
        }

        // Constructs Observation, Active, Action and Action mask layers, and updates information for concurrent passes
        private void OnModelStructureFinished()
        {
            if (IndexedLayers == null) {
                IndexedLayers = new Dictionary<string, List<ModelLayerParent>>();

                ConstructObservationLayers();
                ConstructActiveLayers();
                ConstructActionAndActionMaskLayers();
                IndexedLayersReady?.Invoke();
            } else {
                UpdateLayerInformation();
            }
        }

        // Update information for existing layers
        private void UpdateLayerInformation() {
            Dictionary<string, LayerVisualInformationParent> visualsByName = modelVisualManager.VisualsByName;

            UpdateObservationInformation(visualsByName);
            UpdateActionAndActionMaskInformation(visualsByName);
        }

        // Update information for observation layers
        private void UpdateObservationInformation(Dictionary<string, LayerVisualInformationParent> visualsByName) {
            ObservationCollection observations = modelConstructor.ObservationCollection;
            string observationName;

            // Update visuals for observations
            for (int i = 0; i < observations.GetFloatArrayLength(); i++) {
                float observation = observations.GetObservationPerIndex(i, out observationName);
                if (visualsByName.ContainsKey(observationName)) {
                    visualsByName[observationName].UpdateVisuals(observation);
                }
            }
        }

        // Update information for action and action mask layers
        private void UpdateActionAndActionMaskInformation(Dictionary<string, LayerVisualInformationParent> visualsByName)
        {
            float[] activeLayerFloats = modelConstructor.GetOutputByName(SettingsHelper.GetSettings().ActionTypeFocus.ToString()).AsFloats();

            for (int i = 0; i < activeLayerFloats.Length; i++) {
                var actionName = "Action_" + i;
                if (visualsByName.ContainsKey(actionName)) {
                    visualsByName[actionName].UpdateVisuals(activeLayerFloats[i]);

                    UpdateActionMask(i, activeLayerFloats[i], visualsByName);
                }
            }
        }

        // Update action masks for specific action
        private void UpdateActionMask(int branchIndex, float activeLayerValue, Dictionary<string, LayerVisualInformationParent> visualsByName) {
            foreach (var actionMask in modelConstructor.ActionMaskCollection.actionMasks.Where(mask => mask.Branch == branchIndex).ToArray()) {
                if (visualsByName.ContainsKey(actionMask.Name)) {
                    bool chosen = actionMask.Index == activeLayerValue;
                    (visualsByName[actionMask.Name] as LayerVisualInformationActionMask).UpdateInformation(chosen, actionMask.Value);
                }
            }
        }

        // Initial construction of action layers and their action masks
        private void ConstructActionAndActionMaskLayers()
        {
            ModelLayerActive activeLayer = IndexedLayers[SettingsHelper.GetSettings().ActionTypeFocus.ToString()][0] as ModelLayerActive;
            for (int i = 0; i < activeLayer.Values.Length; i++) {
                ModelLayerAction actionLayer = ConstructActionLayer(activeLayer.Values[i], "Action_" + i, activeLayer);
                GLobalActionMaskReference[] actionMasks = modelConstructor.ActionMaskCollection.actionMasks.Where(mask => mask.Branch == i).ToArray();
                for (int j = 0; j < actionMasks.Length; j++) {
                    bool chosen = actionMasks[j].Index == activeLayer.Values[i];
                    ConstructActionMaskLayer(actionMasks[j], actionLayer, chosen);
                    actionLayer.AddActionMask(j, actionMasks[j]);
                }
            }
        }

        // Initial construction of observation layers
        private void ConstructObservationLayers() {
            foreach (var (name, tensor) in modelConstructor.InputDictionary) {
                if (name != "action_masks") {
                    for (int i = 0; i < tensor.AsFloats().Length; i++) {
                        ConstructObservationLayer(i, name);
                    }
                }
            }
        }

        // Initial construction of active layers
        private void ConstructActiveLayers()
        {
            List<Tensor> layerTensors = modelConstructor.RequestFullModel();

            for (int i = 0; i < layerTensors.Count; i++) {
                ConstructActiveLayer(layerTensors[i]);

            }
        }

        // Construct individual active layer
        private void ConstructActiveLayer(Tensor tensor)
        {
            Layer.Type layerType = modelConstructor.GetLayerTypeByName(tensor.name);
            Layer.Activation activationType = modelConstructor.GetActivationType(tensor.name);
            string[] inputNames = modelConstructor.GetLayerInputNames(tensor.name);

            ModelLayerActive observationLayer = new ModelLayerActive(tensor.name, layerType, activationType, GetModelLayersByName(inputNames), tensor.AsFloats());

            AddToNamedDictionary(tensor.name, observationLayer);
        }

        // Construct individual observation layer
        private void ConstructObservationLayer(int index, string name) {
            string observationName;
            float valueNewValue = modelConstructor.ObservationCollection.GetObservationPerIndex(index, out observationName);
            ModelLayerObservation observationLayer = new ModelLayerObservation(observationName, valueNewValue);
            AddToNamedDictionary(name, observationLayer);
        }

        // Construct individual action mask layer
        private void ConstructActionMaskLayer (GLobalActionMaskReference boolReference, ModelLayerParent inputLayer, bool chosen) {
            ModelLayerActionMask observationLayer = new ModelLayerActionMask(boolReference.Name, boolReference, inputLayer, chosen);
            AddToNamedDictionary(boolReference.Name, observationLayer);
        }

        // Construct individual action layer
        private ModelLayerAction ConstructActionLayer (float value, string name, ModelLayerActive inputLayer) {
            ModelLayerAction actionLayer = new ModelLayerAction(name, inputLayer, value);
            AddToNamedDictionary(name, actionLayer);

            return actionLayer;
        }

        // Returns previously created ModelLayers by name
        private ModelLayerParent[] GetModelLayersByName(string[] names) {
            if (names != null && names.Length > 0) {
                return names.Where(IndexedLayers.ContainsKey).SelectMany(x => IndexedLayers[x]).ToArray();
            }
            return null;
        }

        private void AddToNamedDictionary(string name, ModelLayerParent modelLayerParent) {
            if (IndexedLayers.ContainsKey(name)) {
                IndexedLayers[name].Add(modelLayerParent);
            } else {
                IndexedLayers.Add(name, new List<ModelLayerParent>{ modelLayerParent });
            }
        }
    }
}