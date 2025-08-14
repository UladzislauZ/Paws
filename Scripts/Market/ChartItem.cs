using FruityPaw.Scripts.Data;
using FruityPaw.Scripts.Fruits;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FruityPaw.Scripts.Market
{
    public class ChartItem : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text countText;
        [SerializeField] private TMP_Text priceText;
        [SerializeField] private ProgressBar[] daysAgoProgressBars;
        
        public FruitType FruitType{get; private set; }

        public void Initialize(FruitType fruitType,
            Sprite sprite,
            int[] daysAgoPrices,
            int maxDaysAgoPrices)
        {
            FruitType = fruitType;
            image.sprite = sprite;
            for (var i = 0; i < daysAgoPrices.Length; i++)
            {
                daysAgoProgressBars[i].UpdateProgress(daysAgoPrices[i], maxDaysAgoPrices);
            }
        }

        public void UpdateChart(int count,
            int price)
        {
            countText.text = count.ToString();
            priceText.text = price.ToString();
        }
    }
}