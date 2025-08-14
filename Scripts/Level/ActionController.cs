using System.Linq;
using Cysharp.Threading.Tasks;
using FruityPaw.Scripts.Level.Availables;
using FruityPaw.Scripts.Level.Paws;
using FruityPaw.Scripts.Popup;
using UnityEngine;

namespace FruityPaw.Scripts.Level
{
    public class ActionController
    {
        private readonly LevelModel _levelModel;
        private readonly PopupController _popupController;
        private readonly AvailableController _availableController;
        
        private UniTaskCompletionSource _tcs;
        private Cell _cell;
        private FieldStep[] _fieldSteps;

        public ActionController(LevelModel levelModel,
            PopupController popupController,
            AvailableController availableController)
        {
            _levelModel = levelModel;
            _popupController = popupController;
            _availableController = availableController;
        }

        public async UniTask StartPlayerActionAsync()
        {
            if (_levelModel.BotPaws.Any(x =>
                    x.RowIndexField == _levelModel.CurrentPaw.RowIndexField
                    && x.ColumnIndexField == _levelModel.CurrentPaw.ColumnIndexField))
            {
                var result = await _popupController.ShowQuestionPopupAsync(
                    "Select an action", "Jump", "Blow");
                if (result)
                {
                    await StartJumpAsync();
                }
                else
                {
                    await StartBlowAsync();
                }

                await StartPlayerActionAsync();
            }
        }

        public async UniTask StartBotActionAsync(Paw paw)
        {
            if (_levelModel.PlayerPaws.Any(x =>
                    x.RowIndexField == paw.RowIndexField
                    && x.ColumnIndexField == paw.ColumnIndexField))
            {
                var result = Random.value < 0.5f;
                if (result)
                {
                    await StartBotJumpAsync(paw);
                }
                else
                {
                    await StartBotBlowAsync(paw);
                }

                await StartBotActionAsync(paw);
            }
        }

        private async UniTask StartJumpAsync()
        {
            _tcs = new UniTaskCompletionSource();
            var botPawType = _levelModel.BotPaws.First(x =>
                x.RowIndexField == _levelModel.CurrentPaw.RowIndexField &&
                x.ColumnIndexField == _levelModel.CurrentPaw.ColumnIndexField).TypePaw;
            _fieldSteps = _availableController.GetAvailableCells(botPawType,
                _levelModel.CurrentPaw.RowIndexField,
                _levelModel.CurrentPaw.ColumnIndexField);
            if (_fieldSteps == null)
            {
                await StartBlowAsync();
                return;
            }

            var gameFields = _levelModel.GetGameFieldsByCells(_fieldSteps.Select(x => x.GameField)
                    .ToArray()).ToArray();
            foreach (var gameField in gameFields)
            {
                gameField.OnClicked += FieldClick;
                gameField.ActivateClickable();
            }
            
            await _tcs.Task;
            
            foreach (var gameField in gameFields)
            {
                gameField.OnClicked -= FieldClick;
                gameField.DeactivateClickable();
            }

            foreach (var stepCell in _fieldSteps.First(x=>x.GameField.RowIndex == _cell.RowIndex 
                                                          && x.GameField.ColumnIndex == _cell.ColumnIndex).Steps)
            {
                var field = _levelModel.GameFields[stepCell.RowIndex, stepCell.ColumnIndex];
                await _levelModel.CurrentPaw.MovePaw(field.RectTransform.position, field.FieldRowIndex,
                    field.FieldColumnIndex);
            }
        }

        private async UniTask StartBlowAsync()
        {
            var botPaw = _levelModel.BotPaws.First(x =>
                x.RowIndexField == _levelModel.CurrentPaw.RowIndexField &&
                x.ColumnIndexField == _levelModel.CurrentPaw.ColumnIndexField);
            var newCell = _availableController.GetAvailableCell(botPaw, true);
            await botPaw.MovePaw(
                _levelModel.GameFields[newCell.RowIndex, newCell.ColumnIndex].RectTransform.position,
                newCell.RowIndex,
                newCell.ColumnIndex);
        }
        
        private async UniTask StartBotJumpAsync(Paw botPaw)
        {
            var playerPawType = _levelModel.PlayerPaws.First(x =>
                x.RowIndexField == botPaw.RowIndexField &&
                x.ColumnIndexField == botPaw.ColumnIndexField).TypePaw;
            var availableCell = _availableController.GetAvailableCellForBot(
                playerPawType,
                botPaw.RowIndexField,
                botPaw.ColumnIndexField);
            if (availableCell == null)
            {
                await StartBotBlowAsync(botPaw);
                return;
            }

            foreach (var stepCell in availableCell.Steps)
            {
                var gameField = _levelModel.GameFields[stepCell.RowIndex, stepCell.ColumnIndex];
                await botPaw.MovePaw(
                    gameField.RectTransform.position, 
                    gameField.FieldRowIndex,
                    gameField.FieldColumnIndex);
            }
        }

        private async UniTask StartBotBlowAsync(Paw botPaw)
        {
            var playerPaw = _levelModel.PlayerPaws.First(x =>
                x.RowIndexField == botPaw.RowIndexField &&
                x.ColumnIndexField == botPaw.ColumnIndexField);
            var newCell = _availableController.GetAvailableCell(playerPaw, false);
            await playerPaw.MovePaw(
                _levelModel.GameFields[newCell.RowIndex, newCell.ColumnIndex].RectTransform.position,
                newCell.RowIndex,
                newCell.ColumnIndex);
        }

        private void FieldClick(int row, int column)
        {
            _cell = new Cell(row, column);
            _tcs.TrySetResult();
        }
    }
}