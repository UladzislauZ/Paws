using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using FruityPaw.Scripts.Boosters;
using FruityPaw.Scripts.Data;
using FruityPaw.Scripts.Fruits;
using FruityPaw.Scripts.Level.Availables;
using FruityPaw.Scripts.Level.FieldObjects;
using FruityPaw.Scripts.Level.Paws;
using FruityPaw.Scripts.Sounds;
using UnityEngine;

namespace FruityPaw.Scripts.Level
{
    public class PlayerController
    {
        private readonly AvailableController _availableController;
        private readonly LevelModel _levelModel;
        private readonly ActionController _actionController;
        private readonly DataController _dataController;
        private readonly FruitsConfig _fruitsConfig;
        private readonly LevelView _levelView;
        private readonly BoostersController _boostersController;
        private readonly ComeLineBoosterController _comeLineBoosterController;
        private readonly SwapBoosterController _swapBoosterController;
        private readonly SoundController _soundController;

        private List<GameField> _availableFields = new ();
        private List<Paw> _availablePaws = new ();
        private UniTaskCompletionSource _stepTcs;
        
        public PlayerController(AvailableController availableController,
            LevelModel levelModel,
            ActionController actionController,
            DataController dataController,
            FruitsConfig fruitsConfig,
            LevelView levelView,
            BoostersController boostersController,
            ComeLineBoosterController comeLineBoosterController,
            SwapBoosterController swapBoosterController,
            SoundController soundController)
        {
            _availableController = availableController;
            _levelModel = levelModel;
            _actionController = actionController;
            _dataController = dataController;
            _fruitsConfig = fruitsConfig;
            _levelView = levelView;
            _boostersController = boostersController;
            _comeLineBoosterController = comeLineBoosterController;
            _swapBoosterController = swapBoosterController;
            _soundController = soundController;
        }

        public void Initialize()
        {
            _comeLineBoosterController.OnComeLineActivate += ComeLineActivate;
            _swapBoosterController.OnStartBooster += ClearSelected;
            _swapBoosterController.OnEndBooster += RestartStep;
        }
        
        public async UniTask PlayerStepAsync(bool withOutBoosters = false)
        {
            _stepTcs = new UniTaskCompletionSource();
            _availablePaws.AddRange(_availableController.GetAvailablePaws());
            if (_availablePaws.Count == 0)
            {
                _levelModel.OnEnd = true;
                return;
            }

            foreach (var playerPaw in _availablePaws)
            {
                playerPaw.OnPawSelected += PawSelect;
                playerPaw.ActivateColliderPaw(true);
            }

            _boostersController.UpdateClickableBoosters(!withOutBoosters);
            await _stepTcs.Task;
            _levelModel.CurrentPaw = null;
            
            foreach (var playerPaw in _availablePaws)
            {
                playerPaw.OnPawSelected -= PawSelect;
                playerPaw.ActivateColliderPaw(false);
            }

            _availablePaws.Clear();
            
            if (_levelModel.PlayerStatistic.CountReward == 3)
            {
                _levelModel.OnEnd = true;
            }
        }

        private void ComeLineActivate()
        {
            ClearSelected();
            _availablePaws.AddRange(_availableController.GetAvailablePaws(true));
            if (_availablePaws.Count == 0)
            {
                _levelModel.OnEnd = true;
                return;
            }

            foreach (var playerPaw in _availablePaws)
            {
                playerPaw.OnPawSelected += PawSelect;
                playerPaw.ActivateColliderPaw(true);
            }
        }

        private void ClearSelected()
        {
            foreach (var playerPaw in _availablePaws)
            {
                playerPaw.OnPawSelected -= PawSelect;
                playerPaw.ActivateColliderPaw(false);
            }
            
            if (_levelModel.CurrentPaw != null) 
                UnselectFields();
            
            _availablePaws.Clear();
            _levelModel.CurrentPaw = null;
        }

        private void RestartStep()
        {
            _availablePaws.AddRange(_availableController.GetAvailablePaws());
            if (_availablePaws.Count == 0)
            {
                _levelModel.OnEnd = true;
                return;
            }

            foreach (var playerPaw in _availablePaws)
            {
                playerPaw.OnPawSelected += PawSelect;
                playerPaw.ActivateColliderPaw(true);
            }
        }
        
        private void PawSelect(Paw paw)
        {
            if (_swapBoosterController.IsSwapActivate) return;
            
            if (_levelModel.CurrentPaw == paw) return;
            
            if (_levelModel.CurrentPaw != null) UnselectFields();

            _soundController.PlaySound(SoundType.PawSelect);
            _levelModel.CurrentPaw = paw;
            SelectFields();
        }

        private void SelectFields()
        {
            _levelModel.CurrentPaw.UpdateLayout(10);
            _levelModel.GameFields[_levelModel.CurrentPaw.RowIndexField, _levelModel.CurrentPaw.ColumnIndexField]
                .SelectField(true);
            ActivateGameFields();
        }

        private void UnselectFields()
        {
            _levelModel.CurrentPaw.UpdateLayout(5);
            _levelModel.GameFields[_levelModel.CurrentPaw.RowIndexField, _levelModel.CurrentPaw.ColumnIndexField]
                .SelectField(false);
            DeactivateGameFields();
        }

        private void ActivateGameFields()
        {
            var cells = _availableController.GetAvailableFields();
            _availableFields.AddRange(_levelModel.GetGameFieldsByCells(cells));
            foreach (var availableField in _availableFields)
            {
                availableField.OnClicked += FieldClick;
                availableField.ActivateClickable();
            }
        }

        private void DeactivateGameFields()
        {
            foreach (var availableField in _availableFields)
            {
                availableField.OnClicked -= FieldClick;
                availableField.DeactivateClickable();
            }
            
            _availableFields.Clear();
        }

        private void FieldClick(int rowIndexField, int columnIndexField)
        {
            if (_swapBoosterController.IsSwapActivate) return;
            
            foreach (var playerPaw in _availablePaws)
            {
                playerPaw.OnPawSelected -= PawSelect;
                playerPaw.ActivateColliderPaw(false);
            }
            DeactivateGameFields();
            _levelModel.GameFields[_levelModel.CurrentPaw.RowIndexField, _levelModel.CurrentPaw.ColumnIndexField]
                .SelectField(false);
            _boostersController.UpdateClickableBoosters(false);
            MovePaw(rowIndexField, columnIndexField).Forget();
        }

        private async UniTask MovePaw(int rowIndexField, int columnIndexField)
        {
            var stepCells = _availableController.GetStepsFields(rowIndexField, columnIndexField);
            foreach (var cell in stepCells)
            {
                var fieldPosition = _levelModel.GameFields[cell.RowIndex, cell.ColumnIndex].RectTransform.position;
                await _levelModel.CurrentPaw.MovePaw(fieldPosition, cell.RowIndex, cell.ColumnIndex);
            }

            await _actionController.StartPlayerActionAsync();
            if (_levelModel.CurrentPaw.RowIndexField == 0)
                TryAddFruit();
            
            _stepTcs.TrySetResult();
        }

        private void TryAddFruit()
        {
            var fruitType = (FruitType)Random.Range(0, 20);
            var fruitSprite = _fruitsConfig.fruits.First(x=>x.fruitType == fruitType).sprite;
            _dataController.AddFruitItem(fruitType);
            _levelModel.PlayerStatistic.AddReward(fruitType);
            _levelView.AddFruitToStatistics(fruitSprite, false);
            _soundController.PlaySound(SoundType.AddFruit);
        }
    }
}