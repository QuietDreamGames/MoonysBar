using Features.MixMinigame.Factories;
using Features.TimeSystem.Interfaces;
using Features.TimeSystem.Interfaces.Handlers;
using Features.TimeSystem.Interfaces.Injected;
using UnityEngine;
using VContainer;

namespace Features.MixMinigame
{
    public class MixGameGameObjectEntry : MonoBehaviour, IUpdateHandler
    {
        [SerializeField] private          MixMinigameSequenceScriptableObject sequenceScriptableObject;
        [Inject]         private readonly MixGameLevelTimerHolder             _levelTimerHolder;

        [Inject] private readonly MixGameTileFactory           _tileFactory;
        [Inject] private readonly MixGameTilesHolderAndUpdater _tilesHolderAndUpdater;
        [Inject] private readonly ITransientTimeCollector      _timeCollector;

        [Inject] private readonly ITimeSystem _timeSystem;

        private int _currentIndex;

        private MixGameTilesSequence _sequence;

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

            _sequence = sequenceScriptableObject.GetSequence();

            if (_sequence == null)
            {
                Debug.LogError("Failed to get sequence from SequenceScriptableObject.");
                return;
            }

            if (_sequence.SequenceElements.Length == 0)
            {
                Debug.LogError("Sequence is empty.");
                return;
            }


            _currentIndex = 0;
        }

        private void OnDestroy()
        {
            _timeSystem.Unsubscribe(_timeCollector);
            _tilesHolderAndUpdater.Dispose();
        }

        public void OnUpdate(float deltaTime)
        {
            _timer += deltaTime;

            _tilesHolderAndUpdater.OnUpdate(deltaTime);
            _levelTimerHolder.OnUpdate(deltaTime);


            if (_currentIndex >= _sequence.SequenceElements.Length) return;
            if (_sequence.SequenceElements[_currentIndex].AppearTiming > _timer) return;

            var sequenceElement = _sequence.SequenceElements[_currentIndex];
            var tile            = _tileFactory.GetTile(sequenceElement, transform);
            _tilesHolderAndUpdater.AddTile(tile.Item1, tile.Item2, tile.Item3);


            _currentIndex++;
        }
    }
}
