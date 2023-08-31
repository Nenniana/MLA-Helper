using UnityEngine;

namespace MLAHelper.ScriptableReferenceSystem.SO
{
    [CreateAssetMenu(fileName = "QuaternionObservation", menuName = "Scriptable Observation/Quaternion Observation")]
    public class QuaternionObservation : ScriptableObservationParent
    {
        public Quaternion Value;
        public Quaternion ConstantValue;
    }
}