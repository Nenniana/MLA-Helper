using Unity.Barracuda;

// Active Layer Information Entity
namespace MLAHelper.Model.Structure {
    public class ModelLayerActive : ModelLayerParent
    {
        private Layer.Type layerType;
        private Layer.Activation activationType;
        private float[] values;

        public float[] Values { get => values; private set => values = value; }

        public ModelLayerActive (string layerName, Layer.Type layerType, Layer.Activation activationType, ModelLayerParent[] inputLayers, float[] values) : base(layerName) {
            layerGeneralType = ModelLayerType.Active;
            this.layerType = layerType;
            this.activationType = activationType;
            this.inputLayers = inputLayers;
            this.values = values;

            CalculateID();
        }

        protected override void CalculateID () { 
            base.CalculateID();

            layerID += "_" + layerType.ToString() + "_" + activationType.ToString();
        }
    }
}