using UnityEngine;

namespace MLAHelper.ScriptableReferenceSystem.SO
{
    [CreateAssetMenu(fileName = "Vector3Observation", menuName = "Scriptable Observation/Vector3 Observation")]
    public class Vector3Observation : ScriptableObservationParent
    {
        public Vector3 Value;
        public Vector3 ConstantValue;
    }
}