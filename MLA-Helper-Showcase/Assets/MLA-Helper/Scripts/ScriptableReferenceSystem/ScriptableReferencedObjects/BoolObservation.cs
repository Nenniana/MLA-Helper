using UnityEngine;

namespace MLAHelper.ScriptableReferenceSystem.SO
{
    [CreateAssetMenu(fileName = "BoolObservation", menuName = "Scriptable Observation/Bool Observation")]
    public class BoolObservation : ScriptableObservationParent
    {
        public bool Value;
        public bool ConstantValue;
    }
}