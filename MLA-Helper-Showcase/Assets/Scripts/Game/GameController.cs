using System;
using System.Collections;
using System.Collections.Generic;
using MLAHelper.ScriptableReferenceSystem;
using MLAHelper.ScriptableReferenceSystem.Reference;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Action<bool,bool> AnswerGiven;
    public Action GameStarted, CorrectAnswer, WrongAnswer;

    [SerializeField]
    private int correctAnswerReward = 50;

    [SerializeField]
    private int wrongAnswerReward = -50;

    [SerializeField]
    private int seed = 0;

    [SerializeField]
    private bool useSeed = false;

    [SerializeField]
    private int minInput = 0;

    [SerializeField]
    private int maxInput = 10;

    [SerializeField]
    private FloatGOReference input1;
    [SerializeField]
    private FloatGOReference input2;

    private float fullInput;
    private float halfOutput;
    private System.Random randomInstance;
    public float FullInput { get => fullInput; private set => fullInput = value; }
    public float HalfOutput { get => halfOutput; private set => halfOutput = value; }
    public int CorrectAnswerReward { get => correctAnswerReward; private set => correctAnswerReward = value; }
    public int WrongAnswerReward { get => wrongAnswerReward; private set => wrongAnswerReward = value; }

    private void Awake() {
        if (useSeed)
            randomInstance = new System.Random(seed);
        else
            randomInstance = new System.Random();

        ComputeHalfOutput();
    }

    [Button]
    public void ResetGame() {
        SetRandomValues();
        GameStarted?.Invoke();
    }

    private void ComputeHalfOutput()
    {
        HalfOutput = Math.Abs(minInput - maxInput);
    }

    private void SetRandomValues() {
        float rnd1 = randomInstance.Next(minInput, maxInput);
        float rnd2 = randomInstance.Next(minInput, maxInput);
        input1.Value = rnd1;
        input2.Value = rnd2;

        FullInput = input1.GetValue[0] + input2.GetValue[0];
    }

    public bool ChooseAnswer(bool overHalf, bool underHalf) {
        AnswerGiven?.Invoke(overHalf, underHalf);

        if (FullInput > HalfOutput && overHalf && !underHalf) {
            CorrectAnswer?.Invoke();
            return true;
        }
        else if (FullInput < HalfOutput && !overHalf && underHalf) {
            CorrectAnswer?.Invoke();
            return true;
        }
        else if (FullInput == HalfOutput && !overHalf && !underHalf) {
            CorrectAnswer?.Invoke();
            return true;
        }

        WrongAnswer?.Invoke();
        return false;
    }
}
