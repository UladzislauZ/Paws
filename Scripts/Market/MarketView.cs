using System;
using System.Linq;
using FruityPaw.Scripts.Data;
using FruityPaw.Scripts.Fruits;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FruityPaw.Scripts.Market
{
    public class MarketView : MonoBehaviour
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private TMP_Text goldText;
        [SerializeField] private Button chartButton;
        [SerializeField] private MarketItem[] marketItems;
        [SerializeField] private ChartItem[] chartItems;
        [SerializeField] private RectTransform sellContentRectTransform;
        [SerializeField] private RectTransform chartContentRectTransform;

        public event Action OnClose;
        public event Action OnChartClick;
        public event Action OnChartClose;
        public event Action<FruitType, int> OnCellFruit;

        public void ResizeToIpad()
        {
            sellContentRectTransform.offsetMin = new Vector2(sellContentRectTransform.offsetMin.x, 100f);
            sellContentRectTransform.offsetMax = new Vector2(sellContentRectTransform.offsetMax.x, -100f);
            chartContentRectTransform.offsetMin = new Vector2(chartContentRectTransform.offsetMin.x, 220f);
            chartContentRectTransform.offsetMax = new Vector2(chartContentRectTransform.offsetMax.x, -100f);
        }

        public void Initialize(FruitItemData[] fruitItems, Fruit[] fruitSprites)
        {
            for (var i = 0; i < fruitItems.Length; i++)
            {
                var fruitSprite = fruitSprites.First(x => x.fruitType == fruitItems[i].FruitType).sprite;
                chartItems[i].Initialize(fruitItems[i].FruitType, fruitSprite, 
                    fruitItems[i].HistoryPrices, fruitItems[i].MaxPrice);
                marketItems[i].Initialize(fruitItems[i].FruitType, fruitSprite);
                marketItems[i].OnBuyClick += BuyClicked;
            }
        }

        public void UpdateGolds(int gold)
        {
            goldText.text = gold.ToString();
        }

        public void UpdateFruitData(FruitItemData fruitItemData)
        {
            chartItems.First(x => x.FruitType == fruitItemData.FruitType)
                .UpdateChart(fruitItemData.Amount, fruitItemData.Price);
            marketItems.First(x => x.FruitType == fruitItemData.FruitType)
                .UpdateCount(fruitItemData.Amount, fruitItemData.Price);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
        
        public void ShowSells()
        {
            closeButton.onClick.AddListener(Close);
            chartButton.onClick.AddListener(ChartsClick);
            sellContentRectTransform.gameObject.SetActive(true);
        }

        public void HideSells()
        {
            closeButton.onClick.RemoveAllListeners();
            chartButton.onClick.RemoveAllListeners();
            sellContentRectTransform.gameObject.SetActive(false);
        }

        public void ShowCharts()
        {
            closeButton.onClick.AddListener(Back);
            chartContentRectTransform.gameObject.SetActive(true);
            chartButton.gameObject.SetActive(false);
        }

        public void HideCharts()
        {
            closeButton.onClick.RemoveAllListeners();
            chartContentRectTransform.gameObject.SetActive(false);
            chartButton.gameObject.SetActive(true);
        }
        
        private void Close()
        {
            closeButton.onClick.RemoveAllListeners();
            OnClose?.Invoke();
        }

        private void Back()
        {
            closeButton.onClick.RemoveAllListeners();
            OnChartClose?.Invoke();
        }

        private void ChartsClick()
        {
            chartButton.onClick.RemoveAllListeners();
            OnChartClick?.Invoke();
        }

        private void BuyClicked(FruitType fruitType, int amount)
        {
            OnCellFruit?.Invoke(fruitType, amount);
        }
    }
}