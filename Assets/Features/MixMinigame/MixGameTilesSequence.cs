using System;
using System.Collections.Generic;
using System.Linq;
using Features.MixMinigame.Datas;
using UnityEngine;

namespace Features.MixMinigame
{
    public class MixGameTilesSequence
    {
        public MixGameTilesSequence(string rawSequenceText)
        {
            SequenceElements = MixGameParser.Parse(rawSequenceText);
        }

        public MixGameSequenceElementData[] SequenceElements { get; }
    }

    internal static class MixGameParser
    {
        // raw sequence text format:
        // visualNumber(int) appearTiming(float) initialRelativePositionX(float) initialRelativePositionY(float) ; optional - for movables - moveDuration(float) finalRelativePositionX(float) finalRelativePositionY(float) [movePathRelativePositionX1(float) movePathRelativePositionY1(float)...]
        // positions are relative to center, and relative in percentage, from -100 to 100 
        public static MixGameSequenceElementData[] Parse(string rawSequenceText)
        {
            var lines    = rawSequenceText.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
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
                if (!float.TryParse(parts[5], out var rotationZ)) continue;
                if (!int.TryParse(parts[6], out var tileType)) continue;

                elements.Add(
                    new MixGameMovableSequenceElementData(
                        visualNumber,
                        appearTiming,
                        new Vector2(initialRelativePositionX / 100, initialRelativePositionY / 100),
                        rotationZ,
                        moveDuration,
                        tileType
                    )
                );
            }

            return elements.ToArray();
        }
    }
}
