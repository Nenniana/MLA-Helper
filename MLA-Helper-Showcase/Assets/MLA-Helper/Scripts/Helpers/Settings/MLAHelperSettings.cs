using Sirenix.OdinInspector;
using UnityEngine;

// Scriptable Object containing all general settings for MLA-Helper, uses Odin Inspector for visualization
namespace MLAHelper {
    [CreateAssetMenu(fileName = "MLHelperSettingsSO", menuName = "MLHelper/Settings ScriptableObject", order = 1)]
    [InlineEditor]
    public class MLAHelperSettings : ScriptableObject
    {
        [SerializeField]
        [FoldoutGroup("Agent", expanded: true)]
        [BoxGroup("Agent/General")]
        [HorizontalGroup("Agent/General/Row1", LabelWidth = 150)]
        private bool createActionMaskCollectionsWithUseConstantDefault = true;

        [SerializeField]
        [FoldoutGroup("Agent", expanded: true)]
        [BoxGroup("Agent/General")]
        [HorizontalGroup("Agent/General/Row1", LabelWidth = 150)]
        private bool createObservationCollectionsWithUseConstantDefault = true;

        [SerializeField]
        [FoldoutGroup("Visualization", expanded: true)]
        [BoxGroup("Visualization/General")]
        [HorizontalGroup("Visualization/General/Row2", LabelWidth = 150)]
        private MLAHelperSettingsEnums.VisualizeOn visualizeOn = MLAHelperSettingsEnums.VisualizeOn.Start;

        [SerializeField]
        [HorizontalGroup("Visualization/General/Row2", LabelWidth = 150)]
        private MLAHelperSettingsEnums.ActionFocus actionTypeFocus = MLAHelperSettingsEnums.ActionFocus.discrete_actions;

        [SerializeField]
        [FoldoutGroup("Visualization", expanded: true)]
        [BoxGroup("Visualization/Colors")]
        [HorizontalGroup("Visualization/Colors/Row1", LabelWidth = 200)]
        [VerticalGroup("Visualization/Colors/Row1/Left")]
        private Color32 observationColor = new Color32(252, 218, 224, 255);

        [SerializeField]
        [VerticalGroup("Visualization/Colors/Row1/Right")]
        private Color32 observationTextColor = new Color32(30, 30, 30, 255);

        [SerializeField]
        [VerticalGroup("Visualization/Colors/Row1/Left")]
        private Color32 activeColor = new Color32(255, 255, 255, 255);

        [SerializeField]
        [VerticalGroup("Visualization/Colors/Row1/Right")]
        private Color32 activeTextColor = new Color32(30, 30, 30, 255);

        [SerializeField]
        [VerticalGroup("Visualization/Colors/Row1/Left")]
        private Color32 actionColor = new Color32(40, 255, 185, 255);

        [SerializeField]
        [VerticalGroup("Visualization/Colors/Row1/Right")]
        private Color32 actionTextColor = new Color32(30, 30, 30, 255);

        [SerializeField]
        [VerticalGroup("Visualization/Colors/Row1/Left")]
        private Color32 actionMaskBlockedColor = new Color32(165, 74, 91, 255);

        [SerializeField]
        [VerticalGroup("Visualization/Colors/Row1/Right")]
        private Color32 actionMaskBlockedTextColor = new Color32(30, 30, 30, 255);

        [SerializeField]
        [VerticalGroup("Visualization/Colors/Row1/Left")]
        private Color32 actionMaskChosenColor = new Color32(37, 62, 34, 255);

        [SerializeField]
        [VerticalGroup("Visualization/Colors/Row1/Right")]
        private Color32 actionMaskChosenTextColor = new Color32(30, 30, 30, 255);

        [SerializeField]
        [VerticalGroup("Visualization/Colors/Row1/Left")]
        private Color32 actionMaskUnblockedColor = new Color32(255, 235, 3, 255);

        [SerializeField]
        [VerticalGroup("Visualization/Colors/Row1/Right")]
        private Color32 actionMaskUnblockedTextColor = new Color32(30, 30, 30, 255);

