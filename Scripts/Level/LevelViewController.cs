using System;
using Cysharp.Threading.Tasks;
using FruityPaw.Scripts.Backgrounds;
using FruityPaw.Scripts.Boosters;
using FruityPaw.Scripts.Data;
using FruityPaw.Scripts.Popup;

namespace FruityPaw.Scripts.Level
{
    public class LevelViewController : IViewController
    {
        private readonly LevelView _levelView;
        private readonly DataController _dataController;
        private readonly BackgroundController _backgroundController;
        private readonly LevelController _levelController;

        private UniTaskCompletionSource<ViewType> _tcs;

        public LevelViewController(LevelView levelView,
            DataController dataController,
            BackgroundController backgroundController,
            LevelController levelController)
        {
            _levelView = levelView;
            _dataController = dataController;
            _backgroundController = backgroundController;
            _levelController = levelController;
        }

        public void Initialize()
        {
            _levelView.Initialize();
            _levelController.OnClose += EndGame;
            _dataController.OnBoosterCountUpdate += CountBoosterUpdated;
        }
        
        public async UniTask<ViewType> Start()
        {
            _backgroundController.UpdateBackground(BackgroundType.Game);
            _tcs = new UniTaskCompletionSource<ViewType>();
            foreach (BoosterType boosterType in Enum.GetValues(typeof(BoosterType)))
            {
                _levelView.UpdateCountBooster(boosterType, _dataController.GetBoosterCount(boosterType));
            }

            _levelView.OnClose += Close;
            _levelView.ActivateButtons();
            _levelView.Show();
            await UniTask.Yield();
            _levelController.CreateLevel();
            await _tcs.Task;
            _levelView.OnClose -= Close;
            _levelView.DeactivateButtons();
            _levelView.Hide();
            return _tcs.GetResult(0);
        }

        private void CountBoosterUpdated(BoosterType boosterType, int count)
        {
            _levelView.UpdateCountBooster(boosterType, count);
        }

        private void EndGame(ViewType viewType)
        {
            _levelController.StopLevel();
            _tcs?.TrySetResult(viewType);
        }

        private void Close()
        {
            _levelView.OnClose -= Close;
            _levelController.StopLevel();
            _tcs.TrySetResult(ViewType.Main);
        }
    }
}