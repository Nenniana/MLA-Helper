using UnityEditor;

// Static access to MLA-Helper Settings ScriptableObject
namespace MLAHelper {
    public static class SettingsHelper
    {
        public static MLAHelperSettings GetSettings() {
            return AssetDatabase.LoadAssetAtPath<MLAHelperSettings>("Assets/MLA-Helper/Settings/MLAHelperSettings/MLHelperSettingsSO_1.asset");
        }
    }
}