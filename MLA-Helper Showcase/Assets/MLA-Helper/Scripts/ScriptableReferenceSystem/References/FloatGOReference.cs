using System;
using MLAHelper.ScriptableReferenceSystem.SO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace MLAHelper.ScriptableReferenceSystem.Reference
{
    [Serializable]
    [InlineProperty(LabelWidth = 100)]
    public class FloatGOReference : GOReferenceParent
    {
        [SerializeField]
        [HideIf("variable")]
        [HorizontalGroup("row")]
        [LabelText("Reference"), LabelWidth(60)]
        [PropertyOrder(100)]
        [OnValueChanged("SetName")]
        private FloatObservation variable;

        // Inspector setter and getter for the constant value
        [ShowInInspector]
        [HideIf("@!UseConstant || variable == null")]
        [HorizontalGroup("$Name/row", Width = 0.4f)]
        [HideLabel]
        private float constantValue {
            get 
            {
                if (variable != null)
                    return variable.ConstantValue;

                return 0;
            }
            set 
            {
                if (variable != null)
                    variable.ConstantValue = value;
            }
        }

        // Inspector getter for the dynamic value
        [ShowInInspector]
        [HideIf("@UseConstant || variable == null")]
        [HorizontalGroup("$Name/row", Width = 0.4f)]
        [HideLabel]
        private float dynamicValue 
        {
            get 
            {
                if (variable != null)
                    return variable.Value;

                return constantValue;
            }
        }

        // Get constant/dynamic value as float array for ML-Agents CollectObservations
        internal override float[] GetValue 
        {
            get
            {
                if (!UseConstant)
                    return new float[1]
                    {
                        variable.Value
                    };
                else 
                    return new float[1]
                    {
                        variable.ConstantValue
                    };
            }
        }

        // Setter and getter for user scripts - outside of MLA-Helper
        [HideInInspector]
        public float Value 
        {
            private get 
            { 
                if (!UseConstant) 
                    return variable.Value; 
                else
                    return constantValue; 
            }
            set 
            { 
                variable.Value = value; 
            }
        }

        // Setter and getter for child type variable
        internal override ScriptableObservationParent Variable 
        { 
            get 
            {
                return variable;
            } 
            set 
            { 
                variable = value as FloatObservation; 
            } 
        }

        // Construct Scriptable Observation
        protected override void GenerateScriptableObject()
        {
            Variable = ScriptableObject.CreateInstance<FloatObservation>();
            AssetDatabase.CreateAsset(Variable, SettingsHelper.GetSettings().IntObservationPath + Name + ".asset");
            AssetDatabase.SaveAssets();
            // AssetDatabase.Refresh();
        }

        // Get size for Observation collection to ensure correctly set observation space
        internal override int GetObservationSize() { return 1; }
    }
}