using System.Collections.Generic;
using UnityEngine;

namespace MLAHelper.ScriptableReferenceSystem.SO
{
    [CreateAssetMenu(fileName = "FloatListObservation", menuName = "Scriptable Observation/Float List Observation")]
    public class FloatListObservation : ScriptableObservationParent
    {
        public List<float> Value = new List<float>();
        public List<float> ConstantValue = new List<float>();
    }
}