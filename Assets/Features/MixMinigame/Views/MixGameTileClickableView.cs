using UnityEngine;

namespace Features.MixMinigame.Views
{
    public class MixGameTileClickableView : MixGameTileView
    {
        public override void AnimateSuccessfulHit()
        {
            Debug.LogError("not implemented");
        }

        public override void AnimateMissedHit()
        {
            Debug.LogError("not implemented");
        }

        public override void AnimateTimeRunOut()
        {
            Debug.LogError("not implemented");
        }
    }
}