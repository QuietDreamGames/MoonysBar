using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Features.MixMinigame.Datas;
using Features.MixMinigame.ViewModels;
using UnityEngine;

namespace Features.MixMinigame.Views
{
    public class MixGameTileDriftingView : MixGameTileClickableView
    {
        private float _driftFinalPositionY;

        public override void Initialize(MixGameTileViewModel tileViewModel)
        {
            base.Initialize(tileViewModel);

            var driftFinalRelativePositionY =
                ((MixGameDriftingSequenceElementData)tileViewModel.TileModel.Data)
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
            catch (ArgumentOutOfRangeException _)
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
