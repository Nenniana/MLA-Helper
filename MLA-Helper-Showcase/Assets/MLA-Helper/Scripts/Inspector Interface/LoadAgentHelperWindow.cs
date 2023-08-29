using System;
using MLAHelper.HelperAgent;
using MLAHelper.Model.Builder;
using MLAHelper.ScriptableReferenceSystem.Collection;
using MLAHelper.ScriptableReferenceSystem.Reference;
using Sirenix.OdinInspector;
using Unity.Barracuda;
using Unity.MLAgents.Policies;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

// Will construct and deploy MLA-Helper in current scene given user setup
namespace MLAHelper.Interface {
    [Serializable]
    [CreateAssetMenu(fileName = "AgentWindowSO", menuName = "MLA-Helper/AgentWindow", order = 1)]
    public class LoadAgentHelperWindow : ScriptableObject {

        // SerializedField for user input
        [SerializeField]
        [FoldoutGroup("Agent", expanded: true)]
        [BoxGroup("Agent/General")]
        [HorizontalGroup("Agent/General/Row1")]
        [VerticalGroup("Agent/General/Row1/Left")]
        [LabelText("Input MLAgents Agent")]
        [LabelWidth(120)]
        [OnValueChanged("GetAgentInformation")]
        private MLAHelperAgent agent;

        // SerializedField for user input or filled if agent has model
        [SerializeField]
        [ShowIf("parametersFound")]
        [InfoBox("No NNModel was found. Please supply your own.", InfoMessageType.Warning, "@agentModel == null")]
        [HorizontalGroup("Agent/General/Row1")]
        [VerticalGroup("Agent/General/Row1/Right")]
        [LabelWidth(80)]
        private NNModel agentModel;

        // Observation collection, informs if size is incorrect to observation space for model
        [SerializeField]
        [InfoBox("@\"The collection is currently not the same overall size as the model. There's currently \" + CurrentObservationSize + \" observations, where the model calls for \" + observationArrayOverallSize + \".\"", InfoMessageType.Warning, "@observationArrayOverallSize != CurrentObservationSize")]
        [ShowIf("@parametersFound || observationCollection != null")]
        [BoxGroup("Agent/Collections")]
        [HorizontalGroup("Agent/Collections/Row1", LabelWidth = 100, Width = 0.5f)]
        [VerticalGroup("Agent/Collections/Row1/Left")]
        [OnValueChanged("SettingObservationAsChanged")]
        [ListDrawerSettings(ShowIndexLabels = false, ShowPaging = false, Expanded = true)]
        [HideReferenceObjectPicker]
        private ObservationCollection observationCollection;

        // Action mask collection
        [SerializeField]
        [ShowIf("@parametersFound || actionMaskCollection != null")]
        [OnValueChanged("SettingActionMasksAsChanged")]
        [HorizontalGroup("Agent/Collections/Row1", LabelWidth = 100, Width = 0.5f)]
        [VerticalGroup("Agent/Collections/Row1/Right")]
        [ListDrawerSettings(ShowIndexLabels = false, ShowPaging = false, Expanded = true)]
        [HideReferenceObjectPicker]
        private ActionMaskCollection actionMaskCollection;

        // ML-Agents BehaviourParameters to attain current setup information
        private BehaviorParameters behaviorParameters;

        // ML-Agent specific information collection fields
        private int[] branchSizeArray;
        private int branchArrayOverallSize;
        private int observationArrayOverallSize;
        private string modelName;

        // Set if observations has been created or changed, to avoid creating already existing collections
        private bool observationCollectionHasChanged;
        private bool actionMaskCollectionHasChanged;

        // Observation size for warning message
        private int CurrentObservationSize 
        { 
            get 
            {
                if (ObservationCollection != null)
                    return ObservationCollection.CurrentSize;

                return 0;
            } 
        }

        // Public get, private set for passthrough
        public MLAHelperAgent Agent { get => agent; private set => agent = value; }
        public NNModel AgentModel { get => agentModel; private set => agentModel = value; }
        public ObservationCollection ObservationCollection { get => observationCollection; private set => observationCollection = value; }
        public ActionMaskCollection ActionMaskCollection { get => actionMaskCollection; private set => actionMaskCollection = value; }

        // Deploy MLA-Helper in scene if currently not in scene. Will create nessecary collections if needed, passes information to FullModelBuilder
        [EnableIf("@parametersFound && FindAnyObjectByType<ModelConstructor>() == null")]
        [HorizontalGroup("Deploy", Width = 0.5f)]
        [Button("Deploy MLA-Helper In Current Scene", ButtonSizes.Large)]
        private void DeployInScene() {
            CreateActionMaskCollectionScriptableObject();
            CreateObservationCollectionScriptableObject();

            GameObject deployPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(SettingsHelper.GetSettings().DeployPrefabPath);
            ModelConstructor fullModelBuilder = Instantiate(deployPrefab).GetComponent<ModelConstructor>();
            fullModelBuilder.Initialize(this);
            InitializeAgent();
            CreateEventSystem();
        }

        // Deploy MLA-Helper information to scene if MLA-Helper already exists. Will create nessecary collections if needed, passes information to FullModelBuilder
        [EnableIf("@parametersFound && FindAnyObjectByType<ModelConstructor>() != null")]
        [HorizontalGroup("Deploy", Width = 0.5f)]
        [Button("Deploy information to MLA-Helper In Current Scene", ButtonSizes.Large)]
        private void DeployCollectionsToModelBuilderInScene() {
            CreateActionMaskCollectionScriptableObject();
            CreateObservationCollectionScriptableObject();
            FindAnyObjectByType<ModelConstructor>().Initialize(this);
            InitializeAgent();
            CreateEventSystem();
        }