        [SerializeField]
        [VerticalGroup("Visualization/Colors/Row1/Left")]
        private Color32 visualizationBackgroundColor = new Color32(30, 30, 30, 0);

        [SerializeField]
        [FoldoutGroup("Visualization", expanded: true)]
        [BoxGroup("Visualization/LayerSpacing")]
        [HorizontalGroup("Visualization/LayerSpacing/Row1", LabelWidth = 150)]
        private int layerHorizontalSpacing = 200;

        [SerializeField]
        [HorizontalGroup("Visualization/LayerSpacing/Row1")]
        private int layerVerticalSpacing = 200;

        [SerializeField]
        [FoldoutGroup("Visualization", expanded: true)]
        [BoxGroup("Visualization/ModelViewer")]
        [HorizontalGroup("Visualization/ModelViewer/Row2", LabelWidth = 150)]
        [VerticalGroup("Visualization/ModelViewer/Row2/Left")]
        private float scrollSensitivity = 1;

        [SerializeField]
        [VerticalGroup("Visualization/ModelViewer/Row2/Left")]
        private Vector2 dragSensitivity = new Vector2(0.035f, 0.035f);

        [SerializeField]
        [VerticalGroup("Visualization/ModelViewer/Row2/Right")]
        private Vector2 zoomPositionSensitivity = new Vector2(1, 1);

        [SerializeField]
        [VerticalGroup("Visualization/ModelViewer/Row2/Right")]
        private Vector2 resizeAreaSize = new Vector2(0.2f, 0.2f);

        [SerializeField]
        [FoldoutGroup("Paths", expanded: true)]
        [FolderPath(RequireExistingPath = true)]
        private string observationCollectionPath = "Assets/MLA-Helper/ScriptableObjects/Collections/Observations/";

        [SerializeField]
        [FoldoutGroup("Paths", expanded: true)]
        [FolderPath(RequireExistingPath = true)]
        private string actionMaskCollectionPath = "Assets/MLA-Helper/ScriptableObjects/Collections/ActionMasks/";

        [SerializeField]
        [FoldoutGroup("Paths", expanded: true)]
        [FolderPath(RequireExistingPath = true)]
        private string floatObservationPath = "Assets/MLA-Helper/ScriptableObjects/Floats/";
        
        [SerializeField]
        [FoldoutGroup("Paths", expanded: true)]
        [FolderPath(RequireExistingPath = true)]
        private string intObservationPath = "Assets/MLA-Helper/ScriptableObjects/Ints/";

        [SerializeField]
        [FoldoutGroup("Paths", expanded: true)]
        [FolderPath(RequireExistingPath = true)]
        private string boolObservationPath = "Assets/MLA-Helper/ScriptableObjects/Bools/";

        [SerializeField]
        [FoldoutGroup("Paths", expanded: true)]
        [FolderPath(RequireExistingPath = true)]
        private string vector2ObservationPath = "Assets/MLA-Helper/ScriptableObjects/Vector2s/";

        [SerializeField]
        [FoldoutGroup("Paths", expanded: true)]
        [FolderPath(RequireExistingPath = true)]
        private string vector3ObservationPath = "Assets/MLA-Helper/ScriptableObjects/Vector3s/";

        [SerializeField]
        [FoldoutGroup("Paths", expanded: true)]
        [FolderPath(RequireExistingPath = true)]
        private string quaternionObservationPath = "Assets/MLA-Helper/ScriptableObjects/Quaternions/";

        [SerializeField]
        [FoldoutGroup("Paths", expanded: true)]
        [FolderPath(RequireExistingPath = true)]
        private string floatListObservationPath = "Assets/MLA-Helper/ScriptableObjects/FloatLists/";

        [SerializeField]
        [FoldoutGroup("Paths", expanded: true)]
        [FolderPath(RequireExistingPath = true)]
        private string boolActionMaskPath = "Assets/MLA-Helper/ScriptableObjects/ActionMask/";

        [SerializeField]
        [FoldoutGroup("Paths", expanded: true)]
        [FolderPath]
        private string resizeCursorPath = "Assets/MLA-Helper/Images/cursor-resize-horizontal.png";

        private string deployPrefabPath = "Assets/MLA-Helper/Prefabs/MLA-Helper_Prefab.prefab";

