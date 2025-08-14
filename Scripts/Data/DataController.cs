using System;
using System.Linq;
using FruityPaw.Scripts.Boosters;
using FruityPaw.Scripts.Fruits;
using Newtonsoft.Json;
using UnityEngine;

namespace FruityPaw.Scripts.Data
{
    public class DataController
    {
        private readonly FruitsConfig _fruitsConfig;
        
        private const string Key = "FruityData";
        
        private DataModel _dataModel;

        public int BotWinCount => _dataModel.BotWinCount;
        public int PlayerWinCount => _dataModel.PlayerWinCount;
        public int Gold => _dataModel.Gold;
        public FruitItemData[] FruitItems => _dataModel.FruitItems;
        public DateTime LastOnline => _dataModel.LastOnline;
        
        public event Action<BoosterType, int> OnBoosterCountUpdate; 

        public bool DailyIsActive()
        {
            var difference = DateTime.Now - _dataModel.LastOpenDaily;
            return difference.TotalHours >= 6;
        }

        public DataController(FruitsConfig fruitsConfig)
        {
            _fruitsConfig = fruitsConfig;
        }

        public FruitItemData GetFruitItemData(FruitType fruitType)
        {
            return _dataModel.FruitItems.First(x => x.FruitType == fruitType);
        }

        public int GetBoosterCount(BoosterType boosterType)
        {
            return boosterType switch
            {
                BoosterType.Freeze => _dataModel.Freeze,
                BoosterType.Swap => _dataModel.Swap,
                BoosterType.OneMove => _dataModel.OneExtra,
                BoosterType.ComeLine => _dataModel.ComeLine,
                _ => throw new ArgumentOutOfRangeException(nameof(boosterType), boosterType, null)
            };
        }

        public void SpendBooster(BoosterType boosterType)
        {
            switch (boosterType)
            {
                case BoosterType.Freeze:
                    _dataModel.Freeze--;
                    if (_dataModel.Freeze < 0)
                        _dataModel.Freeze = 0;
                    
                    OnBoosterCountUpdate?.Invoke(BoosterType.Freeze, _dataModel.Freeze);
                    break;
                case BoosterType.Swap:
                    _dataModel.Swap--;
                    if (_dataModel.Swap < 0)
                        _dataModel.Swap = 0;
                    
                    OnBoosterCountUpdate?.Invoke(BoosterType.Swap, _dataModel.Swap);
                    break;
                case BoosterType.OneMove:
                    _dataModel.OneExtra--;
                    if (_dataModel.OneExtra < 0)
                        _dataModel.OneExtra = 0;
                    
                    OnBoosterCountUpdate?.Invoke(BoosterType.OneMove, _dataModel.OneExtra);
                    break;
                case BoosterType.ComeLine:
                    _dataModel.ComeLine--;
                    if (_dataModel.ComeLine < 0)
                        _dataModel.ComeLine = 0;
                    
                    OnBoosterCountUpdate?.Invoke(BoosterType.ComeLine, _dataModel.ComeLine);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(boosterType), boosterType, null);
            }
            
            SaveData();
        }

        public void Start()
        {
            var json = PlayerPrefs.GetString(Key, string.Empty);
            if (string.IsNullOrEmpty(json))
            {
                _dataModel = new DataModel();
                _dataModel.SetFruits(_fruitsConfig.fruits);
                SaveData();
                return;
            }

            _dataModel = JsonConvert.DeserializeObject<DataModel>(json);
        }

        public void IncreaseBotWin()
        {
            _dataModel.BotWinCount++;
            SaveData();
        }

        public void IncreasePlayerWin()
        {
            _dataModel.PlayerWinCount++;
            SaveData();
        }

        public void CollectDaily()
        {
            _dataModel.LastOpenDaily = DateTime.Now;
            SaveData();
        }

        public void AddFreeze()
        {
            _dataModel.Freeze++;
            SaveData();
        }

        public void AddSwap()
        {
            _dataModel.Swap++;
            SaveData();
        }

        public void AddOneExtra()
        {
            _dataModel.OneExtra++;
            SaveData();
        }

        public void AddComeLine()
        {
            _dataModel.ComeLine++;
            SaveData();
        }

        public void SpendGold(int amount)
        {
            _dataModel.Gold -= amount;
            SaveData();
        }

        public void AddGold(int amount)
        {
            _dataModel.Gold += amount;
            SaveData();
        }

        public void AddFruitItem(FruitType fruitType)
        {
            _dataModel.FruitItems.First(x => x.FruitType == fruitType).Amount++;
            SaveData();
        }

        private void SaveData()
        {
            var json = JsonConvert.SerializeObject(_dataModel);
            PlayerPrefs.SetString(Key, json);
        }
    }
}