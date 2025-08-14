using System;
using FruityPaw.Scripts.Data;
using FruityPaw.Scripts.Fruits;

namespace FruityPaw.Scripts.Market
{
    public class MarketController
    {
        private readonly DataController _dataController;
        
        public MarketController(DataController dataController)
        {
            _dataController = dataController;
        }

        public void Start()
        {
            var currentDay = DateTime.Today;
            if (currentDay.Date >= _dataController.LastOnline.Date) return;
            
            foreach (var fruitItem in _dataController.FruitItems)
            {
                fruitItem.CreateNewPrice();
            }
        }

        public bool TrySellFruit(FruitType fruitType, int amount)
        {
            var fruitItemData = _dataController.GetFruitItemData(fruitType);
            if (fruitItemData.Amount == 0 || amount > fruitItemData.Amount) return false;
            
            var coins = amount * fruitItemData.Price;
            fruitItemData.Amount -= amount;
            _dataController.AddGold(coins);
            return true;
        }
    }
}