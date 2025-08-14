using System.Linq;
using Cysharp.Threading.Tasks;
using FruityPaw.Scripts.Data;
using FruityPaw.Scripts.Fruits;
using FruityPaw.Scripts.Level.Availables;
using FruityPaw.Scripts.Sounds;
using UnityEngine;

namespace FruityPaw.Scripts.Level
{
    public class BotController
    {
        private readonly LevelModel _levelModel;
        private readonly LevelView _levelView;
        private readonly FruitsConfig _fruitsConfig;
        private readonly AvailableController _availableController;
        private readonly DataController _dataController;
        private readonly ActionController _actionController;
        private readonly SoundController _soundController;

        public BotController(LevelModel levelModel,
            LevelView levelView,
            FruitsConfig fruitsConfig,
            AvailableController availableController,
            DataController dataController,
            ActionController actionController,
            SoundController soundController)
        {
            _levelModel = levelModel;
            _levelView = levelView;
            _fruitsConfig = fruitsConfig;
            _availableController = availableController;
            _dataController = dataController;
            _actionController = actionController;
            _soundController = soundController;
        }

        public async UniTask BotStepAsync()
        {
            var availableItem = _availableController.GetAvailableItem();
            if (availableItem.AvailableGameFields.Length == 0)
            {
                _levelModel.OnEnd = true;
                return;
            }

            if (Random.value > 0.3f)
            {
                foreach (var cell in availableItem.AvailableGameFields[0].Steps)
                {
                    var fieldPosition = _levelModel.GameFields[cell.RowIndex, cell.ColumnIndex].RectTransform.position;
                    await availableItem.Paw.MovePaw(fieldPosition, cell.RowIndex, cell.ColumnIndex);
                }
            }
            else
            {
                var index = Random.Range(0, availableItem.AvailableGameFields.Length);
                foreach (var cell in availableItem.AvailableGameFields[index].Steps)
                {
                    var fieldPosition = _levelModel.GameFields[cell.RowIndex, cell.ColumnIndex].RectTransform.position;
                    await availableItem.Paw.MovePaw(fieldPosition, cell.RowIndex, cell.ColumnIndex);
                }
            }

            await _actionController.StartBotActionAsync(availableItem.Paw);
            
            if (availableItem.Paw.RowIndexField == 7)
                TryAddFruit();
            
            if (_levelModel.BotStatistic.CountReward == 3)
            {
                _levelModel.OnEnd = true;
                _dataController.IncreaseBotWin();
            }

            foreach (var botPaw in _levelModel.BotPaws)
            {
                botPaw.UnFreeze();
            }
        }

        private void TryAddFruit()
        {
            var fruitType = (FruitType)Random.Range(0, 20);
            var fruitSprite = _fruitsConfig.fruits.First(x=>x.fruitType == fruitType).sprite;
            _levelModel.BotStatistic.AddReward(fruitType);
            _levelView.AddFruitToStatistics(fruitSprite, true);
            _soundController.PlaySound(SoundType.AddFruit);
        }
    }
}