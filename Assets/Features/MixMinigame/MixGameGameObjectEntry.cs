using System.Collections.Generic;
using Features.MixMinigame.Factories;
using Features.MixMinigame.Models;
using Features.MixMinigame.ViewModels;
using Features.MixMinigame.Views;
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
        
        [Inject] private MixGameTileFactory _tileFactory;
        
        private MixGameTilesSequence _sequence;
        
        private List<(MixGameTileView, MixGameTileViewModel)> _tileList;
        
        private int _currentIndex;
        
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
            
            _tileList = new List<(MixGameTileView, MixGameTileViewModel)>();
        }

        private void OnDestroy()
        {
            _timeSystem.Unsubscribe(_timeCollector);
            for (int i = 0; i < _tileList.Count; i++)
            {
                _tileList[i].Item1.ReturnToPool();
                _tileList[i].Item2.Dispose();
            }
            
            _tileList.Clear();
        }

        public void OnUpdate(float deltaTime)
        {
            _timer += deltaTime;

            for (int i = 0; i < _tileList.Count; i++)
            {
                var tileView = _tileList[i].Item1;
                tileView.OnUpdate(deltaTime);
            }

            if (_sequence.SequenceElements[_currentIndex].AppearTiming < _timer) return;
            if (_currentIndex >= _sequence.SequenceElements.Length) return;
            
            var sequenceElement = _sequence.SequenceElements[_currentIndex];
            var tile = _tileFactory.GetTile(sequenceElement, transform);
            _tileList.Add(tile);
            
            _currentIndex++;
        }
    }
}