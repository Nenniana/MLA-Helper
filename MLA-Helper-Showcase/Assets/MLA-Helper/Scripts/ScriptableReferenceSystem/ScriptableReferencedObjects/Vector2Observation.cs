using UnityEngine;

namespace MLAHelper.ScriptableReferenceSystem.SO
{   
    [CreateAssetMenu(fileName = "Vector2Observation", menuName = "Scriptable Observation/Vector2 Observation")]
    public class Vector2Observation : ScriptableObservationParent
    {
        public Vector2 Value;
        public Vector2 ConstantValue;
    }
}