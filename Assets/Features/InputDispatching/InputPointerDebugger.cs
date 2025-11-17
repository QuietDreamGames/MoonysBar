using Features.InputDispatching.Interfaces;
using UnityEngine;
using VContainer;

namespace Features.InputDispatching
{
    public class InputPointerDebugger
    {
        [Inject]
        public InputPointerDebugger(IInputEventBusFeed inputEventBusFeed)
        {
            inputEventBusFeed.OnPointerClick     += () => LogEvent("Pointer Clicked");
            inputEventBusFeed.OnPointerHoldStart += () => LogEvent("Pointer Hold Started");
            inputEventBusFeed.OnPointerHoldEnd   += () => LogEvent("Pointer Hold Ended");
            inputEventBusFeed.OnPointerDrag      += (Vector2 delta) => LogEvent("Pointer Drag Started");
            inputEventBusFeed.OnPointerDragEnd   += () => LogEvent("Pointer Drag Ended");
            inputEventBusFeed.OnPointerDrift     += (Vector2 delta) => LogEvent("Pointer Drifted");
        }

        private void LogEvent(string eventDescription)
        {
            Debug.Log($"[InputPointerDebugger] {eventDescription}");
        }
    }
}
