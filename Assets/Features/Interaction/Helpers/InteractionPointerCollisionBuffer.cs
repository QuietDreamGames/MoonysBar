using System;
using Features.Collision;

namespace Features.Interaction.Helpers
{
    public class InteractionPointerCollisionBuffer
    {
        public readonly PointerCollider[] CollidersBuffer = new PointerCollider[10];

        // Internal buffers owned by the class/struct
        private readonly int[] _addedIndicesBuffer   = new int[10];
        private readonly int[] _removedIndicesBuffer = new int[10];

        // This array is necessary for the O(N*M) check.
        private bool[] _matchedOldBuffer = Array.Empty<bool>(); // Initialize small

        public ColliderUpdates TryDetectUpdatedColliders(
            ReadOnlySpan<PointerCollider> newBuffer,
            ReadOnlySpan<PointerCollider> oldBuffer
        )
        {
            var newCount = newBuffer.Length;
            var oldCount = oldBuffer.Length;

            // --- Allocation for Tracking ---
            // Ensure the tracking buffer is large enough, or reallocate if needed.
            if (_matchedOldBuffer.Length < oldCount) _matchedOldBuffer = new bool[oldCount];

            // Reset the necessary part of the buffer (much faster than allocating new)
            var matchedOld = new Span<bool>(array: _matchedOldBuffer, start: 0, length: oldCount);
            matchedOld.Clear(); // Efficiently resets all to false

            // --- Detection ---
            var addedCount   = 0;
            var removedCount = 0;

            // 1. Single Pass: Detect additions and mark matches in oldBuffer
            for (var i = 0; i < newCount; i++)
            {
                var isFound = false;
                for (var j = 0; j < oldCount; j++)
                    if (newBuffer[i].Equals(oldBuffer[j]))
                    {
                        isFound       = true;
                        matchedOld[j] = true; // Mark the old element as matched
                        break;
                    }

                if (isFound) continue;

                // Found an addition
                if (addedCount >= _addedIndicesBuffer.Length)
                    // The buffer overflowed. You must decide if you allocate or stop.
                    // For zero-allocation, we return an empty result if capacity is exceeded.
                    return new ColliderUpdates();

                _addedIndicesBuffer[addedCount] = i;
                addedCount++;
            }

            // 2. Detect removals: find old elements that were NOT matched
            for (var i = 0; i < oldCount; i++)
            {
                if (matchedOld[i]) continue; // Skip if matched

                if (removedCount >= _removedIndicesBuffer.Length)
                    // Overflow (same strategy as above)
                    return new ColliderUpdates();

                _removedIndicesBuffer[removedCount] = i;
                removedCount++;
            }

            // 3. Prepare zero-allocation output
            return new ColliderUpdates
            {
                AddedIndices   = _addedIndicesBuffer.AsSpan()[..addedCount],
                RemovedIndices = _removedIndicesBuffer.AsSpan()[..removedCount]
            };
        }


        // public bool TryDetectUpdatedColliders(
        //     PointerCollider[] newBuffer,
        //     int               newCount,
        //     PointerCollider[] oldBuffer,
        //     int               oldCount,
        //     out int[]         addedIndices,
        //     out int[]         removedIndices
        // )
        // {
        //     var addedCount   = 0;
        //     var removedCount = 0;
        //
        //     for (var i = 0; i < newCount; i++)
        //     {
        //         var isFound = false;
        //
        //         for (var j = 0; j < oldCount; j++)
        //         {
        //             if (newBuffer[i] != oldBuffer[j]) continue;
        //
        //             isFound = true;
        //             // Mark this old index as matched so we don't re-check it when detecting removals
        //             if (j < oldCount) // defensive, though j is guaranteed < oldCount
        //                 ;             // no-op to keep structure clear
        //             break;
        //         }
        //
        //         if (isFound) continue;
        //
        //         _addedIndicesBuffer[addedCount] = i;
        //         addedCount++;
        //     }
        //
        //     // Optimize removals by marking which old indices were matched in the first pass.
        //     // Re-run the first pass to build the matched mask (so we avoid double comparisons).
        //     var matchedOld = new bool[oldCount];
        //     for (var i = 0; i < newCount; i++)
        //     for (var j = 0; j < oldCount; j++)
        //     {
        //         if (newBuffer[i] != oldBuffer[j]) continue;
        //         matchedOld[j] = true;
        //         break;
        //     }
        //
        //     for (var i = 0; i < oldCount; i++)
        //     {
        //         if (matchedOld[i]) continue;
        //
        //         _removedIndicesBuffer[removedCount] = i;
        //         removedCount++;
        //     }
        //
        //     // Prepare output arrays trimmed to actual counts
        //     if (addedCount == 0)
        //     {
        //         addedIndices = Array.Empty<int>();
        //     }
        //     else
        //     {
        //         addedIndices = new int[addedCount];
        //         Array.Copy(sourceArray: _addedIndicesBuffer, sourceIndex: 0, destinationArray: addedIndices,
        //             destinationIndex: 0, length: addedCount);
        //     }
        //
        //     if (removedCount == 0)
        //     {
        //         removedIndices = Array.Empty<int>();
        //     }
        //     else
        //     {
        //         removedIndices = new int[removedCount];
        //         Array.Copy(sourceArray: _removedIndicesBuffer, sourceIndex: 0, destinationArray: removedIndices,
        //             destinationIndex: 0, length: removedCount);
        //     }
        //
        //     return addedCount > 0 || removedCount > 0;
        // }

        public bool CompareColliders(PointerCollider[] otherBuffer, int count)
        {
            for (var i = 0; i < count; i++)
                if (CollidersBuffer[i] != otherBuffer[i])
                    return false;

            return true;
        }

        public bool CompareFirstCollider(PointerCollider otherCollider)
        {
            return CollidersBuffer[0] == otherCollider;
        }

        public bool FindFirstCollider(ReadOnlySpan<PointerCollider> otherCollider)
        {
            for (var i = 0; i < otherCollider.Length; i++)
                if (CollidersBuffer[0] == otherCollider[i])
                    return true;
            return false;
        }

        public void UpdateBuffer(ReadOnlySpan<PointerCollider> sourceBuffer)
        {
            for (var i = 0; i < CollidersBuffer.Length; i++)
                if (i >= sourceBuffer.Length)
                    CollidersBuffer[i] = null;
                else
                    CollidersBuffer[i] = sourceBuffer[i];
        }

        public void Clear()
        {
            for (var i = 0; i < CollidersBuffer.Length; i++)
                CollidersBuffer[i] = null;
        }

        public bool IsEmpty(int count)
        {
            for (var i = 0; i < count; i++)
                if (CollidersBuffer[i] != null)
                    return false;

            return true;
        }

        public int FindLength()
        {
            var length = 0;
            for (var i = 0; i < CollidersBuffer.Length; i++)
                if (CollidersBuffer[i] != null)
                    length++;

            return length;
        }
    }
}
