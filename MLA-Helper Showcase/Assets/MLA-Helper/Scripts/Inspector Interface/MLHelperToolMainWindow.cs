using MLAHelper.ScriptableReferenceSystem.Collection;
using MLAHelper.ScriptableReferenceSystem.SO;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace MLAHelper.Interface {
    public class MLHelperToolMainWindow : OdinMenuEditorWindow
    {
        // Instance of new collection, enabling user to rapidly create specific collections
        private CreateNew<ObservationCollection> createNewObservationCollection;
        private CreateNew<ActionMaskCollection> createNewActionMaskCollection;

        // Settings page
        private MLAHelperSettings settings;

        // Agent loader page
        private LoadAgentHelperWindow agentHelperWindow;

        // Creates Menu Item to open MLA-Helper pages.
        [MenuItem("MLA-Helper/Main")]
        private static void OpenWindow()
        {
            GetWindow<MLHelperToolMainWindow>().Show();
        }

        // Odin Menu tree with settings, agent helper window, creation of collections, and list of all observations and action masks
        protected override OdinMenuTree BuildMenuTree() {
            settings = AssetDatabase.LoadAssetAtPath<MLAHelperSettings>("Assets/MLA-Helper/Settings/MLAHelperSettings/MLHelperSettingsSO_1.asset");
            agentHelperWindow = AssetDatabase.LoadAssetAtPath<LoadAgentHelperWindow>("Assets/MLA-Helper/Settings/AgentWindow/MainAgentWindowSO.asset");
            createNewObservationCollection = new CreateNew<ObservationCollection>("New Observation Collection", settings.ObservationCollectionPath);
            createNewActionMaskCollection = new CreateNew<ActionMaskCollection>("New Action Mask Collection", settings.ActionMaskCollectionPath);
            
            OdinMenuTree tree = new OdinMenuTree(true, GetTreeConfig()) {
                { "Setup Agent",                                    agentHelperWindow,                                          EditorIcons.Checkmark },
                { "MLHelpter Settings",                             settings,                                                   EditorIcons.SettingsCog },
                { "Create Observation Collection",                  createNewObservationCollection,                             EditorIcons.FileCabinet },
                { "Create Action Mask Collection",                  createNewActionMaskCollection,                              EditorIcons.FileCabinet },
                // { "General Information",                            null,                                                       EditorIcons.SpeechBubblesSquare }
            };
            
            tree.AddAllAssetsAtPath("Observation Collections", settings.ObservationCollectionPath, typeof(ObservationCollection), true).AddThumbnailIcons().SortMenuItemsByName();
            tree.AddAllAssetsAtPath("Action Mask Collections", settings.ActionMaskCollectionPath, typeof(ActionMaskCollection), true).AddThumbnailIcons().SortMenuItemsByName();

            return tree;
        }

        // Setup Odin Menu Tree with Search and caching expandend states
        private OdinMenuTreeDrawingConfig GetTreeConfig() {
            OdinMenuTreeDrawingConfig config = new OdinMenuTreeDrawingConfig
            {
                DrawSearchToolbar = true,
                DefaultMenuStyle = OdinMenuStyle.TreeViewStyle,
                UseCachedExpandedStates = true
            };
            return config;
        }

        // Generic creation of collection and setup hereof by user
        private class CreateNew<T> where T : ScriptableObjectParent {
            [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
            public T scriptableObject;

            private string path;
            private string name;
            private string createName;

            public CreateNew (string _name, string _path) {
                scriptableObject = CreateInstance<T>();
                scriptableObject.name = _name;
                path = _path;
                name = _name;
                createName = "Create " + name;
            }

            // Save collection as ScriptableObject
            [EnableIf("@!string.IsNullOrEmpty(scriptableObject.Name)")]
            [Button("$createName")]
            private void CreateNewData() {
                if (!string.IsNullOrEmpty(scriptableObject.Name)) {
                    AssetDatabase.CreateAsset(scriptableObject, path + scriptableObject.Name + ".asset");
                    AssetDatabase.SaveAssets();

                    scriptableObject = CreateInstance<T>();
                    scriptableObject.name = scriptableObject.Name;
                }
            }
        }

        // Generic deletion of collections
        private void DeleteIfType<T>(OdinMenuTreeSelection selectedMenuItem) where T : class {
            if (selectedMenuItem.SelectedValue is T) {
                T asset = selectedMenuItem.SelectedValue as T;
                string path = AssetDatabase.GetAssetPath(asset as ScriptableObject);
                AssetDatabase.DeleteAsset(path);
                AssetDatabase.Refresh();
            }
        }

        // Draw deletion buttons for collections
        protected override void OnBeginDrawEditors() {
            base.OnBeginDrawEditors();

            OdinMenuTreeSelection selectedMenuItem = MenuTree.Selection;

            SirenixEditorGUI.BeginHorizontalToolbar(); {
                GUILayout.FlexibleSpace();
                if (selectedMenuItem.SelectedValue is ScriptableObjectParent) {
                    GUILayout.Label("Delete:");
                    if (SirenixEditorGUI.ToolbarButton(EditorIcons.AlertTriangle, false)) {
                        
                        if (selectedMenuItem != null && selectedMenuItem.SelectedValue != null) {
                            DeleteIfType<ObservationCollection>(selectedMenuItem);
                            DeleteIfType<ActionMaskCollection>(selectedMenuItem);
                        }
                    }
                }
            }
            SirenixEditorGUI.EndHorizontalToolbar();
        }

        // OnDestroy if collections are not null
        protected override void OnDestroy() {
            base.OnDestroy();

            DestroyIfNotNull(createNewObservationCollection);
            DestroyIfNotNull(createNewActionMaskCollection);
        }

        private void DestroyIfNotNull<T>(T _object) {
            if (_object != null && _object as UnityEngine.Object) {
                DestroyImmediate(_object as UnityEngine.Object);
            }
        }
    }
}