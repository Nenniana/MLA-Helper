using System;
using UnityEditor;
using Sirenix.OdinInspector;
using UnityEngine;
using MLAHelper.ScriptableReferenceSystem.SO;

namespace MLAHelper.ScriptableReferenceSystem.Reference
{
    [Serializable]
    public abstract class GOReferenceParent : ReferenceParent
    {
        // Button to toggle UseConstant - Toggles between use dynamic / use constant
        [BoxGroup("$Name", false)]
        [HorizontalGroup("$Name/row", Width = 0.2f, Order = -1)]
        [ShowIf("Variable")]
        [Button("$toggleButtonName"), GUIColor("$toggleButtonColor")]
        private void ToggleUseConstant() { UseConstant = !UseConstant; }

        [SerializeField]
        [HorizontalGroup("row", Width = 0.35f, Order = -1)]
        [LabelWidth(35)]
        [HideIf("Variable")]
        private string name;

        // Parent GetValue for MLA-Agent internal functions
        internal virtual float[] GetValue { get; private set; }
        
        // Parent Variable for MLA-Agent internal functions
        internal abstract ScriptableObservationParent Variable { get; set; }

        // Parent Get size for Observation collection to ensure correctly set observation space 
        internal virtual int GetObservationSize() {
            if (Variable != null) {
                return GetValue.Length - 1;
            }

            return 0;
        }

        // Getter and setter for name, after ScriptableActionMask has been constructed
        [ShowInInspector]
        [ShowIf("Variable")]
        [HideLabel]
        [HorizontalGroup("$Name/row", Width = 0.40f)]
        [PropertyOrder(-1)]
        public string Name 
        {
            get 
            {
                return name;
            }
            private set
            {
                name = value;
            }
        }

        // Use constant or dynamic values
        protected bool UseConstant 
        { 
            get 
            { 
                ToggleChange();
                if (Variable != null)
                    return Variable.UseConstant;
                
                return true;
            } 
            set 
            {
                if (Variable != null) {
                    Variable.UseConstant = value;
                    ToggleChange();
                }
            } 
        }

        // Parent Construct Scriptable Observation - Setting values for inspector visuals
        [HorizontalGroup("row", Width = 0.25f, Order = 1)]
        [Button("Generate Reference"), GUIColor(0, 0.9f, 0)]
        [HideIf("Variable")]
        protected override void GenerateScriptableObject() {}

        // Visuals for Constant/Dynamic toggle
        private void ToggleChange() {
            if (Variable != null) {
                if (Variable.UseConstant) {
                    toggleButtonColor = new Color (0.4f, 0.8f, 1);
                    toggleButtonName = "Using Constant";
                } else {
                    toggleButtonColor = new Color32 (243, 109, 134, 255);
                    toggleButtonName = "Using Dynamic";
                }
            }        
        }

        // Rename Button if ScriptableActionMask and reference have different names.
        [ShowIf("@Variable && Variable.name != name")]
        [Button("Rename Physical Scriptable Object")]
        protected override void Rename () {
            if (!string.IsNullOrEmpty(name) && name != Variable.name) {
                string assetPath =  AssetDatabase.GetAssetPath(Variable.GetInstanceID());
                AssetDatabase.RenameAsset(assetPath, name);
                AssetDatabase.SaveAssets();
            }
        }

        // Called on [OnValueChanged("SetName")] for variable
        protected override void SetName() {
            if (Variable != null) {
                name = Variable.name;
            }
        }

        // Fields used for odin inspector visuals
        #region Private Fields

        // Disable 'never used' warning
        #pragma warning disable 0414
        private string toggleButtonName = "Using Constant";
        private Color32 toggleButtonColor = new Color (0.4f, 0.8f, 1);
        #pragma warning restore 0414

        #endregion
    }
}