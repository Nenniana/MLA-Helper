using MLAHelper.Model.Structure;

// Visual information for Observation layer prefabs
namespace MLAHelper.Model.Visuals {
    public class LayerVisualInformationObservation : LayerVisualInformationParent
    {
        // Set colors and display information
        internal override void Initialize (ModelLayerParent modelLayer) {
            base.Initialize (modelLayer);
            background_Image.color = SettingsHelper.GetSettings().ObservationColor;
            SetTextColor(SettingsHelper.GetSettings().ObservationTextColor);

            ModelLayerObservation modelLayerObservation = modelLayer as ModelLayerObservation;
            value_TextMesh.text = modelLayerObservation.Value.ToString("0.00");
        }

        // Update with new observation information
        internal override void UpdateVisuals(float value) {
            value_TextMesh.text = value.ToString("0.00");
        }
    }
}