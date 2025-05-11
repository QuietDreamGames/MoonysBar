using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Features.MixMinigame.Views
{
    public class MixGameTileMovableView : MixGameTileView
    {
        private bool _isMoving;

        protected override void OnHit()
        {
            // if (!_isMoving)
            // {
            //     _isMoving = true;
            //     _         = PlayAnimationAndWaitAsync("Hit", 0);
            // }
            // else
            // {
            //     _ = PlayAnimationAndReturnToPoolAsync("HitReleased");
            // }
        }

        protected override void OnMiss()
        {
            // _ = PlayAnimationAndReturnToPoolAsync("Miss");
        }

        protected override void OnFail()
        {
            // _ = PlayAnimationAndReturnToPoolAsync("Fail");
        }

        protected override UniTask ResolveAnimation(string animationName, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
