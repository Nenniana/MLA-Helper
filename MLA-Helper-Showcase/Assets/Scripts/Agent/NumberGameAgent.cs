using System;
using Unity.MLAgents.Actuators;
using UnityEngine;
using MLAHelper.HelperAgent;
using UnityEngine.UI;
using MLAHelper.Model.Builder;

public class NumberGameAgent : MLAHelperAgent
{
    [SerializeField]
    private GameController gameController;

    [SerializeField]
    private Button button;
    
    [SerializeField]
    private ModelConstructor modelConstructor;
    Action<float> RewardUpdated;

    protected override void OnEnable()
    {
        base.OnEnable();

        gameController.CorrectAnswer += OnCorrectAnswer;
        gameController.WrongAnswer += OnWrongAnswer;
        button.onClick.AddListener(Run);
    }
    protected override void OnDisable()
    {
        base.OnDisable();

        gameController.CorrectAnswer -= OnCorrectAnswer;
        gameController.WrongAnswer -= OnWrongAnswer;
        button.onClick.RemoveListener(Run);
    }

    private void OnWrongAnswer()
    {
        AddRewardValue(gameController.WrongAnswerReward);
        EndEpisode();
    }

    private void OnCorrectAnswer()
    {
        AddRewardValue(gameController.CorrectAnswerReward);
        EndEpisode();
    }

    public void AddRewardValue(float reward)
    {
        AddReward(reward);

        RewardUpdated?.Invoke(GetCumulativeReward());
    }

    public void SetRewardValue(float reward)
    {
        SetReward(reward);

        RewardUpdated?.Invoke(GetCumulativeReward());
    }

    public override void OnEpisodeBegin()
    {
        gameController.ResetGame();
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        gameController.ChooseAnswer(actions.DiscreteActions[0] != 0, actions.DiscreteActions[1] != 0);
    }

    private void Run() {
        modelConstructor.RequestNewExecute();
        RequestDecision();
        RequestAction();
    }
}
