using Cysharp.Threading.Tasks;
using FruityPaw.Scripts.Backgrounds;
using FruityPaw.Scripts.Data;
using FruityPaw.Scripts.Popup;
using FruityPaw.Scripts.Sounds;

namespace FruityPaw.Scripts.Boosters
{
    public class BoostersViewController : IViewController
    {
        private readonly BoostersView _boostersView;
        private readonly BoostersController _boostersController;
        private readonly PopupController _popupController;
        private readonly BackgroundController _backgroundController;
        private readonly DataController _dataController;
        private readonly SoundController _soundController;

        private UniTaskCompletionSource<ViewType> _tcs;
        private bool _locked;

        public BoostersViewController(BoostersView boostersView,
            BoostersController boostersController,
            PopupController popupController,
            BackgroundController backgroundController,
            DataController dataController,
            SoundController soundController)
        {
            _boostersView = boostersView;
            _boostersController = boostersController;
            _popupController = popupController;
            _backgroundController = backgroundController;
            _dataController = dataController;
            _soundController = soundController;
        }

        public async UniTask<ViewType> Start()
        {
            _backgroundController.UpdateBackground(BackgroundType.Game);
            _tcs = new UniTaskCompletionSource<ViewType>();
            Subscribe();
            UpdateBoosters();
            _boostersView.Show();
            await _tcs.Task;
            Unsubscribe();
            _boostersView.Hide();
            return _tcs.GetResult(0);
        }

        private void ActivateButtons()
        {
            var gold = _dataController.Gold;
            _boostersView.ActivateFreezeBuyButton(gold >= BoostersController.FreezePrice);
            _boostersView.ActivateSwapBuyButton(gold >= BoostersController.SwapPrice);
            _boostersView.ActivateOneExtraBuyButton(gold >= BoostersController.OneExtraPrice);
            _boostersView.ActivateComeLineBuyButton(gold >= BoostersController.ComeLinePrice);
        }

        private void Subscribe()
        {
            _boostersView.OnClose += Close;
            _boostersView.OnFreezeBuyClick += BuyFreeze;
            _boostersView.OnSwapBuyClick += BuySwap;
            _boostersView.OnOneExtraBuyClick += BuyOneExtra;
            _boostersView.OnComeLineBuyClick += BuyComeLine;
        }

        private void Unsubscribe()
        {
            _boostersView.OnClose -= Close;
            _boostersView.OnFreezeBuyClick -= BuyFreeze;
            _boostersView.OnSwapBuyClick -= BuySwap;
            _boostersView.OnOneExtraBuyClick -= BuyOneExtra;
            _boostersView.OnComeLineBuyClick -= BuyComeLine;
        }

        private void Close()
        {
            _boostersView.OnClose -= Close;
            _soundController.PlaySound(SoundType.Click);
            _tcs.TrySetResult(ViewType.Main);
        }

        private void BuyFreeze()
        {
            if (_locked) return;
            
            _locked = true;
            _soundController.PlaySound(SoundType.Click);
            TryBuyBooster(BoosterType.Freeze).Forget();
        }

        private void BuySwap()
        {
            if (_locked) return;

            _locked = true;
            _soundController.PlaySound(SoundType.Click);
            TryBuyBooster(BoosterType.Swap).Forget();
        }

        private void BuyOneExtra()
        {
            if (_locked) return;

            _locked = true;
            _soundController.PlaySound(SoundType.Click);
            TryBuyBooster(BoosterType.OneMove).Forget();
        }

        private void BuyComeLine()
        {
            if (_locked) return;

            _locked = true;
            _soundController.PlaySound(SoundType.Click);
            TryBuyBooster(BoosterType.ComeLine).Forget();
        }

        private void UpdateBoosters()
        {
            _boostersView.UpdateGold(_dataController.Gold);
            _boostersView.SetCounts(_dataController.GetBoosterCount(BoosterType.Freeze),
                _dataController.GetBoosterCount(BoosterType.Swap),
                _dataController.GetBoosterCount(BoosterType.OneMove),
                _dataController.GetBoosterCount(BoosterType.ComeLine));
            ActivateButtons();
        }

        private async UniTask TryBuyBooster(BoosterType boosterType)
        {
            var question = $"Do you want to buy booster?";
            var result = await _popupController.ShowQuestionPopupAsync(
                question,
                "Yes",
                "No");
            _locked = false;
            if (!result) return;

            _soundController.PlaySound(SoundType.Sell);
            if (!_boostersController.TryBuyBooster(boosterType)) return;
            UpdateBoosters();
        }
    }
}