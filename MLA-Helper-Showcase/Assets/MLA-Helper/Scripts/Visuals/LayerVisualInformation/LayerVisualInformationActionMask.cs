using UnityEngine;
using MLAHelper.Model.Structure;
using MLAHelper.ScriptableReferenceSystem.Reference;

// Visual information for Action layer prefabs
namespace MLAHelper.Model.Visuals {
    public class LayerVisualInformationActionMask : LayerVisualInformationParent
    {
        // Set colors and display information, show warning if an action mask that was blocked was the chosen action
        internal override void Initialize (ModelLayerParent modelLayer) {
            base.Initialize (modelLayer);

            ModelLayerActionMask modelLayerActionMask = modelLayer as ModelLayerActionMask;
            GLobalActionMaskReference boolReference = modelLayerActionMask.BoolReference;
            type_TextMesh.text = modelLayerActionMask.GetTypeName();

            if (modelLayerActionMask.Value && !boolReference.Value) {
                Debug.LogWarning ($"ModelLayerActionMask {modelLayerActionMask.LayerName} was chosen as the correct action, but should have been blocked by action mask!");
            }
            
            UpdateValueText(modelLayerActionMask.Value, boolReference.Value);
        }

        // Update information after initial setup
        internal void UpdateInformation(bool value, bool refValue) {
            UpdateValueText(value, refValue);
        }

        // Update visuals based on values
        private void UpdateValueText(bool actionMaskValue, bool referenceValue) {
            if (actionMaskValue) {
                background_Image.color = SettingsHelper.GetSettings().ActionMaskChosenColor;
                SetTextColor(SettingsHelper.GetSettings().ActionMaskChosenTextColor);
                value_TextMesh.text = "Chosen";
            }
            else if (referenceValue) {
                background_Image.color = SettingsHelper.GetSettings().ActionMaskUnblockedColor;
                SetTextColor(SettingsHelper.GetSettings().ActionMaskUnblockedTextColor);
                value_TextMesh.text = "Unblocked";
            }
            else {
                background_Image.color = SettingsHelper.GetSettings().ActionMaskBlockedColor;
                SetTextColor(SettingsHelper.GetSettings().ActionMaskBlockedTextColor);
                value_TextMesh.text = "Blocked";
            }
        }
    }
}