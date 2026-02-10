using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Features.MixMinigame.Views;
using UnityEngine;

namespace Features.MixMinigame.Presenters
{
    public class MixGameTileClickablePresenter : MixGameTilePresenter
    {
        [SerializeField] private SpriteRenderer staticViewSpriteRenderer;
        [SerializeField] private SpriteRenderer dynamicViewSpriteRenderer;

        [SerializeField] private Color dynamicHitColor;
        [SerializeField] private Color dynamicFailedColor;

        protected float HitTiming;

        public override void Initialize(MixGameTileView tileView)
        {
            base.Initialize(tileView);
            HitTiming = tileView.TileModel.HitTiming;

            dynamicViewSpriteRenderer.transform.localScale = Vector3.one;

            var textInitColor = textMeshVisualNumber.color;
            textMeshVisualNumber.color = new Color(r: textInitColor.r, g: textInitColor.g, b: textInitColor.b, a: 1);
            staticViewSpriteRenderer.color = Color.white;
            dynamicViewSpriteRenderer.color = Color.white;


            _ = PlayAnimationAndWaitAsync(animationName: "Shrink", layer: 1);
        }

        protected override void OnHit()
        {
            base.OnHit();
            dynamicViewSpriteRenderer.color = new Color(
                r: dynamicHitColor.r,
                g: dynamicHitColor.g,
                b: dynamicHitColor.b,
                a: dynamicViewSpriteRenderer.color.a);

            _ = PlayAnimationAndReturnToPoolAsync(animationName: "Hit", layer: 0);
            _ = PlayAnimationAndWaitAsync(animationName: "ShrinkCircleFade", layer: 1);
        }

        protected override void OnMiss()
        {
            dynamicViewSpriteRenderer.color = new Color(
                r: dynamicFailedColor.r,
                g: dynamicFailedColor.g,
                b: dynamicFailedColor.b,
                a: dynamicViewSpriteRenderer.color.a);
            base.OnMiss();
            _ = PlayAnimationAndReturnToPoolAsync(animationName: "Miss", layer: 0);
            _ = PlayAnimationAndWaitAsync(animationName: "ShrinkCircleFade", layer: 1);
        }

        protected override void OnFail()
        {
            base.OnFail();
            _ = PlayAnimationAndReturnToPoolAsync(animationName: "Fail", layer: 0);
            _ = PlayAnimationAndWaitAsync(animationName: "ShrinkCircleFade", layer: 1);
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
                _ => throw new ArgumentOutOfRangeException(paramName: nameof(animationName), actualValue: animationName,
                    message: null)
            };

            return MorphAnimationTweenToUniTask(tween: tween, ct: ct);
        }

        private Tween HitTween()
        {
            var staticColorTween = staticViewSpriteRenderer
                .DOColor(endValue: new Color(r: 1, g: 1, b: 1, a: 0), duration: 0.5f);
            var dynColorTween = dynamicViewSpriteRenderer
                .DOColor(endValue: new Color(r: 1, g: 1, b: 1, a: 0), duration: 0.5f);

            var textInitColor = textMeshVisualNumber.color;
            var textColorTween = textMeshVisualNumber
                .DOColor(endValue: new Color(r: textInitColor.r, g: textInitColor.g, b: textInitColor.b, a: 0),
                    duration: 0.25f);

            return DOTween.Sequence()
                .Append(staticColorTween)
                .Join(dynColorTween)
                .Join(textColorTween);
        }

        private Tween MissTween()
        {
            var staticColorTween = staticViewSpriteRenderer
                .DOColor(endValue: new Color(r: 1, g: 1, b: 1, a: 0), duration: 0.5f);
            var dynColorTween = dynamicViewSpriteRenderer
                .DOColor(endValue: new Color(r: 1, g: 1, b: 1, a: 0), duration: 0.5f);

            var textInitColor = textMeshVisualNumber.color;
            var textColorTween = textMeshVisualNumber
                .DOColor(endValue: new Color(r: textInitColor.r, g: textInitColor.g, b: textInitColor.b, a: 0),
                    duration: 0.25f);

            return DOTween.Sequence()
                .Append(staticColorTween)
                .Join(dynColorTween)
                .Join(textColorTween);
        }

        private Tween FailTween()
        {
            var staticColorTween = staticViewSpriteRenderer
                .DOColor(endValue: new Color(r: 1, g: 1, b: 1, a: 0), duration: 0.5f);
            var dynColorTween = dynamicViewSpriteRenderer
                .DOColor(endValue: new Color(r: 1, g: 1, b: 1, a: 0), duration: 0.5f);

            var textInitColor = textMeshVisualNumber.color;
            var textColorTween = textMeshVisualNumber
                .DOColor(endValue: new Color(r: textInitColor.r, g: textInitColor.g, b: textInitColor.b, a: 0),
                    duration: 0.25f);

            return DOTween.Sequence()
                .Append(staticColorTween)
                .Join(dynColorTween)
                .Join(textColorTween);
        }

        private Tween ShrinkTween()
        {
            var shrinkingTween = dynamicViewSpriteRenderer
                .transform.DOScale(endValue: Vector3.one, duration: HitTiming)
                .From(Vector3.one * 3);
            var currentColor = dynamicViewSpriteRenderer.color;
            var initColor    = new Color(r: currentColor.r, g: currentColor.g, b: currentColor.b, a: 0);
            var targetColor  = new Color(r: currentColor.r, g: currentColor.g, b: currentColor.b, a: 1);
            var coloringTween = dynamicViewSpriteRenderer
                .DOColor(endValue: targetColor, duration: HitTiming * 2)
                .From(initColor);

            return DOTween.Sequence()
                .Append(shrinkingTween)
                .Join(coloringTween);
        }

        private Tween ShrinkCircleFade()
        {
            var initColor = dynamicViewSpriteRenderer.color;
            var colorTween = dynamicViewSpriteRenderer.DOColor(
                endValue: new Color(r: initColor.r, g: initColor.g, b: initColor.b, a: 0),
                duration: 0.5f);

            var scaleTween = dynamicViewSpriteRenderer
                .transform.DOScale(endValue: Vector3.one * 3f, duration: 0.5f);

            return DOTween.Sequence()
                .Append(colorTween)
                .Join(scaleTween);
        }
    }
}
