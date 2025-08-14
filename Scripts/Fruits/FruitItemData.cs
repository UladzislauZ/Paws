using System;
using Newtonsoft.Json;
using Random = UnityEngine.Random;

namespace FruityPaw.Scripts.Fruits
{
    [Serializable]
    public class FruitItemData
    {
        [JsonProperty] public FruitType FruitType;
        [JsonProperty] public int Amount;
        [JsonProperty] public int Price;
        [JsonProperty] public int MaxPrice;
        [JsonProperty] public int MinPrice;
        [JsonProperty] public int[] HistoryPrices;

        public FruitItemData(FruitType fruitType, int minPrice, int maxPrice)
        {
            FruitType = fruitType;
            Amount = 0;
            MinPrice = minPrice;
            MaxPrice = maxPrice;
            Price = Random.Range(minPrice, maxPrice);
            HistoryPrices = new[]
            {
                Random.Range(minPrice, maxPrice),
                Random.Range(minPrice, maxPrice),
                Random.Range(minPrice, maxPrice),
                Random.Range(minPrice, maxPrice),
                Random.Range(minPrice, maxPrice)
            };
        }

        public void CreateNewPrice()
        {
            for (var i = 0; i < 4; i++)
            {
                HistoryPrices[i] = HistoryPrices[i + 1];
            }

            HistoryPrices[4] = Price;
            var newPrice = Random.Range(MinPrice, MaxPrice);
            Price = newPrice;
        }
    }
}