using System.Collections.Generic;
using MLAHelper.ScriptableReferenceSystem.Reference;

// Action Layer Information Entity
namespace MLAHelper.Model.Structure {
    public class ModelLayerAction : ModelLayerParent
    {
        private float value;
        private Dictionary<int, GLobalActionMaskReference> modelLayerActionMasks;

        public ModelLayerAction (string layerName, ModelLayerParent inputLayer, float value) : base(layerName) {
            layerGeneralType = ModelLayerType.Action;
            inputLayers = new ModelLayerParent[] {inputLayer};
            this.value = value;

            CalculateID();
        }

        public float Value { get => value; private set => this.value = value; }

        internal string GetValue(int index)
        {
            return modelLayerActionMasks[index].Name;
        }

        internal void AddActionMask(int index, GLobalActionMaskReference actionMask) {
            if (modelLayerActionMasks == null)
                modelLayerActionMasks = new Dictionary<int, GLobalActionMaskReference>();

            modelLayerActionMasks.Add(index, actionMask);
        }
    }
}