using UnityEngine;
using UnityEngine.UI;

namespace MLAHelper.Model.Visuals {
    public class PauseTimeButton : MonoBehaviour
    {
        [SerializeField]
        private Button button;
        private bool timeStopped = false;
        private void OnEnable () {
            button.onClick.AddListener(ToggleTime);
        }

        private void OnDisable () {
            button.onClick.RemoveListener(ToggleTime);
        }

        private void ToggleTime () {
            timeStopped = !timeStopped;
            if (timeStopped)
                Time.timeScale = 0;
            else
                Time.timeScale = 1;

            Debug.Log("Time scale is " + Time.timeScale);
        }
    }
}