// Observation Layer Information Entity
namespace MLAHelper.Model.Structure {
    public class ModelLayerObservation : ModelLayerParent
    {
        private float value;
        public float Value { get => value; private set => this.value = value; }

        public ModelLayerObservation (string layerName, float value) : base(layerName) {
            layerGeneralType = ModelLayerType.Observation;
            this.value = value;

            CalculateID();
        }
    }
}