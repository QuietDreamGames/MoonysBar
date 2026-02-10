using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Features.MixMinigame.Datas;
using Features.MixMinigame.Views;
using UnityEngine;

namespace Features.MixMinigame.Presenters
{
    public class MixGameTileDriftingPresenter : MixGameTileClickablePresenter
    {
        private float _driftFinalPositionY;

        public override void Initialize(MixGameTileView tileView)
        {
            base.Initialize(tileView);

            var driftFinalRelativePositionY =
                ((MixGameDriftingSequenceElementData)tileView.TileModel.Data)
                .DriftFinalPositionY;

            var convertedPos =
                MixGamePlayingFieldService.ConvertRelativeToWorldPosition(new Vector2(x: 0,
                    y: driftFinalRelativePositionY));

            _driftFinalPositionY = convertedPos.y;

            _ = PlayAnimationAndWaitAsync(animationName: "Drifting", layer: 3);
        }

        protected override UniTask ResolveAnimation(string animationName, CancellationToken ct)
        {
            try
            {
                return base.ResolveAnimation(animationName: animationName, ct: ct);
            }
            catch (ArgumentOutOfRangeException)
            {
                if (animationName == "Drifting")
                    return MorphAnimationTweenToUniTask(tween: DriftingTween(), ct: ct);
                throw new ArgumentOutOfRangeException(paramName: nameof(animationName), actualValue: animationName,
                    message: null);
            }
        }

        private Tween DriftingTween()
        {
            return transform.DOLocalMoveY(endValue: _driftFinalPositionY, duration: HitTiming)
                .SetEase(Ease.InQuad);
        }
    }
}
