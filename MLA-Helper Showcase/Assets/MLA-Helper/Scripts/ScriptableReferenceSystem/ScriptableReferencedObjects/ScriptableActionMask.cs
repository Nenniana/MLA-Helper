using UnityEngine;

namespace MLAHelper.ScriptableReferenceSystem.SO
{
    public class ScriptableActionMask : ScriptableObject
    {
        public bool Value = true;
        public bool ConstantValue = true;
        public bool UseConstant = true;

        // Dynamic branch information for action mask placement in onnx model
        public int Branch;
        // Dynamic index information for action mask placement in onnx model
        public int Index;
        // Constant branch information for action mask placement in onnx model
        public int ConstantBranch;
        // Constant index information for action mask placement in onnx model
        public int ConstantIndex;

        // Setting branch and index information on creation
        internal void SetPlacement(int branch, int index) {
            Branch = branch;
            ConstantBranch = branch;
            Index = index;
            ConstantIndex = index;
        }
    }
}