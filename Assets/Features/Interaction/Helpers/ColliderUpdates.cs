using System;

namespace Features.Interaction.Helpers
{
    public ref struct ColliderUpdates
    {
        // These Spans are windows into the calling class's internal buffers
        public ReadOnlySpan<int> AddedIndices   { get; internal set; }
        public ReadOnlySpan<int> RemovedIndices { get; internal set; }

        public bool HasUpdates => !AddedIndices.IsEmpty || !RemovedIndices.IsEmpty;
    }
}
