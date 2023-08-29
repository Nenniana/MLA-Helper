using UnityEngine;

namespace MLAHelper.ScriptableReferenceSystem.SO
{
    [CreateAssetMenu(fileName = "FloatObservation", menuName = "Scriptable Observation/Float Observation")]
    public class FloatObservation : ScriptableObservationParent
    {
        public float Value;
        public float ConstantValue;
    }
}