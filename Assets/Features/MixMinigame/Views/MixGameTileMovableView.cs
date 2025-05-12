using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Features.MixMinigame.Views
{
    public class MixGameTileMovableView : MixGameTileView
    {
        [SerializeField] private Transform viewPivot;

        [SerializeField] private SpriteRenderer handleSpriteRenderer;
        [SerializeField] private SpriteRenderer timingCircleSpriteRenderer;
        [SerializeField] private SpriteRenderer timingCircleHitSpriteRenderer;

        private bool _isMoving;

        protected override void OnHit()
        {
            if (!_isMoving)
            {
                _isMoving = true;
                _ = PlayAnimationAndWaitAsync("Hit", 0);
            }
            else
            {
                _ = PlayAnimationAndReturnToPoolAsync("HitReleased", 0);
            }
        }

        protected override void OnMiss()
        {
            _ = PlayAnimationAndReturnToPoolAsync("Miss", 0);
        }

        protected override void OnFail()
        {
            _ = PlayAnimationAndReturnToPoolAsync("Fail", 0);
        }

        protected override UniTask ResolveAnimation(string animationName, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
