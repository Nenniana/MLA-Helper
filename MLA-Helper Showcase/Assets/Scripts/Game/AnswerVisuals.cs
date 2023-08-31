using UnityEngine;
using TMPro;

public class AnswerVisuals : MonoBehaviour
{
    [SerializeField]
    private GameController gameController;
    private TextMeshProUGUI textElement;

    private void Awake() {
        TryGetComponent<TextMeshProUGUI>(out textElement);
    }

    private void OnEnable() {
        gameController.AnswerGiven += OnAnswerGiven;
        gameController.GameStarted += OnGameStarted;
    }

    private void OnDisable() {
        gameController.AnswerGiven -= OnAnswerGiven;
        gameController.GameStarted -= OnGameStarted;
    }

    private void OnGameStarted()
    {
        // textElement.text = $"Is {gameController.FullInput} bigger/smaller than {gameController.HalfOutput}?";
    }

    private void OnAnswerGiven(bool arg1, bool arg2)
    {
        textElement.text = $"Is <b>{gameController.FullInput}</b> bigger or smaller than <b>{gameController.HalfOutput}</b>?\nIs it bigger than: <b>'{arg1}'</b> and is it smaller than: <b>'{arg2}'</b>.";
        // Debug.Log($"Is {gameController.FullInput} bigger/smaller than {gameController.HalfOutput}? Bigger than: <b>{arg1}</b>, smaller than: <b>{arg2}</b>.");
        // textElement.text = $"Is {gameController.FullInput} bigger/smaller than {gameController.HalfOutput}? Bigger than: <b>{arg1}</b>, smaller than: <b>{arg2}</b>.";
    }
}
