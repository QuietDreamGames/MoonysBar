using System;
using System.Collections.Generic;
using System.Globalization;
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
            rawSequenceText = rawSequenceText.Replace("\r", string.Empty);
            var lines       = rawSequenceText.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var elements    = new List<MixGameSequenceElementData>();
            var cultureInfo = new CultureInfo("en-US");

            foreach (var line in lines)
            {
                var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length < 4) continue;
                if (parts.First().First() == '#') continue; // Skip comments

                if (!int.TryParse(parts[0], out var visualNumber)) continue;
                if (!float.TryParse(parts[1], NumberStyles.Number, cultureInfo, out var appearTiming)) continue;
                if (!float.TryParse(parts[2], NumberStyles.Number, cultureInfo, out var initialRelativePositionX))
                    continue;
                if (!float.TryParse(parts[3], NumberStyles.Number, cultureInfo, out var initialRelativePositionY))
                    continue;

                if (parts.Length == 4) // Clickable
                {
                    var initialPosition = new Vector2(initialRelativePositionX / 100, initialRelativePositionY / 100);
                    elements.Add(new MixGameClickableSequenceElementData(visualNumber, appearTiming, initialPosition));
                    continue;
                }

                if (!float.TryParse(parts[4], NumberStyles.Number, cultureInfo, out var moveDuration)) continue;
                if (!float.TryParse(parts[5], NumberStyles.Number, cultureInfo, out var rotationZ)) continue;
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
