namespace FruityPaw.Scripts.Backgrounds
{
    public class BackgroundController
    {
        private readonly BackgroundView _backgroundView;
        private readonly BackgroundConfig _backgroundConfig;

        public bool IsIpad => _backgroundView.IsIpadOrientation;

        public BackgroundController(BackgroundView backgroundView,
            BackgroundConfig backgroundConfig)
        {
            _backgroundView = backgroundView;
            _backgroundConfig = backgroundConfig;
        }

        public void UpdateBackground(BackgroundType backgroundType)
        {
            var sprite = _backgroundConfig.GetBackgroundSprite(backgroundType, IsIpad);
            _backgroundView.AdjustResolution(sprite);
        }
    }
}