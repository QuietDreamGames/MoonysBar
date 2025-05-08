namespace Features.MixMinigame.Views
{
    public class MixGameTileClickableView : MixGameTileView
    {
        protected override void OnHit()
        {
            _ = PlayAnimationAndReturnToPoolAsync("Hit");
        }

        protected override void OnMiss()
        {
            _ = PlayAnimationAndReturnToPoolAsync("Miss");
        }

        protected override void OnFail()
        {
            _ = PlayAnimationAndReturnToPoolAsync("Fail");
        }
    }
}
