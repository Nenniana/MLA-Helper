using MLAHelper.Model.Builder;
using Unity.MLAgents;
using UnityEngine;

public class MLAHDecisionRequester : DecisionRequester
{
    [SerializeField]
    private ModelConstructor fullModelBuilder;

    protected override bool ShouldRequestDecision(DecisionRequestContext context)
    {
        bool requestDecision = context.AcademyStepCount % DecisionPeriod == 0;

        if (requestDecision)
            fullModelBuilder.RequestNewExecute();
            
        return requestDecision;
    }

    protected override bool ShouldRequestAction(DecisionRequestContext context)
    {
        return TakeActionsBetweenDecisions;
    }
}