        private void InitializeAgent() {
            if (agent != null) {
                agent.Setup(observationCollection, actionMaskCollection);
            }
        }

        // Called during collection creation and if the entities are changed, indicating that creation has happened
        private void SettingObservationAsChanged() {
            observationCollectionHasChanged = true;
        }

        private void SettingActionMasksAsChanged() {
            actionMaskCollectionHasChanged = true;
        }

        // Called when an Agent is loaded, and gets all relevant information from agent and its behaviourParameters
        [ShowIf("parametersFound")]
        [HorizontalGroup("Agent/General/Row1", LabelWidth = 200)]
        private void GetAgentInformation() {
            if (Agent != null) {
                if (Agent.TryGetComponent(out behaviorParameters)) {
                    SetModel();
                    GetInformationFromParameters();
                    CreateActionMaskCollection();
                    CreateObservationCollection();
                    
                    actionMaskCollectionHasChanged = false;
                    parametersFound = true;
                } else {
                    Debug.LogWarning("Loaded agent does not have a BehaviorParameters component attached.");
                }
            } 
        }

        // Create Action mask collection ScriptableObject if it has yet to be created
        [SerializeField]
        [HideIf("@!parametersFound")]
        [HorizontalGroup("Agent/Collections/Row2", Order = -1)]
        [VerticalGroup("Agent/Collections/Row2/Right")]
        [Button]
        private void CreateActionMaskCollectionScriptableObject() {
            string filePath = AssetUtility.GenerateUniqueAssetPath(SettingsHelper.GetSettings().ActionMaskCollectionPath, ActionMaskCollection.Name, ".asset");
            if (!string.IsNullOrEmpty(ActionMaskCollection.Name) && !actionMaskCollectionHasChanged) {
                actionMaskCollectionHasChanged = true;
                AssetDatabase.CreateAsset(ActionMaskCollection, filePath);
                AssetDatabase.SaveAssets();
            }
        }

        // Create observation collection ScriptableObject if it has yet to be created
        [SerializeField]
        [HideIf("@!parametersFound")]
        [HorizontalGroup("Agent/Collections/Row2", Order = -1)]
        [VerticalGroup("Agent/Collections/Row2/Left")]
        [Button]
        private void CreateObservationCollectionScriptableObject() {
            string filePath = AssetUtility.GenerateUniqueAssetPath(SettingsHelper.GetSettings().ObservationCollectionPath, ObservationCollection.Name, ".asset");
            if (!string.IsNullOrEmpty(ObservationCollection.Name) && !observationCollectionHasChanged) {
                observationCollectionHasChanged = true;
                AssetDatabase.CreateAsset(ObservationCollection, filePath);
                AssetDatabase.SaveAssets();
            }
        }

        private void CreateEventSystem() {
            if (FindAnyObjectByType<EventSystem>() == null) {
                GameObject eventSystem = new GameObject();
                eventSystem.AddComponent<EventSystem>();
                eventSystem.AddComponent<StandaloneInputModule>();
                eventSystem.name = "EventSystem";
            }
        }

        // Set model and model name for passthrough
        private void SetModel()
        {   if (behaviorParameters.Model != null)
                AgentModel = behaviorParameters.Model;
                modelName = AgentModel.name;
        }

        // Get needed information from ML-Agents behaviour- and brainParameters
        private void GetInformationFromParameters() {
            if (behaviorParameters.BrainParameters != null) {
                if (behaviorParameters.BrainParameters.ActionSpec.BranchSizes != null  && behaviorParameters.BrainParameters.ActionSpec.BranchSizes.Length > 0) {
                    branchSizeArray = behaviorParameters.BrainParameters.ActionSpec.BranchSizes;
                    branchArrayOverallSize = behaviorParameters.BrainParameters.ActionSpec.SumOfDiscreteBranchSizes;
                    observationArrayOverallSize = behaviorParameters.BrainParameters.VectorObservationSize;
                }
            }
        }

        // Creates Action Mask collection with correct size for currently loaded model, setting branch and index for each ActionMaskReference
        private void CreateActionMaskCollection() {
            if (branchSizeArray != null) {
                ActionMaskCollection = ScriptableObject.CreateInstance<ActionMaskCollection>();
                actionMaskCollectionHasChanged = false;
                ActionMaskCollection.Name = modelName + " Action mask Collection";
                ActionMaskCollection.actionMasks = new GLobalActionMaskReference[branchArrayOverallSize];
                int counter = 0;
                for (int branch = 0; branch < branchSizeArray.Length; branch++) {
                    for (int index = 0; index < branchSizeArray[branch]; index++) {
                        ActionMaskCollection.actionMasks[counter] = new GLobalActionMaskReference(SettingsHelper.GetSettings().CreateActionMaskCollectionsWithUseConstantDefault, branch, index);
                        counter++;
                    }
                }
            }
        }

        // Creates Observation collection
        private void CreateObservationCollection() {
            ObservationCollection = ScriptableObject.CreateInstance<ObservationCollection>();
            observationCollectionHasChanged = false;
            ObservationCollection.Name = modelName +  " Observation Collection";
            ObservationCollection.ObservationReferences = new GOReferenceParent[0];
        }

        // Fields used for odin inspector visuals
        #region Private Fields

        #pragma warning disable 0414
        private bool parametersFound = false;
        #pragma warning restore 0414

        #endregion
    }
}