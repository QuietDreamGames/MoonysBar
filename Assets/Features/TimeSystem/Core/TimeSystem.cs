using System.Collections.Generic;
using Features.TimeSystem.Interfaces;
using Features.TimeSystem.Interfaces.Handlers;
using UnityEngine;

namespace Features.TimeSystem.Core
{
    public class TimeSystem : ITimeSystem
    {
        private float _timeScale = 1f;
        private bool  _isPaused;
        private bool  _isInitialized;

        private readonly List<IUpdateHandler>      _updateHandlers      = new();
        private readonly List<IFixedUpdateHandler> _fixedUpdateHandlers = new();
        private readonly List<ILateUpdateHandler>  _lateUpdateHandlers  = new();

        
        public void SetUpdateProvider(IUpdateProvider updateProvider)
        {
            if (updateProvider == null)
            {
                Debug.LogError("Update provider for TimeSystem is null");
                return;
            }

            updateProvider.OnUpdate      += OnUpdate;
            updateProvider.OnFixedUpdate += OnFixedUpdate;
            updateProvider.OnLateUpdate  += OnLateUpdate;
        }
        
        public void Initialize()
        {
            _isInitialized = true;
        }

        public void Subscribe(ITimeCollector timeCollector)
        {
            _updateHandlers.AddRange(timeCollector.UpdateHandlers);
            _fixedUpdateHandlers.AddRange(timeCollector.FixedUpdateHandlers);
            _lateUpdateHandlers.AddRange(timeCollector.LateUpdateHandlers);
        }

        public void Unsubscribe(ITimeCollector timeCollector)
        {
            _updateHandlers.RemoveAll(timeCollector.UpdateHandlers.Contains);
            _fixedUpdateHandlers.RemoveAll(timeCollector.FixedUpdateHandlers.Contains);
            _lateUpdateHandlers.RemoveAll(timeCollector.LateUpdateHandlers.Contains);
        }

        public float GetTimeScale()
        {
            return _timeScale;
        }

        public void SetTimeScale(float timeScale)
        {
            _timeScale = timeScale;
        }

        public void Pause()
        {
            _isPaused = true;
        }

        public void Resume()
        {
            _isPaused = false;
        }

        #region Updates

        private void OnUpdate()
        {
            if (_isPaused || !_isInitialized) return;

            for (var i = 0; i < _updateHandlers.Count; i++)
            {
                _updateHandlers[i].OnUpdate(Time.deltaTime * _timeScale);
            }
        }

        private void OnFixedUpdate()
        {
            if (_isPaused || !_isInitialized) return;

            for (var i = 0; i < _fixedUpdateHandlers.Count; i++)
            {
                _fixedUpdateHandlers[i].OnFixedUpdate(Time.fixedDeltaTime * _timeScale);
            }
        }

        private void OnLateUpdate()
        {
            if (_isPaused || !_isInitialized) return;

            for (var i = 0; i < _lateUpdateHandlers.Count; i++)
            {
                _lateUpdateHandlers[i].OnLateUpdate(Time.deltaTime * _timeScale);
            }
        }

        #endregion
    }
}