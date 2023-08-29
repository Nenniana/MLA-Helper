using MLAHelper.Model.Builder;
using Sirenix.OdinInspector;
using Unity.MLAgents;
using UnityEngine;

public class AgentRequest : MonoBehaviour {

    /* [SerializeField]
    private bool runNormalAgent; */

    private Agent agent;
    
    [SerializeField]
    private ModelConstructor fullModelBuilder;

    internal void Awake()
    {
        agent = gameObject.GetComponent<Agent>();
        Debug.Assert(agent != null, "Agent component was not found on this gameObject and is required.");
    }

    [Button("Request Agent Decision and Action")]
    public void RequestNewDecision() {
        fullModelBuilder.RequestNewExecute();
        agent?.RequestDecision();
        agent?.RequestAction();
    }
}