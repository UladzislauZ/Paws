using Cysharp.Threading.Tasks;
using FruityPaw.Scripts.Backgrounds;
using FruityPaw.Scripts.Data;
using FruityPaw.Scripts.Fruits;
using FruityPaw.Scripts.Popup;
using FruityPaw.Scripts.Sounds;

namespace FruityPaw.Scripts.Market
{
    public class MarketViewController : IViewController
    {
        private readonly MarketView _marketView;
        private readonly MarketController _marketController;
        private readonly BackgroundController _backgroundController;
        private readonly PopupController _popupController;
        private readonly FruitsConfig _fruitsConfig;
        private readonly DataController _dataController;
        private readonly SoundController _soundController;
        private UniTaskCompletionSource<ViewType> _tcs;
        
        public MarketViewController(MarketView marketView,
            MarketController marketController,
            BackgroundController backgroundController,
            PopupController popupController,
            FruitsConfig fruitsConfig,
            DataController dataController,
            SoundController soundController)
        {
            _marketView = marketView;
            _marketController = marketController;
            _backgroundController = backgroundController;
            _popupController = popupController;
            _fruitsConfig = fruitsConfig;
            _dataController = dataController;
            _soundController = soundController;
        }

        public void Initialize()
        {
            _marketView.Initialize(_dataController.FruitItems, _fruitsConfig.fruits);
        }
        
        public async UniTask<ViewType> Start()
        {
            _backgroundController.UpdateBackground(BackgroundType.Game);
            _tcs = new UniTaskCompletionSource<ViewType>();
            _marketView.OnClose += Close;
            _marketView.OnCellFruit += CellFruit;
            _marketView.OnChartClick += ChartClick;
            _marketView.OnChartClose += ChartClose;
            _marketView.UpdateGolds(_dataController.Gold);
            foreach (var fruitItemData in _dataController.FruitItems)
            {
                _marketView.UpdateFruitData(fruitItemData);
            }
            
            _marketView.Show();
            _marketView.ShowSells();
            await _tcs.Task;
            _marketView.OnClose -= Close;
            _marketView.OnCellFruit -= CellFruit;
            _marketView.OnChartClick -= ChartClick;
            _marketView.OnChartClose -= ChartClose;
            _marketView.HideSells();
            _marketView.Hide();
            return _tcs.GetResult(0);
        }

        private void Close()
        {
            _marketView.OnClose -= Close;
            _soundController.PlaySound(SoundType.Click);
            _tcs.TrySetResult(ViewType.Main);
        }

        private void CellFruit(FruitType fruitType, int fruitAmount)
        {
            _soundController.PlaySound(SoundType.Click);
            CellFruitAsync(fruitType, fruitAmount).Forget();
        }

        private async UniTask CellFruitAsync(FruitType fruitType, int fruitAmount)
        {
            var result = await _popupController.ShowQuestionPopupAsync("Do you really\nwant to sell the\nfruit",
                "Yes", "No");
            if (!result || !_marketController.TrySellFruit(fruitType, fruitAmount)) return;
            
            _soundController.PlaySound(SoundType.Sell);
            _marketView.UpdateGolds(_dataController.Gold);
            _marketView.UpdateFruitData(_dataController.GetFruitItemData(fruitType));
        }

        private void ChartClick()
        {
            _soundController.PlaySound(SoundType.Click);
            _marketView.HideSells();
            _marketView.ShowCharts();
        }

        private void ChartClose()
        {
            _soundController.PlaySound(SoundType.Click);
            _marketView.HideCharts();
            _marketView.ShowSells();
        }
    }
}