using System;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;
using System.IO;
using MLAHelper.ScriptableReferenceSystem.SO;

namespace MLAHelper.ScriptableReferenceSystem.Reference
{
    [Serializable]
    public class GLobalActionMaskReference : ReferenceParent
    {
        // Reference name for inspector, before ScriptableActionMask is constructed
        [SerializeField]
        [HideIf("Variable")]
        [HorizontalGroup("$name/row")] 
        [LabelWidth(35)]
        private string name;

        // Reference branch for inspector and construction, before ScriptableActionMask is constructed
        [SerializeField]
        [HideIf("Variable")]
        [HorizontalGroup("$name/row", Width = 0.10f)]
        [LabelWidth(45)]
        private int branch;

        // Reference index for inspector and construction, before ScriptableActionMask is constructed
        [SerializeField]
        [HideIf("Variable")]
        [HorizontalGroup("$name/row", Width = 0.10f)]
        [LabelWidth(35)]
        private int index;

        [SerializeField]
        [HideIf("Variable")]
        [HorizontalGroup("$name/row")]
        [LabelText("Reference")]
        [LabelWidth(60)]
        [PropertyOrder(100)]
        [OnValueChanged("SetName")]
        private ScriptableActionMask Variable;

        // Button to toggle UseConstant - Toggles between use dynamic / use constant
        [Button("$toggleButtonName"), GUIColor("$toggleButtonColor")]
        [BoxGroup("$name", false)]
        [HorizontalGroup("$name/row", Width = 0.2f)]
        [ShowIf("Variable")]
        // [LabelWidth(60)]
        [PropertyOrder(3)]
        private void ToggleUseConstant() { UseConstant = !UseConstant; }

        // Button to toggle whether action mask is blocked or unblocked
        [Button("$actionMaskButtonName"), GUIColor("$actionMaskButtonColor")]
        [HorizontalGroup("$name/row", Width = 0.2f)]
        [EnableIf("UseConstant")]
        [ShowIf("Variable")]
        [PropertyOrder(2)]
        private void ToggleActionMask() { internalValue = !internalValue; }

        public GLobalActionMaskReference(bool useConstant, int branch, int index) {
             UseConstant = useConstant; 
             this.branch = branch; 
             this.index = index; 
        }

        // Getter and setter for name, after ScriptableActionMask has been constructed
        [ShowInInspector]
        [ShowIf("Variable")]
        [HideLabel]
        [HorizontalGroup("$name/row", Width = 0.4f)]
        [PropertyOrder(-1)]
        public string Name
        {
            get { return name; }
            private set { name = value; }
        }

        // Getter and setter for branch, after ScriptableActionMask has been constructed
        [ShowInInspector]
        [HorizontalGroup("$name/row", Width = 0.1f)]
        [LabelWidth(45)]
        [ShowIf("Variable")]
        internal int Branch
        {
            get
            {
                if (Variable != null)
                    return !Variable.UseConstant ? Variable.Branch : Variable.ConstantBranch;
                return branch;
            }
            private set
            {
                if (!Variable.UseConstant) Variable.Branch = value;
                else Variable.ConstantBranch = value;
            }
        }

        // Getter and setter for index, after ScriptableActionMask has been constructed
        [ShowInInspector]
        [HorizontalGroup("$name/row", Width = 0.1f)]
        [LabelWidth(35)]
        [ShowIf("Variable")]
        internal int Index
        {
            get
            {
                if (Variable != null)
                    return !Variable.UseConstant ? Variable.Index : Variable.ConstantIndex;
                return index;
            }
            private set
            {
                if (!Variable.UseConstant) Variable.Index = value;
                else Variable.ConstantIndex = value;
            }
        }

        // Setter and getter for user scripts - outside of MLA-Helper
        [ShowInInspector]
        [HideIf("@Variable == null || Variable != null")]
        public bool Value
        {
            get { return internalValue; }
            set { if (Variable != null) Variable.Value = value; }
        }

        // Setter and getter internal value, needed for toggling of value
        private bool internalValue
        {
            get
            {
                if (Variable != null)
                {
                    bool newValue = Variable.UseConstant ? Variable.ConstantValue : Variable.Value;
                    ToggleActionMaskChange(newValue);
                    return newValue;
                }
                return false;
            }
            set { if (Variable != null && Variable.UseConstant) Variable.ConstantValue = value; }
        }

        // Setter and getter global UseConstant
        protected bool UseConstant
        {
            get
            {
                ToggleChange();
                if (Variable != null) return Variable.UseConstant;
                return false;
            }
            set { if (Variable != null) { Variable.UseConstant = value; ToggleChange(); } }
        }

        // Visuals for Constant/Dynamic toggle
        private void ToggleChange()
        {
            if (Variable != null)
            {
                if (Variable.UseConstant)
                {
                    toggleButtonColor = new Color(0.4f, 0.8f, 1);
                    toggleButtonName = "Using Constant";
                }
                else
                {
                    toggleButtonColor = new Color32(243, 109, 134, 255);
                    toggleButtonName = "Using Dynamic";
                }
            }
        }

        // Visuals for Blocked/Unblocked toggle
        private void ToggleActionMaskChange(bool actionBool)
        {
            if (Variable != null)
            {
                if (actionBool)
                {
                    actionMaskButtonColor = new Color(0, 0.9f, 0);
                    actionMaskButtonName = "Action Unblocked";
                }
                else
                {
                    actionMaskButtonColor = new Color32(243, 109, 134, 255);
                    actionMaskButtonName = "Action Blocked";
                }
            }
        }

        // Inspector button for creation of ScriptableActionMask
        [HideIf("@this.UseConstant || this.Variable != null")]
        [HorizontalGroup("$name/row", Width = 0.25f, Order = 1)]
        [Button("Generate Reference"), GUIColor(0, 0.9f, 0)]
        protected override void GenerateScriptableObject()
        {
            GenerateScriptableActionMask(name, SettingsHelper.GetSettings().BoolActionMaskPath);
        }

        // Construction of ScriptableActionMask
        internal void GenerateScriptableActionMask(string _name, string path, bool save = true)
        {
            Variable = ScriptableObject.CreateInstance<ScriptableActionMask>();
            Variable.SetPlacement(branch, index);
            name = AssetUtility.CreateOrGetUniqueAsset(path, _name, ".asset");;
            ToggleChange();
            ToggleActionMaskChange(internalValue);
            AssetDatabase.CreateAsset(Variable, Path.Combine(path, name + ".asset"));

            if (save)
            {
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        // Rename Button if ScriptableActionMask and reference have different names.
        [ShowIf("@Variable && Variable.name != name")]
        [Button("Rename Physical Scriptable Object")]
        protected override void Rename()
        {
            if (!string.IsNullOrEmpty(name) && name != Variable.name)
            {
                string assetPath = AssetDatabase.GetAssetPath(Variable.GetInstanceID());
                AssetDatabase.RenameAsset(assetPath, name);
                AssetDatabase.SaveAssets();
            }
        }

        internal bool IsVariableNull()
        {
            return Variable == null;
        }

        // Called on [OnValueChanged("SetName")] for variable
        protected override void SetName()
        {
            if (Variable != null)
            {
                name = Variable.name;
            }
        }

        // Fields used for odin inspector visuals
        #region Private Fields

        #pragma warning disable 0414
        private string toggleButtonName = "Using Constant";
        private Color32 toggleButtonColor = new Color(243, 109, 134, 255);
        private string actionMaskButtonName = "Blocked";
        private Color32 actionMaskButtonColor = new Color(0.4f, 0.8f, 1);
        #pragma warning restore 0414

        #endregion
    }
}