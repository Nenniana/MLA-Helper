using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using MLAHelper.ScriptableReferenceSystem.SO;
using MLAHelper.ScriptableReferenceSystem.Reference;

// Collection of observations used as input for model visualization and ML-Agents
namespace MLAHelper.ScriptableReferenceSystem.Collection {
    [InlineEditor]
    [CreateAssetMenu(fileName = "ObservationCollection", menuName = "Scriptable Observation/Observation Collection", order = 1)]
    public class ObservationCollection : ScriptableObjectParent
    {
        // TODO: Write warning box about model visualization values being of previous world state.

        // Keeps track of overall size of ObservationReferences (includes each value in lists, vector3s and so forth)
        private int currentSize = 0;
        
        // Array of Observation references
        [SerializeReference]
        [InlineProperty]
        [ListDrawerSettings(ShowIndexLabels = false, ShowPaging = false, Expanded = true)]
        [HideReferenceObjectPicker]
        [OnCollectionChanged(After = "UpdateCurrentObservationSize")]
        public GOReferenceParent[] ObservationReferences = new GOReferenceParent[0];

        // Public get, private set for overall size
        public int CurrentSize { get => currentSize; private set => currentSize = value; }

        // Calculates overall observation size
        public int GetFloatArrayLength() {
            int length = 0;
            foreach (GOReferenceParent reference in ObservationReferences) {
                length += reference.GetObservationSize();
            }

            return length;
        }

        // Get value and out name for individual observation - as an example a Vector3 would be three individual observations
        public float GetObservationPerIndex(int index, out string name) {
            int counter = 0;
            foreach (GOReferenceParent referenceGroup in ObservationReferences) {
                int length = referenceGroup.GetValue.Length;
                if ((counter + length) > index) {
                    int placement = index - counter;
                    name = referenceGroup.Variable.name + "_" + placement;
                    return referenceGroup.GetValue[placement];
                }

                counter += length;
            }

            name = "";
            return 0;
        }

        // Called on Inspector Initiation and subscribes to all ListGoReferences in observation array, to get their correct size at all times
        [OnInspectorInit("SubscribeToLists")]
        private void SubscribeToLists() {
            int counter = 0;
            foreach (GOReferenceParent referenceGroup in ObservationReferences) {
                if (referenceGroup is ListGOReference) {
                    SubscribeToObservation(referenceGroup as ListGOReference);
                    counter++;
                }
            }

            currentSize = GetFloatArrayLength() + counter;
        }

        // Called after changes to observation array, ensuring correct size of all individual observations
        private void UpdateCurrentObservationSize(CollectionChangeInfo info, object value) {
            if (info.ChangeType == CollectionChangeType.Add) {
                UpdateSize(0, (info.Value as GOReferenceParent).GetObservationSize());
                if (info.Value is ListGOReference) 
                    SubscribeToObservation(info.Value as ListGOReference);   
            } else if (info.ChangeType == CollectionChangeType.RemoveValue || info.ChangeType == CollectionChangeType.RemoveIndex || info.ChangeType == CollectionChangeType.RemoveKey) {
                UpdateOverallSizeOnChange();
            }
        }

        private void UpdateOverallSizeOnChange()
        {
            currentSize = GetFloatArrayLength();
        }

        // Subscribe to an individual ListGoReference
        private void SubscribeToObservation(ListGOReference reference)
        {
            UpdateSize(0, reference.GetObservationSize());
            reference.SizeChangedFromTo += UpdateSize;
        }

        // Called when ListGoReference list size changes
        private void UpdateSize(int oldSize, int newSize) {
            currentSize += (-oldSize + newSize);
        }
    }
}