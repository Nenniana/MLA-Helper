using Sirenix.OdinInspector;

namespace MLAHelper.ScriptableReferenceSystem.Reference
{
    public abstract class ReferenceParent {
        // Parent Construct Scriptable Object
        protected abstract void GenerateScriptableObject();

        // Parent Rename Button if ScriptableActionMask and reference have different names.
        [ShowIf("@Variable && Variable.name != name")]
        [Button("Rename Physical Scriptable Object")]
        protected abstract void Rename();

        // Parent Called on [OnValueChanged("SetName")] for variable
        protected abstract void SetName();
    }
}