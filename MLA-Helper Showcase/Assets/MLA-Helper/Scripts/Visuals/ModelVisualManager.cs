using System.Linq;
using MLAHelper.Model.Structure;
using Unity.Mathematics;
using Unity.Burst;
using UnityEngine;
using System.Collections.Generic;
using MLAHelper.Model.Builder;

// Constructs all visuals for model visualization and indexes visuals by name, for updating on subsequent executes
namespace MLAHelper.Model.Visuals {
    public class ModelVisualManager
    {
        // LayerConstructor for data passthrough and notification when a new layer collection is ready
        private LayerConstructor layerConstructor;

        private Transform visualizatioParentTransform;

        private GameObject layerElementPrefab;

        private GameObject lineElementPrefab;

        private ModelLayerParent[] modelLayers;

        private Dictionary<string, LayerVisualInformationParent> visualsByName;

        public Dictionary<string, LayerVisualInformationParent> VisualsByName { get => visualsByName; private set => visualsByName = value; }

        // Constructor setting layerContructor, prefabs and transform, subscribes to IndexedLayersReady
        public ModelVisualManager(LayerConstructor layerConstructor, GameObject layerElementPrefab, GameObject lineElementPrefab, Transform visualizatioParentTransform) {
            this.layerConstructor = layerConstructor;
            this.layerElementPrefab = layerElementPrefab;
            this.lineElementPrefab = lineElementPrefab;
            this.visualizatioParentTransform = visualizatioParentTransform;

            layerConstructor.IndexedLayersReady += OnIndexedLayersReady;
        }

        // Deconstructor, unsubscribing
        ~ModelVisualManager()
        {
            layerConstructor.IndexedLayersReady -= OnIndexedLayersReady;
        }

        // Construct VisualsByName to update layer visuals on coming passes, flattens array, topological sort and instantiates layer and line prefabs
        private void OnIndexedLayersReady()
        {   
            VisualsByName = new Dictionary<string, LayerVisualInformationParent>();
            modelLayers = FlattenDictionaryToArray();
            ModelTopologicalDistanceSort.Sort(ref modelLayers);
            SetUpLayerVisualInformations();
        }

        // Create layer and line visuals for each layer
        [BurstCompile]
        private void SetUpLayerVisualInformations()
        {
            for (int i = 0; i < modelLayers.Length; i++)
            {
                ModelLayerParent layer = modelLayers[i];
                SetUpLayerVisualInformation(layer);
                SetUpLines(layer);
            }
        }

        // Flatten array
        public ModelLayerParent[] FlattenDictionaryToArray()
        {
            return layerConstructor.IndexedLayers.Values.SelectMany(layer => layer.ToArray()).ToArray();
        }
        
        // Setup bezier curves for connection between layers
        [BurstCompile]
        private void SetUpLines(ModelLayerParent layer)
        {   
            if (layer.InputLayers != null && layer.InputLayers.Length > 0) {
                for (int i = 0; i < layer.InputLayers.Length; i++) {
                    ConstructLines(ConstructBezierWayPoints(layer.GridPosition, layer.InputLayers[i].GridPosition));
                }
            }
        }

        // Setup bezier curve points for quadratic bezier curve
        [BurstCompile]
        private Vector3[] ConstructBezierWayPoints(Vector2Int startPosition, Vector2Int endPosition) {
            Vector3[] bezierPoints = new Vector3[4];

            bezierPoints[0] = new Vector3(startPosition.x * SettingsHelper.GetSettings().LayerHorizontalSpacing, startPosition.y * SettingsHelper.GetSettings().LayerVerticalSpacing);
            bezierPoints[1] = new Vector3((endPosition.x * SettingsHelper.GetSettings().LayerHorizontalSpacing) + (SettingsHelper.GetSettings().LayerHorizontalSpacing / 2), startPosition.y * SettingsHelper.GetSettings().LayerVerticalSpacing);
            bezierPoints[2] = new Vector3((endPosition.x * SettingsHelper.GetSettings().LayerHorizontalSpacing) + (SettingsHelper.GetSettings().LayerHorizontalSpacing / 2), endPosition.y * SettingsHelper.GetSettings().LayerVerticalSpacing);
            bezierPoints[3] = new Vector3(endPosition.x * SettingsHelper.GetSettings().LayerHorizontalSpacing, endPosition.y * SettingsHelper.GetSettings().LayerVerticalSpacing);

            return bezierPoints;
        }

        // Sets position for layer visuals and constructs prefab
        [BurstCompile]
        private void SetUpLayerVisualInformation(ModelLayerParent modelLayer) {
            Vector3 position = new Vector3(modelLayer.GridPosition.x * SettingsHelper.GetSettings().LayerHorizontalSpacing, modelLayer.GridPosition.y * SettingsHelper.GetSettings().LayerVerticalSpacing, 0);
            CreateVisualLayerInformation(position, modelLayer);
        }

        // Sets LayerVisualInformation per type, and instantiates prefab
        [BurstCompile]
        private void CreateVisualLayerInformation(Vector3 position, ModelLayerParent modelLayer)
        {
            GameObject layerVisual = GameObject.Instantiate(layerElementPrefab, position, quaternion.identity);

            LayerVisualInformationParent visualComponent;

            switch (modelLayer.LayerGeneralType)
            {
                case ModelLayerType.Observation:
                    visualComponent = layerVisual.AddComponent<LayerVisualInformationObservation>();
                    VisualsByName.Add(modelLayer.LayerOriginalName, visualComponent);
                    break;
                case ModelLayerType.ActionMask:
                    visualComponent = layerVisual.AddComponent<LayerVisualInformationActionMask>();
                    VisualsByName.Add(modelLayer.LayerOriginalName, visualComponent);
                    break;
                case ModelLayerType.Action:
                    visualComponent = layerVisual.AddComponent<LayerVisualInformationAction>();
                    VisualsByName.Add(modelLayer.LayerOriginalName, visualComponent);
                    break;
                default:
                    visualComponent = layerVisual.AddComponent<LayerVisualInformationActive>();
                    break;
            }

            visualComponent.Initialize(modelLayer);
            layerVisual.transform.SetParent(visualizatioParentTransform, false);
        }

        // Instantiates line prefabs for bezier curves
        [BurstCompile]
        private void ConstructLines(Vector3[] positions) {
            GameObject line = GameObject.Instantiate(lineElementPrefab);
            line.transform.SetParent(visualizatioParentTransform, false);
            LineRendererSmoother smoother = line.GetComponent<LineRendererSmoother>();
            smoother.InitializeBezierCurve(positions, 0.1f, 30);
        }
    }
}
