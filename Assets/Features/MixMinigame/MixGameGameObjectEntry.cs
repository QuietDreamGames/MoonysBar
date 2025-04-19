using Features.TimeSystem.Interfaces;
using Features.TimeSystem.Interfaces.Handlers;
using Features.TimeSystem.Interfaces.Injected;
using UnityEngine;
using VContainer;

namespace Features.MixMinigame
{
    public class MixGameGameObjectEntry : MonoBehaviour, IUpdateHandler
    {
        [SerializeField] private MixMinigameSequenceScriptableObject sequenceScriptableObject;
        
        [Inject] private ITimeSystem             _timeSystem;
        [Inject] private ITransientTimeCollector _timeCollector;

        private float _timer;
        
        public void Start()
        {
            _timer = 0;
            _timeCollector.UpdateHandlers.Add(this);
            _timeSystem.Subscribe(_timeCollector);
            
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

        private void OnDestroy()
        {
            _timeSystem.Unsubscribe(_timeCollector);
        }

        public void OnUpdate(float deltaTime)
        {
            _timer += deltaTime;
        }
    }
}