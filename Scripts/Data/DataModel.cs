using System;
using FruityPaw.Scripts.Fruits;
using Newtonsoft.Json;

namespace FruityPaw.Scripts.Data
{
    public class DataModel
    {
        [JsonProperty] public int BotWinCount = 0;
        [JsonProperty] public int PlayerWinCount = 0;
        [JsonProperty] public int Gold = 50;
        [JsonProperty] public int Freeze = 1;
        [JsonProperty] public int Swap = 1;
        [JsonProperty] public int OneExtra = 1;
        [JsonProperty] public int ComeLine = 1;
        [JsonProperty] public FruitItemData[] FruitItems;
        [JsonProperty] public DateTime LastOnline = DateTime.Now;
        [JsonProperty] public DateTime LastOpenDaily;

        public void SetFruits(Fruit[] fruits)
        {
            FruitItems = new FruitItemData[fruits.Length];
            for (var i = 0; i < fruits.Length; i++)
            {
                FruitItems[i] = new FruitItemData(fruits[i].fruitType, fruits[i].minPrice, fruits[i].maxPrice);
            }
        }
    }
}