using UnityEngine;
using Sirenix.OdinInspector;
using System;
using UnityEditor;
using MLAHelper.ScriptableReferenceSystem.SO;
using MLAHelper.ScriptableReferenceSystem.Reference;

// Collection of action masks used as input for model visualization and ML-Agents
namespace MLAHelper.ScriptableReferenceSystem.Collection
{
    [CreateAssetMenu(fileName = "ActionMaskCollection", menuName = "Scriptable Observation/ActionMask Collection", order = 1)]
    [Serializable]
    [InlineEditor]
    public class ActionMaskCollection : ScriptableObjectParent
    {
        // TODO: Write warning box about model visualization values being of previous world state.

        // Array of action mask references
        [SerializeReference]
        [InlineProperty]
        [ListDrawerSettings(DraggableItems = false, Expanded = true, ShowIndexLabels = false, ShowPaging = false, HideRemoveButton = false)]
        [HideReferenceObjectPicker]
        public GLobalActionMaskReference[] actionMasks = new GLobalActionMaskReference[0];

        // Folderpath for user input
        [SerializeField]
        [HorizontalGroup("Fill Collection")]
        [HideIf("@actionMasks == null || actionMasks.Length <= 0")]
        [FolderPath(RequireExistingPath = true)]
        [LabelWidth(80)]
        [PropertyTooltip("Set the path for all physical masks for this collection, created by 'CreateAllUnintializedActionMasks'.")]
        private string folderPath;

        // Loops through current actionmask references and constructs ScriptableActionMasks with relevant names for each
        [Button]
        [PropertyOrder(-1)]
        [HideIf("@actionMasks == null || actionMasks.Length <= 0")]
        [DisableIf("@string.IsNullOrEmpty(Name)")]
        [HorizontalGroup("Fill Collection", Width = 0.4f)]
        [PropertyTooltip("Will create action masks for all current references without a physical mask. Will use given name or by collection_branch_index name. Will be created in default folder if path is empty.")]
        private void CreateAllUnintializedActionMasks() {
            string collectionReferenceFolderPath = SetFolderPath();
            if (actionMasks.Length != 0) {
                for (int i = 0; i < actionMasks.Length; i++) {
                    if (actionMasks[i].IsVariableNull())
                        CreateActionMaskSO(actionMasks[i], collectionReferenceFolderPath);
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        // Sets folderPath to be either defined by user, or in a new folder designated by the system
        private string SetFolderPath() {
            if (!string.IsNullOrEmpty(folderPath)) {
                return folderPath;
            }

            return AssetUtility.CreateUniqueFolder(SettingsHelper.GetSettings().BoolActionMaskPath, Name);
        }

        // Creates ScriptableActionMask at path with desired name
        private void CreateActionMaskSO(GLobalActionMaskReference actionMaskReference, string collectionReferenceFolderPath)
        {
            string actionMaskSoName = CreateActionMaskSOName(actionMaskReference);
            actionMaskReference.GenerateScriptableActionMask(actionMaskSoName, collectionReferenceFolderPath, false);
        }

        // Sets ScriptableActionMask name as designated by user or by model name + branch + index
        private string CreateActionMaskSOName(GLobalActionMaskReference boolReference) {
            string referenceName;
            if (!string.IsNullOrEmpty(boolReference.Name)) {
                referenceName = boolReference.Name;
            } else {
                referenceName = Name;
                referenceName += "_" + boolReference.Branch.ToString();
                referenceName += "_" + boolReference.Index.ToString();
            }

            return referenceName;
        }
    }
}