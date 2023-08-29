using System;
using MLAHelper.ScriptableReferenceSystem.Collection;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

// Builds upon ML-Agents agent, allowing for action masks and observations from collections to input to agent
namespace MLAHelper.HelperAgent {
    [AddComponentMenu("MLA-Helper/Agent-MLA-Helper", 1)]
    // [RequireComponent(typeof(AgentRequest))]
    public class MLAHelperAgent : Agent {

        [SerializeField]
        private ObservationCollection observationCollection;

        [SerializeField]
        private ActionMaskCollection actionMaskCollection;

        public ActionMaskCollection ActionMaskCollection { get => actionMaskCollection; private set => actionMaskCollection = value; }
        public ObservationCollection ObservationCollection { get => observationCollection; private set => observationCollection = value; }

        public override void CollectObservations(VectorSensor sensor)
        {
            base.CollectObservations(sensor);
            foreach (var observation in ObservationCollection.ObservationReferences) {
                sensor.AddObservation(observation.GetValue);
            }
        }

        public override void WriteDiscreteActionMask(IDiscreteActionMask actionMask)
        {
            base.WriteDiscreteActionMask(actionMask);
            foreach (var actionMaskReference in ActionMaskCollection.actionMasks){
                actionMask.SetActionEnabled(actionMaskReference.Branch, actionMaskReference.Index, actionMaskReference.Value);
            }
        }

        internal void Setup(ObservationCollection observationCollection, ActionMaskCollection actionMaskCollection)
        {
            this.observationCollection = observationCollection;
            this.actionMaskCollection = actionMaskCollection;
        }
    } 
}