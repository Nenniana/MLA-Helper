using UnityEngine;

namespace MLAHelper.Model.Structure {
    public abstract class ModelLayerParent
    {

        protected string layerName;
        protected string layerID;
        protected string layerOriginalName;
        protected ModelLayerType layerGeneralType;
        protected Vector2Int gridPosition;
        protected ModelLayerParent[] inputLayers;

        public string LayerName { get => layerName; private set => layerName = value; }
        public string LayerID { get => layerID; private set => layerID = value; }
        public Vector2Int GridPosition { get => gridPosition; private set => gridPosition = value; }
        public ModelLayerType LayerGeneralType { get => layerGeneralType; private set => layerGeneralType = value; }
        public ModelLayerParent[] InputLayers { get => inputLayers; private set => inputLayers = value; }
        public string LayerOriginalName { get => layerOriginalName; private set => layerOriginalName = value; }

        protected ModelLayerParent (string layerName) {
            this.layerName = layerName;
            this.layerOriginalName = layerName;
        }

        // Calculates Unique ID
        protected virtual void CalculateID()
        {
            layerID = LayerName + "_" + LayerGeneralType.ToString() + "_" + GridPosition.ToString();
        }

        // Internal setter for gridPosition
        internal void SetGridPosition(Vector2Int gridPosition) {
            this.gridPosition = gridPosition;
        }
    }
}