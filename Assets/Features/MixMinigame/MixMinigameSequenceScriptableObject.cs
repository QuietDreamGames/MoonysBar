using UnityEngine;

namespace Features.MixMinigame
{
    [CreateAssetMenu(fileName = "MixMinigameSequence", menuName = "MixMinigame/MixMinigameSequence", order = 0)]
    public class MixMinigameSequenceScriptableObject : ScriptableObject
    {
        [SerializeField] private TextAsset sequenceFile;
        [SerializeField] private string    soundtrackName;

        public string SoundtrackName => soundtrackName;

        public MixGameTilesSequence GetSequence()
        {
            if (sequenceFile == null)
            {
                Debug.LogError("Sequence file is not set.");
                return null;
            }

            return new MixGameTilesSequence(sequenceFile.text);
        }
    }
}