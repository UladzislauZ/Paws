using Cysharp.Threading.Tasks;
using FruityPaw.Scripts.Backgrounds;
using FruityPaw.Scripts.Data;
using FruityPaw.Scripts.Sounds;
using UnityEngine;

namespace FruityPaw.Scripts.Main
{
    public class MainController : IViewController
    {
        private readonly MainView _mainView;
        private readonly DataController _dataController;
        private readonly SoundController _soundController;
        private readonly SoundConfig _soundConfig;
        private readonly BackgroundController _backgroundController;

        private UniTaskCompletionSource<ViewType> _tcs;

        public MainController(MainView mainView,
            DataController dataController,
            SoundController soundController,
            SoundConfig soundConfig,
            BackgroundController backgroundController)
        {
            _mainView = mainView;
            _dataController = dataController;
            _soundController = soundController;
            _soundConfig = soundConfig;
            _backgroundController = backgroundController;
        }

        public async UniTask<ViewType> Start()
        {
            _backgroundController.UpdateBackground(BackgroundType.Main);
            _tcs = new UniTaskCompletionSource<ViewType>();
            _mainView.OnSoundChange += SoundChange;
            _mainView.OnClick += Click;
            _mainView.UpdateSoundImage(GetSoundSprite());
            _mainView.Show(GetTotalWinText(), _dataController.DailyIsActive());
            await _tcs.Task;
            _mainView.OnSoundChange -= SoundChange;
            _mainView.OnClick -= Click;
            _mainView.Hide();
            return _tcs.GetResult(0);
        }

        private void SoundChange()
        {
            _soundController.ChangeOnSound();
            _soundController.PlaySound(SoundType.Click);
            _mainView.UpdateSoundImage(GetSoundSprite());
        }

        private void Click(ViewType viewType)
        {
            _soundController.PlaySound(SoundType.Click);
            _tcs.TrySetResult(viewType);
        }

        private string GetTotalWinText()
        {
            return $"{_dataController.BotWinCount}/{_dataController.PlayerWinCount}";
        }

        private Sprite GetSoundSprite()
        {
            return _soundController.OnSound ? _soundConfig.onSoundSprite : _soundConfig.offSoundSprite;
        }
    }
}