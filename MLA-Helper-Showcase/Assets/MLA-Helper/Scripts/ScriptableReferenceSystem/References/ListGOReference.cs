using System;
using System.Collections;
using System.Collections.Generic;
using MLAHelper.ScriptableReferenceSystem.SO;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace MLAHelper.ScriptableReferenceSystem.Reference
{
    public class ListGOReference : GOReferenceParent
    {
        // Event delegate for updating size information special to the ListGoReference
        public Action<int, int> SizeChangedFromTo;

        private int oldSize, newSize = 0;

        [SerializeField]
        [HideIf("variable")]
        [HorizontalGroup("row")]
        [LabelText("Reference"), LabelWidth(60)]
        [PropertyOrder(100)]
        [OnValueChanged("SetName")]
        private FloatListObservation variable;

        // Inspector setter and getter for the constant value
        [ShowInInspector]
        [HideIf("@!UseConstant || variable == null")]
        [HorizontalGroup("$Name/row")]
        [HideLabel]
        private List<float> constantValue {
            get 
            {
                if (variable != null)
                    return variable.ConstantValue;

                return null;
            }
            set 
            {
                if (variable != null) {
                    variable.ConstantValue = value;
                    newSize = GetValue.Length;
                    CheckIfSizeChanged();
                }
            }
        }

        // Inspector getter for the dynamic value
        [ShowInInspector]
        [HideIf("@UseConstant || variable == null")]
        [HorizontalGroup("$Name/row")]
        [HideLabel]
        private List<float> dynamicValue 
        {
            get 
            {
                if (variable != null) {
                    newSize = GetValue.Length;
                    CheckIfSizeChanged();
                    return variable.Value;
                }

                return null;
            }
        }

        // Get constant/dynamic value as float array for ML-Agents CollectObservations
        [ShowInInspector]
        [HideIf("UseConstant")]
        [HorizontalGroup("$Name/row")]
        private int size {
            set
            {
                if ((value - variable.Value.Count) > 0)
                    variable.Value.AddRange(new float[value - variable.Value.Count]);
                else
                    variable.Value.RemoveRange(value, math.abs(value - variable.Value.Count));

                newSize = GetValue.Length;
            }
            get 
            {
                if (variable != null)
                    return variable.Value.Count;

                return 0;
            }
        }

        // Get constant/dynamic value as float array for ML-Agents CollectObservations
        internal override float[] GetValue 
        {
            get
            {
                if (!UseConstant)
                    return variable.Value.ToArray();
                else 
                    return variable.ConstantValue.ToArray();
            }
        }

        // Setter and getter for user scripts - outside of MLA-Helper
        [HideInInspector]
        public IList Value 
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
                variable.Value = value as List<float>;
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
                variable = value as FloatListObservation; 
            } 
        }

        // Update size information for list for observation collection to ensure correctly set observation space
        private void CheckIfSizeChanged() {
            if (oldSize != newSize) {
                SizeChangedFromTo?.Invoke(oldSize, newSize);
                oldSize = newSize;
            }
        }

        // Construct Scriptable Observation
        protected override void GenerateScriptableObject()
        {
            Variable = ScriptableObject.CreateInstance<FloatListObservation>();
            AssetDatabase.CreateAsset(Variable, SettingsHelper.GetSettings().FloatListObservationPath + Name + ".asset");
            AssetDatabase.SaveAssets();
            // AssetDatabase.Refresh();
        }

        // Unsubscribe all clients on finalize.
        ~ListGOReference() {
            Delegate[] clientList = SizeChangedFromTo.GetInvocationList();

            if (clientList != null) {
                foreach (Delegate client in clientList) {
                    SizeChangedFromTo -= (client as Action<int, int>);
                }
            }
        }
    }
}