        public string ActionMaskCollectionPath { get => actionMaskCollectionPath; private set => actionMaskCollectionPath = value; }
        public string ObservationCollectionPath { get => observationCollectionPath; private set => observationCollectionPath = value; }
        public string FloatObservationPath { get => floatObservationPath; private set => floatObservationPath = value; }
        public string IntObservationPath { get => intObservationPath; private set => intObservationPath = value; }
        public string Vector2ObservationPath { get => vector2ObservationPath; private set => vector2ObservationPath = value; }
        public string Vector3ObservationPath { get => vector3ObservationPath; private set => vector3ObservationPath = value; }
        public string QuaternionObservationPath { get => quaternionObservationPath; private set => quaternionObservationPath = value; }
        public string FloatListObservationPath { get => floatListObservationPath; private set => floatListObservationPath = value; }
        public string BoolActionMaskPath { get => boolActionMaskPath; private set => boolActionMaskPath = value; }
        public int LayerHorizontalSpacing { get => layerHorizontalSpacing; private set => layerHorizontalSpacing = value; }
        public int LayerVerticalSpacing { get => layerVerticalSpacing; private set => layerVerticalSpacing = value; }
        public float ScrollSensitivity { get => scrollSensitivity; private set => scrollSensitivity = value; }
        public Vector2 DragSensitivity { get => dragSensitivity; private set => dragSensitivity = value; }
        public Vector2 ZoomPositionSensitivity { get => zoomPositionSensitivity; private set => zoomPositionSensitivity = value; }
        public Vector2 ResizeAreaSize { get => resizeAreaSize; private set => resizeAreaSize = value; }
        public MLAHelperSettingsEnums.VisualizeOn VisualizeOn { get => visualizeOn; private set => visualizeOn = value; }
        public Color32 ObservationColor { get => observationColor; private set => observationColor = value; }
        public Color32 ObservationTextColor { get => observationTextColor; private set => observationTextColor = value; }
        public Color32 ActiveColor { get => activeColor; private set => activeColor = value; }
        public Color32 ActiveTextColor { get => activeTextColor; private set => activeTextColor = value; }
        public Color32 ActionColor { get => actionColor; private set => actionColor = value; }
        public Color32 ActionTextColor { get => actionTextColor; private set => actionTextColor = value; }
        public Color32 ActionMaskBlockedColor { get => actionMaskBlockedColor; private set => actionMaskBlockedColor = value; }
        public Color32 ActionMaskBlockedTextColor { get => actionMaskBlockedTextColor; private set => actionMaskBlockedTextColor = value; }
        public Color32 ActionMaskChosenColor { get => actionMaskChosenColor; private set => actionMaskChosenColor = value; }
        public Color32 ActionMaskChosenTextColor { get => actionMaskChosenTextColor; private set => actionMaskChosenTextColor = value; }
        public Color32 ActionMaskUnblockedColor { get => actionMaskUnblockedColor; private set => actionMaskUnblockedColor = value; }
        public Color32 ActionMaskUnblockedTextColor { get => actionMaskUnblockedTextColor; private set => actionMaskUnblockedTextColor = value; }
        public MLAHelperSettingsEnums.ActionFocus ActionTypeFocus { get => actionTypeFocus; private set => actionTypeFocus = value; }
        public bool CreateActionMaskCollectionsWithUseConstantDefault { get => createActionMaskCollectionsWithUseConstantDefault; private set => createActionMaskCollectionsWithUseConstantDefault = value; }
        public bool CreateObservationCollectionsWithUseConstantDefault { get => createObservationCollectionsWithUseConstantDefault; private set => createObservationCollectionsWithUseConstantDefault = value; }
        public string BoolObservationPath { get => boolObservationPath; private set => boolObservationPath = value; }
        public Color32 VisualizationBackgroundColor { get => visualizationBackgroundColor; private set => visualizationBackgroundColor = value; }
        public string DeployPrefabPath { get => deployPrefabPath; private set => deployPrefabPath = value; }
        public string ResizeCursorPath { get => resizeCursorPath; private set => resizeCursorPath = value; }
    }
}