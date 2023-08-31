using UnityEngine;

namespace MLAHelper.ScriptableReferenceSystem.SO
{
    [CreateAssetMenu(fileName = "IntObservation", menuName = "Scriptable Observation/Int Observation")]
    public class IntObservation : ScriptableObservationParent
    {
        public int Value;
        public int ConstantValue;
    }
}