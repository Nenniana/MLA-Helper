using MLAHelper.Model.Structure;

// Visual information for Active layer prefabs
namespace MLAHelper.Model.Visuals {
    public class LayerVisualInformationActive : LayerVisualInformationParent
    {
        // Set colors and display information
        internal override void Initialize (ModelLayerParent modelLayer) {
            base.Initialize (modelLayer);

            background_Image.color = SettingsHelper.GetSettings().ActiveColor;
            SetTextColor(SettingsHelper.GetSettings().ActiveTextColor);

            ModelLayerActive modelLayerActive = modelLayer as ModelLayerActive;
            type_TextMesh.text = modelLayerActive.GetTypeName();
        }
    }
}