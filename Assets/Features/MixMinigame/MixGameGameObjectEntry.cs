using UnityEngine;

namespace Features.MixMinigame
{
    public class MixGameGameObjectEntry : MonoBehaviour
    {
        [SerializeField] private MixMinigameSequenceScriptableObject sequenceScriptableObject;

        public void Start()
        {
            if (sequenceScriptableObject == null)
            {
                Debug.LogError("SequenceScriptableObject is not assigned.");
                return;
            }

            var sequence = sequenceScriptableObject.GetSequence();
            
            if (sequence == null)
            {
                Debug.LogError("Failed to get sequence from SequenceScriptableObject.");
                return;
            }
        }
    }
}