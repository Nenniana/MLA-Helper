using UnityEngine;
using TMPro;
using MLAHelper.Model.Structure;
using UnityEngine.UI;

// Visual information for layer prefab, sets text and color of child-classes
namespace MLAHelper.Model.Visuals {
    public class LayerVisualInformationParent : MonoBehaviour
    {
        protected TextMeshProUGUI name_TextMesh;
        protected TextMeshProUGUI type_TextMesh;
        protected TextMeshProUGUI value_TextMesh;
        protected Image background_Image;

        // Get all text and image elements on awake
        private void Awake() {
            name_TextMesh = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            type_TextMesh = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            value_TextMesh = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            background_Image = GetComponent<Image>();
            // 
        }

        // Generalized Initializa by ModelLayer
        internal virtual void Initialize (ModelLayerParent modelLayer) {
            name_TextMesh.text = modelLayer.LayerName;
            type_TextMesh.text = modelLayer.LayerGeneralType.ToString();
            // value_TextMesh.text = modelLayer.Value;
        }

        // Parent update visuals for updating after initial setup
        internal virtual void UpdateVisuals(float value) {}

        protected virtual void SetTextColor(Color32 color) {
            name_TextMesh.color = color;
            type_TextMesh.color = color;
            value_TextMesh.color = color;
        }
    }
}