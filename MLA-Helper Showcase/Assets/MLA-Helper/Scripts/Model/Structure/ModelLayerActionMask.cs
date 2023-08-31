using MLAHelper.ScriptableReferenceSystem.Reference;

// Action Mask Layer Information Entity
namespace MLAHelper.Model.Structure {
    public class ModelLayerActionMask : ModelLayerParent
    {
        private GLobalActionMaskReference boolReference;
        private bool value;

        public GLobalActionMaskReference BoolReference { get => boolReference; private set => boolReference = value; }
        public bool Value { get => value; private set => this.value = value; }

        public ModelLayerActionMask (string layerName, GLobalActionMaskReference boolReference, ModelLayerParent inputLayer, bool chosen)  : base(layerName) {
            layerGeneralType = ModelLayerType.ActionMask;
            inputLayers = new ModelLayerParent[]{inputLayer};
            value = chosen;
            this.boolReference = boolReference;

            SetName();
            CalculateID();
        }

        public string GetTypeName() {
            return layerGeneralType  + "_" + boolReference.Branch + "_" + boolReference.Index;
        }

        private void SetName() {
            if (!string.IsNullOrEmpty(boolReference.Name)) {
                layerName = boolReference.Name;
            } else {
                layerName = "Action_Mask";
            }
        }

        internal void SetInputLayers(ModelLayerParent deterministic_discrete_actions, ModelLayerParent discrete_actions) {
            inputLayers = new ModelLayerParent[2]{deterministic_discrete_actions, discrete_actions};
        }
    }
}