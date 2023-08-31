using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

// Allows for resizing, dragging of window and model, as well as zooming of visualization
namespace MLAHelper.Model.Visuals {
    public class ZoomDragUIWindow : MonoBehaviour, IDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private ReziseBorderElement[] border;
        [SerializeField]
        private Canvas canvas;

        [SerializeField]
        private RectTransform rectTransform;

        [SerializeField]
        private RectTransform modelGroupRectTransform;

        [SerializeField]
        private RectTransform modelparent;

        [SerializeField]
        [OnInspectorInit("SetCameraBackgroundColor")]
        private Camera visualizationCamera;

        [SerializeField]
        RenderTexture renderTexture;

        private bool isZoomable = false;

        // Subscribes to border information to resize window on drag
        private void OnEnable () {
            foreach (var element in border) {
                element.DraggedInfo += OnBorderDragged;
            }
        }

        // Ubsubscribe from borders
        private void OnDisable() {
            foreach (var element in border) {
                element.DraggedInfo -= OnBorderDragged;
            }
        }

        // Resize window on border drag
        private void OnBorderDragged(PointerEventData data)
        {
            ResizeWindow(data.delta);
        }

        // Set information and resize render texture used for window
        private void Awake()
        {
            SetCanvas();
            SetRectTransform();
            SetRenderTexture();
            ResizeRenderTexture(renderTexture, rectTransform.sizeDelta.x, rectTransform.sizeDelta.y);
        }

        // Called on inspector visualization if user changes background color in settings
        private void SetCameraBackgroundColor () {
            if (visualizationCamera != null)
                visualizationCamera.backgroundColor = SettingsHelper.GetSettings().VisualizationBackgroundColor;
        }

        private void Update() {
            if (isZoomable) {
                ScrollToZoom();
            }
        }

        // OnDrag on actual window, dragging model with left mouse button, and moving window with right.
        public void OnDrag(PointerEventData eventData)
        {
            if (Input.GetMouseButton(0)) {
                DragModel(eventData.delta);
            } else if (Input.GetMouseButton(1)) {
                MoveWindow(eventData.delta);
            }
        }

        // Resize window based on border drag
        private void ResizeWindow(Vector3 mouseDrag) {
            Vector2 pointerData = mouseDrag / canvas.scaleFactor;

            rectTransform.sizeDelta -= new Vector2(-pointerData.x, pointerData.y);
            ResizeRenderTexture(renderTexture, rectTransform.sizeDelta.x, rectTransform.sizeDelta.y);
        }

        private void MoveWindow(Vector2 mouseDrag) {
            rectTransform.anchoredPosition += mouseDrag / canvas.scaleFactor;
        }

        // Resizes render texture and updates visualizationCamera to ensure visualization is kept crisp, without being overscaled.
        private void ResizeRenderTexture(RenderTexture renderTexture, float width, float height)
        {
            if (renderTexture)
            {
                renderTexture.Release();
                renderTexture.width = Mathf.CeilToInt(width);
                renderTexture.height = Mathf.CeilToInt(height);
                visualizationCamera.Render();
            }
        }

        private void SetRenderTexture()
        {
            renderTexture = visualizationCamera.targetTexture;
        }

        // Scrolls camera based on scrollwheel input
        private void ScrollToZoom()
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0 || Input.GetAxis("Mouse ScrollWheel") < 0) {
                ScrollCamera(Input.GetAxis("Mouse ScrollWheel"));
            }
        }

        // Scales model if rectTransform is known
        private void ScrollCamera(float scrollValue) {
            if (modelGroupRectTransform != null) {
                ScaleModel(scrollValue);
                // FocusZoomOnMousePosition(scrollValue);
            }
        }

        private void ScaleModel(float scrollValue) {
            float scale = scrollValue * SettingsHelper.GetSettings().ScrollSensitivity;
            modelGroupRectTransform.localScale += new Vector3(scale, scale, scale);
        }

        internal void SetCanvas(Canvas _canvas = null)
        {
            if (canvas == null)
                if (_canvas != null)
                    canvas = _canvas;
                else
                    canvas = GetComponentInParent<Canvas>();
        }

        internal void SetRectTransform(RectTransform _rectTransform = null)
        {
            if (rectTransform == null)
                if (_rectTransform != null)
                    rectTransform = _rectTransform;
                else
                    rectTransform = GetComponent<RectTransform>();
        }

        // Disable zoom if mouse pointer leaves window
        public void OnPointerExit(PointerEventData eventData)
        {
            isZoomable = false;
        }

        private void DragModel(Vector3 mouseDrag) {
            modelparent.position += (Vector3)(mouseDrag * SettingsHelper.GetSettings().DragSensitivity);
        }

        // Enable zoom if mouse pointer enters window
        public void OnPointerEnter(PointerEventData eventData)
        {
            isZoomable = true;
        }
    }
}