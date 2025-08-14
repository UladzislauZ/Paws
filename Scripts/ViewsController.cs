using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using FruityPaw.Scripts.Boosters;
using FruityPaw.Scripts.Info;
using FruityPaw.Scripts.Level;
using FruityPaw.Scripts.LuckyWheel;
using FruityPaw.Scripts.Main;
using FruityPaw.Scripts.Market;

namespace FruityPaw.Scripts
{
    public class ViewsController
    {
        private readonly MainController _mainController;
        private readonly InfoController _infoController;
        private readonly MarketViewController _marketViewController;
        private readonly BoostersViewController _boostersViewController;
        private readonly LevelViewController _levelViewController;
        private readonly LuckyViewController _luckyViewController;
        
        private IViewController _currentViewController;
        private CancellationTokenSource _cancellationTokenSource;
        
        public ViewsController(MainController mainController,
            InfoController infoController,
            MarketViewController marketViewController,
            BoostersViewController boostersViewController,
            LevelViewController levelViewController,
            LuckyViewController luckyViewController)
        {
            _mainController = mainController;
            _infoController = infoController;
            _marketViewController = marketViewController;
            _boostersViewController = boostersViewController;
            _levelViewController = levelViewController;
            _luckyViewController = luckyViewController;
        }

        public async UniTask StartAsync()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            SetViewController(ViewType.Main);
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                var result = await _currentViewController.Start();
                SetViewController(result);
            }
        }

        private void SetViewController(ViewType viewType)
        {
            _currentViewController = viewType switch
            {
                ViewType.Main => _mainController,
                ViewType.Info => _infoController,
                ViewType.Play => _levelViewController,
                ViewType.Market => _marketViewController,
                ViewType.Boosters => _boostersViewController,
                ViewType.DailyBonus => _luckyViewController,
                _ => throw new ArgumentOutOfRangeException(nameof(viewType), viewType, null)
            };
        }
    }
}