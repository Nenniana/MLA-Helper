using MLAHelper.ScriptableReferenceSystem.SO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace MLAHelper.ScriptableReferenceSystem.Reference
{
    public class Vector3GOReference : GOReferenceParent
    {
        [SerializeField]
        [HideIf("variable")]
        [HorizontalGroup("row")]
        [LabelText("Reference"), LabelWidth(60)]
        [PropertyOrder(100)]
        [OnValueChanged("SetName")]
        private Vector3Observation variable;

        // Inspector setter and getter for the constant value
        [ShowInInspector]
        [HideIf("@!UseConstant || variable == null")]
        [HorizontalGroup("$Name/row")]
        [HideLabel]
        private Vector3 constantValue {
            get 
            {
                if (variable != null)
                    return variable.ConstantValue;

                return Vector3.zero;
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
        [HorizontalGroup("$Name/row")]
        [HideLabel]
        private Vector3 dynamicValue 
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
                    return new float[3]
                    {
                        variable.Value.x,
                        variable.Value.y,
                        variable.Value.z
                    };
                else 
                    return new float[3]
                    {
                        variable.ConstantValue.x,
                        variable.ConstantValue.y,
                        variable.ConstantValue.z
                    };
            }
        }

        // Setter and getter for user scripts - outside of MLA-Helper
        [HideInInspector]
        public Vector2 Value 
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
                variable = value as Vector3Observation; 
            } 
        }

        // Construct Scriptable Observation
        protected override void GenerateScriptableObject()
        {
            Variable = ScriptableObject.CreateInstance<Vector3Observation>();
            AssetDatabase.CreateAsset(Variable, SettingsHelper.GetSettings().Vector3ObservationPath + Name + ".asset");
            AssetDatabase.SaveAssets();
            // AssetDatabase.Refresh();
        }

        internal override int GetObservationSize() { return 3; }
    }
}