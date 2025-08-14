using Cysharp.Threading.Tasks;
using FruityPaw.Scripts.Backgrounds;
using FruityPaw.Scripts.Boosters;
using FruityPaw.Scripts.Data;
using FruityPaw.Scripts.Level;
using FruityPaw.Scripts.Market;
using FruityPaw.Scripts.Sounds;

namespace FruityPaw.Scripts
{
    public class RootController
    {
        private readonly DataController _dataController;
        private readonly MarketController _marketController;
        private readonly MarketViewController _marketViewController;
        private readonly SoundController _soundController;
        private readonly LevelViewController _levelViewController;
        private readonly BoostersController _boostersController;
        private readonly LevelController _levelController;
        private readonly ViewsController _viewsController;
        private readonly PlayerController _playerController;
        private readonly ResizeController _resizeController;

        public RootController(DataController dataController,
            MarketController marketController,
            MarketViewController marketViewController,
            SoundController soundController,
            LevelViewController levelViewController,
            BoostersController boostersController,
            LevelController levelController,
            ViewsController viewsController,
            PlayerController playerController,
            ResizeController resizeController)
        {
            _dataController = dataController;
            _marketController = marketController;
            _marketViewController = marketViewController;
            _soundController = soundController;
            _levelViewController = levelViewController;
            _boostersController = boostersController;
            _levelController = levelController;
            _viewsController = viewsController;
            _playerController = playerController;
            _resizeController = resizeController;
        }

        public void Start()
        {
            _resizeController.Start();
            _dataController.Start();
            _soundController.Start();
            _marketController.Start();
            _marketViewController.Initialize();
            _levelViewController.Initialize();
            _boostersController.Start();
            _playerController.Initialize();
            _levelController.Start();
            _viewsController.StartAsync().Forget();
        }
    }
}