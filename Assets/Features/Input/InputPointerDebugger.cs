using System;
using Features.Input.Interfaces;
using Features.Parameters;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Features.Input
{
    public class InputPointerDebugger : IStartable, IDisposable
    {
        [Inject] private IInputEventBusFeed          _inputEventBusFeed;
        [Inject] private DefaultRootParametersHolder _rootParametersHolder;

        public void Dispose()
        {
            if (!_rootParametersHolder.DebugParameters.IsInputDebugMode)
                return;

            _inputEventBusFeed.OnPointerClick     -= PointerClickFired;
            _inputEventBusFeed.OnPointerHoldStart -= PointerHoldStartFired;
            _inputEventBusFeed.OnPointerHoldEnd   -= PointerHoldEndFired;
            _inputEventBusFeed.OnPointerDrag      -= PointerDragFired;
            _inputEventBusFeed.OnPointerDragEnd   -= PointerDragEndFired;
            _inputEventBusFeed.OnPointerDrift     -= PointerDriftFired;
        }

        public void Start()
        {
            if (!_rootParametersHolder.DebugParameters.IsInputDebugMode)
                return;

            _inputEventBusFeed.OnPointerClick     += PointerClickFired;
            _inputEventBusFeed.OnPointerHoldStart += PointerHoldStartFired;
            _inputEventBusFeed.OnPointerHoldEnd   += PointerHoldEndFired;
            _inputEventBusFeed.OnPointerDrag      += PointerDragFired;
            _inputEventBusFeed.OnPointerDragEnd   += PointerDragEndFired;
            _inputEventBusFeed.OnPointerDrift     += PointerDriftFired;
        }

        private void PointerClickFired()
        {
            Debug.Log("Pointer Click Fired");
        }

        private void PointerHoldStartFired()
        {
            Debug.Log("Pointer Hold Start Fired");
        }

        private void PointerHoldEndFired()
        {
            Debug.Log("Pointer Hold End Fired");
        }

        private void PointerDriftFired(Vector2 delta)
        {
            Debug.Log($"Pointer Drift Fired with delta: {delta}");
        }

        private void PointerDragFired(Vector2 delta)
        {
            Debug.Log($"Pointer Drag Fired with delta: {delta}");
        }

        private void PointerDragEndFired()
        {
            Debug.Log("Pointer Drag End Fired");
        }
    }
}
