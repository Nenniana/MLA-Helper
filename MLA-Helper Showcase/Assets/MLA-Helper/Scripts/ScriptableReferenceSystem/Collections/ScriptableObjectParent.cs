using UnityEngine;
using Sirenix.OdinInspector;

namespace MLAHelper.ScriptableReferenceSystem.SO {
    [InlineEditor]
    public abstract class ScriptableObjectParent : ScriptableObject
    {   
        [Required]
        public string Name;
    }
}