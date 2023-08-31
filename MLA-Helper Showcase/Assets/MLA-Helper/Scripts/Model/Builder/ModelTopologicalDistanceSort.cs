using System.Collections.Generic;
using MLAHelper.Model.Structure;
using UnityEngine;

// Topologi Based Sort for gridposition of Model Layers. Sorts layer based on dependencies in their inputlayers array
namespace MLAHelper.Model.Builder {
    public static class ModelTopologicalDistanceSort
    {
        public static void Sort(ref ModelLayerParent[] layers)
        {
            // Calculate the depth (number of dependent layers) for each layer
            Dictionary<ModelLayerParent, int> depthMap = new Dictionary<ModelLayerParent, int>();
            foreach (var layer in layers)
            {
                int depth = CalculateDepth(layer, depthMap);
                depthMap[layer] = depth;
            }
            
            Dictionary<int, List<ModelLayerParent>> layersByColumn = new Dictionary<int, List<ModelLayerParent>>();
            // Topological sort and assign layers to columns
            for (int i = 0; i < layers.Length; i++)
            {
                int column = depthMap[layers[i]];

                if (!layersByColumn.ContainsKey(column))
                    layersByColumn.Add(column, new List<ModelLayerParent> {layers[i]});
                else
                    layersByColumn[column].Add(layers[i]);
            }

            // Assign gridPosition by column and row
            foreach (var layer in layersByColumn) {
                int row = 0;
                foreach (ModelLayerParent modelLayer in layer.Value) {
                    modelLayer.SetGridPosition(new Vector2Int(layer.Key, row - (layer.Value.Count / 2)));
                    row++;
                }
            }
        }

        // Calculates layer depth based on dependencies
        private static int CalculateDepth(ModelLayerParent layer, Dictionary<ModelLayerParent, int> depthMap)
        {
            if (depthMap.ContainsKey(layer))
            {
                return depthMap[layer];
            } 
            else if (layer.InputLayers == null || layer.InputLayers.Length == 0)
            {
                if (layer.LayerGeneralType == ModelLayerType.Observation) {
                    return 0;
                }
                return 1;
            }

            int maxDepth = 1;
            foreach (var inputLayer in layer.InputLayers)
            {
                int depth = CalculateDepth(inputLayer, depthMap);
                maxDepth = Mathf.Max(maxDepth, depth + 1);
            }

            return maxDepth;
        }
    }
}