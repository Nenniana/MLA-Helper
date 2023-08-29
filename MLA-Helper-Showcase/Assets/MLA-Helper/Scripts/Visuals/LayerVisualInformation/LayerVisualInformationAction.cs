using MLAHelper.Model.Structure;

// Visual information for Action layer prefabs
namespace MLAHelper.Model.Visuals {
    public class LayerVisualInformationAction : LayerVisualInformationParent
    {
        // Save reference to get correctly chosen action
        private ModelLayerAction modelLayerAction;

        // Set colors and display information
        internal override void Initialize (ModelLayerParent modelLayer) {
            base.Initialize (modelLayer);

            modelLayerAction = modelLayer as ModelLayerAction;
            background_Image.color = SettingsHelper.GetSettings().ActionColor;
            SetTextColor(SettingsHelper.GetSettings().ActionTextColor);

            value_TextMesh.text = modelLayerAction.GetValue((int)modelLayerAction.Value);
        }

        // Update with name of new chosen action
        internal override void UpdateVisuals(float value) {
            value_TextMesh.text = modelLayerAction.GetValue((int)value);
        }
    }
}