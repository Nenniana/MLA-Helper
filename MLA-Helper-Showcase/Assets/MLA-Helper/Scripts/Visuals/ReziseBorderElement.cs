using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

// Tracks dragging of borders and informs clients with PointerEventData, changes cursor texture to indicate draggability
namespace MLAHelper.Model.Visuals {
    public class ReziseBorderElement : MonoBehaviour, IDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public Action<PointerEventData> DraggedInfo;
        private Texture2D texture2D;
        private CursorMode cursorMode = CursorMode.Auto;
        private Vector2 hotspot = Vector2.zero;

        public void OnDrag(PointerEventData eventData)
        {
            DraggedInfo?.Invoke(eventData);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            GetCursor();
            Cursor.SetCursor(texture2D, hotspot, CursorMode.ForceSoftware);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ReturnToNormalCursor();
        }

        private void ReturnToNormalCursor () {
            Cursor.SetCursor(null, Vector2.zero, cursorMode);
        }

        private void GetCursor() {
            if (texture2D == null) {
                texture2D = AssetDatabase.LoadAssetAtPath<Texture2D>(SettingsHelper.GetSettings().ResizeCursorPath);
                hotspot = new Vector2(texture2D.width / 2, texture2D.height / 2);
            }
        }
    }
}