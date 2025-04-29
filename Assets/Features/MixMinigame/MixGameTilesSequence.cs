using System;
using System.Collections.Generic;
using System.Linq;
using Features.MixMinigame.Datas;
using UnityEngine;

namespace Features.MixMinigame
{
    public class MixGameTilesSequence
    {
        private readonly MixGameSequenceElementData[] _sequenceElements;
        public MixGameSequenceElementData[] SequenceElements => _sequenceElements;
        
        public MixGameTilesSequence(string rawSequenceText)
        {
            _sequenceElements = MixGameParser.Parse(rawSequenceText);
        }
    }

    internal static class MixGameParser
    {
        // raw sequence text format:
        // visualNumber(int) appearTiming(float) initialRelativePositionX(float) initialRelativePositionY(float) ; optional - for movables - moveDuration(float) finalRelativePositionX(float) finalRelativePositionY(float) [movePathRelativePositionX1(float) movePathRelativePositionY1(float)...]
        // positions are relative to center, and relative in percentage, from -100 to 100 
        public static MixGameSequenceElementData[] Parse(string rawSequenceText)
        {
            var lines = rawSequenceText.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var elements = new List<MixGameSequenceElementData>();

            foreach (var line in lines)
            {
                var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length < 4) continue;
                if (parts.First().First() == '#') continue; // Skip comments

                if (!int.TryParse(parts[0], out var visualNumber)) continue;
                if (!float.TryParse(parts[1], out var appearTiming)) continue;
                if (!float.TryParse(parts[2], out var initialRelativePositionX)) continue;
                if (!float.TryParse(parts[3], out var initialRelativePositionY)) continue;

                if (parts.Length == 4) // Clickable
                {
                    var initialPosition = new Vector2(initialRelativePositionX / 100, initialRelativePositionY / 100);
                    elements.Add(new MixGameClickableSequenceElementData(visualNumber, appearTiming, initialPosition));
                    continue;
                }

                if (!float.TryParse(parts[4], out var moveDuration)) continue;
                if (!float.TryParse(parts[5], out var finalRelativePositionX)) continue;
                if (!float.TryParse(parts[6], out var finalRelativePositionY)) continue;

                var finalPosition = new Vector2(finalRelativePositionX / 100, finalRelativePositionY / 100);
                var movePath = new Vector2[(parts.Length - 7) / 2];
                
                for (var i = 0; i < movePath.Length; i++)
                {
                    if (!float.TryParse(parts[7 + i * 2], out var pathX)) continue;
                    if (!float.TryParse(parts[8 + i * 2], out var pathY)) continue;
                    movePath[i] = new Vector2(pathX / 100, pathY / 100);
                }

                elements.Add(
                    new MixGameMovableSequenceElementData(
                        visualNumber,
                        appearTiming,
                        new Vector2(initialRelativePositionX / 100, initialRelativePositionY / 100),
                        finalPosition,
                        moveDuration,
                        movePath
                    )
                );
            }

            return elements.ToArray();
        }
    }
}