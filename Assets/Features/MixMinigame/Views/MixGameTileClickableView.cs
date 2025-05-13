using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Features.MixMinigame.ViewModels;
using UnityEngine;

namespace Features.MixMinigame.Views
{
    public class MixGameTileClickableView : MixGameTileView
    {
        [SerializeField] private SpriteRenderer staticViewSpriteRenderer;
        [SerializeField] private SpriteRenderer dynamicViewSpriteRenderer;

        private float _hitTiming;

        private Vector3 _initialBaseScale;
        private Vector3 _initialDynamicScale;

        public override void Initialize(MixGameTileViewModel tileViewModel)
        {
            base.Initialize(tileViewModel);

            _initialBaseScale    = transform.localScale;
            _initialDynamicScale = dynamicViewSpriteRenderer.transform.localScale;
            _hitTiming           = tileViewModel.TileModel.HitTiming;

            var initDynColor = dynamicViewSpriteRenderer.color;

            dynamicViewSpriteRenderer.color = new Color(
                initDynColor.r, initDynColor.g, initDynColor.b,
                1);

            _ = PlayAnimationAndWaitAsync("Shrink", 1);
        }

        protected override void OnHit()
        {
            _ = PlayAnimationAndReturnToPoolAsync("Hit", 0);
            _ = PlayAnimationAndWaitAsync("ShrinkCircleFade", 1);
        }

        protected override void OnMiss()
        {
            _ = PlayAnimationAndReturnToPoolAsync("Miss", 0);
            _ = PlayAnimationAndWaitAsync("ShrinkCircleFade", 1);
        }

        protected override void OnFail()
        {
            _ = PlayAnimationAndReturnToPoolAsync("Fail", 0);
            _ = PlayAnimationAndWaitAsync("ShrinkCircleFade", 1);
        }

        protected override UniTask ResolveAnimation(string animationName, CancellationToken ct)
        {
            var tween = animationName switch
            {
                "Hit"              => HitTween(),
                "Miss"             => MissTween(),
                "Fail"             => FailTween(),
                "Shrink"           => ShrinkTween(),
                "ShrinkCircleFade" => ShrinkCircleFade(),
                _                  => throw new ArgumentOutOfRangeException(nameof(animationName), animationName, null)
            };

            tween.SetUpdate(UpdateType.Manual);
            Tweens.Add(tween);
            tween.OnKill(() =>
            {
                if (Tweens.Contains(tween)) Tweens.Remove(tween);
            });

            return tween.WithCancellation(ct);
        }

        public override void ReturnToPool()
        {
            base.ReturnToPool();
            transform.localScale                           = _initialBaseScale;
            dynamicViewSpriteRenderer.transform.localScale = _initialDynamicScale;
        }

        private Tween HitTween()
        {
            var tween = transform.DOScale(_initialBaseScale * 1.2f, 0.5f)
                .From(_initialBaseScale);
            return tween;
        }

        private Tween MissTween()
        {
            var tween = transform.DOScale(_initialBaseScale * 0.8f, 0.5f)
                .From(_initialBaseScale);
            return tween;
        }

        private Tween FailTween()
        {
            var tween = transform.DOScale(_initialBaseScale * 0.5f, 0.5f)
                .From(_initialBaseScale);
            return tween;
        }

        private Tween ShrinkTween()
        {
            var tween = dynamicViewSpriteRenderer
                .transform.DOScale(_initialDynamicScale, _hitTiming)
                .From(_initialDynamicScale * 3);
            return tween;
        }

        private Tween ShrinkCircleFade()
        {
            var initColor = staticViewSpriteRenderer.color;
            var tween = dynamicViewSpriteRenderer.DOColor(
                new Color(initColor.r, initColor.g, initColor.b, 0),
                _hitTiming);
            return tween;
        }
    }
}
