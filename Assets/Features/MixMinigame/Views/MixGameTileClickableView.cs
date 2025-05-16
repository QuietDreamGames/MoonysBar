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

        public override void Initialize(MixGameTileViewModel tileViewModel)
        {
            base.Initialize(tileViewModel);
            _hitTiming = tileViewModel.TileModel.HitTiming;

            dynamicViewSpriteRenderer.transform.localScale = Vector3.one;

            var textInitColor = textMeshVisualNumber.color;
            textMeshVisualNumber.color      = new Color(textInitColor.r, textInitColor.g, textInitColor.b, 1);
            staticViewSpriteRenderer.color  = Color.white;
            dynamicViewSpriteRenderer.color = Color.white;


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

        private Tween HitTween()
        {
            var staticColorTween = staticViewSpriteRenderer
                .DOColor(new Color(1, 1, 1, 0), 0.5f);
            var dynColorTween = dynamicViewSpriteRenderer
                .DOColor(new Color(1, 1, 1, 0), 0.5f);

            var textInitColor = textMeshVisualNumber.color;
            var textColorTween = textMeshVisualNumber
                .DOColor(new Color(textInitColor.r, textInitColor.g, textInitColor.b, 0), 0.25f);

            return DOTween.Sequence()
                .Append(staticColorTween)
                .Join(dynColorTween)
                .Join(textColorTween);
        }

        private Tween MissTween()
        {
            var staticColorTween = staticViewSpriteRenderer
                .DOColor(new Color(1, 1, 1, 0), 0.5f);
            var dynColorTween = dynamicViewSpriteRenderer
                .DOColor(new Color(1, 1, 1, 0), 0.5f);

            var textInitColor = textMeshVisualNumber.color;
            var textColorTween = textMeshVisualNumber
                .DOColor(new Color(textInitColor.r, textInitColor.g, textInitColor.b, 0), 0.25f);

            return DOTween.Sequence()
                .Append(staticColorTween)
                .Join(dynColorTween)
                .Join(textColorTween);
        }

        private Tween FailTween()
        {
            var staticColorTween = staticViewSpriteRenderer
                .DOColor(new Color(1, 1, 1, 0), 0.5f);
            var dynColorTween = dynamicViewSpriteRenderer
                .DOColor(new Color(1, 1, 1, 0), 0.5f);

            var textInitColor = textMeshVisualNumber.color;
            var textColorTween = textMeshVisualNumber
                .DOColor(new Color(textInitColor.r, textInitColor.g, textInitColor.b, 0), 0.25f);

            return DOTween.Sequence()
                .Append(staticColorTween)
                .Join(dynColorTween)
                .Join(textColorTween);
        }

        private Tween ShrinkTween()
        {
            var shrinkingTween = dynamicViewSpriteRenderer
                .transform.DOScale(Vector3.one, _hitTiming)
                .From(Vector3.one * 3);
            var currentColor = dynamicViewSpriteRenderer.color;
            var initColor    = new Color(currentColor.r, currentColor.g, currentColor.b, 0);
            var targetColor  = new Color(currentColor.r, currentColor.g, currentColor.b, 1);
            var coloringTween = dynamicViewSpriteRenderer
                .DOColor(targetColor, _hitTiming * 2)
                .From(initColor);

            return DOTween.Sequence()
                .Append(shrinkingTween)
                .Join(coloringTween);
        }

        private Tween ShrinkCircleFade()
        {
            var initColor = dynamicViewSpriteRenderer.color;
            var colorTween = dynamicViewSpriteRenderer.DOColor(
                new Color(initColor.r, initColor.g, initColor.b, 0),
                0.5f);

            var scaleTween = dynamicViewSpriteRenderer
                .transform.DOScale(Vector3.one * 3f, 0.5f);

            return DOTween.Sequence()
                .Append(colorTween)
                .Join(scaleTween);
        }
    }
}
