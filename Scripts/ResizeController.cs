using FruityPaw.Scripts.Backgrounds;
using FruityPaw.Scripts.Boosters;
using FruityPaw.Scripts.Info;
using FruityPaw.Scripts.Level;
using FruityPaw.Scripts.LuckyWheel;
using FruityPaw.Scripts.Main;
using FruityPaw.Scripts.Market;

namespace FruityPaw.Scripts
{
    public class ResizeController
    {
        private readonly BackgroundController _backgroundController;
        private readonly BoostersView _boostersView;
        private readonly MarketView _marketView;
        private readonly LuckyWheelView _luckyWheelView;
        private readonly MainView _mainView;
        private readonly LevelView _levelView;
        private readonly InfoView _infoView;

        public ResizeController(BackgroundController backgroundController,
            BoostersView boostersView,
            MarketView marketView,
            LuckyWheelView luckyWheelView,
            MainView mainView,
            LevelView levelView,
            InfoView infoView)
        {
            _backgroundController = backgroundController;
            _boostersView = boostersView;
            _marketView = marketView;
            _luckyWheelView = luckyWheelView;
            _mainView = mainView;
            _levelView = levelView;
            _infoView = infoView;
        }

        public void Start()
        {
            if (!_backgroundController.IsIpad) return;
            
            _boostersView.ResizeToIpad();
            _marketView.ResizeToIpad();
            _luckyWheelView.ResizeToIpad();
            _mainView.ResizeToIpad();
            _levelView.ResizeToIpad();
            _infoView.ResizeToIpad();
        }
    }
}