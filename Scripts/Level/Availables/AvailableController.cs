using System;
using System.Collections.Generic;
using System.Linq;
using FruityPaw.Scripts.Level.Paws;

namespace FruityPaw.Scripts.Level.Availables
{
    public class AvailableController
    {
        private readonly LevelModel _levelModel;
        
        private int[] _deltaRow = { -1, 0, 0 };
        private int[] _deltaColumn = { 0, 1, -1 };
        private int[] _blowColumnDelta = { 0, -1, 1 };

        private List<AvailableItem> _availableItems = new();

        public AvailableController(LevelModel levelModel)
        {
            _levelModel = levelModel;
        }

        #region player

        public Paw[] GetAvailablePaws(bool isAll = false)
        {
            CheckAvailablePaws(isAll);
            return _availableItems
                .Where(x => x != null && x.Paw.RowIndexField != 0 && x.AvailableGameFields.Length > 0)
                .Select(x => x.Paw).ToArray();
        }

        public Cell[] GetAvailableFields()
        {
            return _availableItems.First(x => x.Paw == _levelModel.CurrentPaw)
                .AvailableGameFields.Select(x=>x.GameField).ToArray();
        }

        public Cell[] GetStepsFields(int rowIndex, int columnIndex)
        {
            return _availableItems.First(x => x.Paw == _levelModel.CurrentPaw)
                .AvailableGameFields.First(x =>
                    x.GameField.RowIndex == rowIndex && x.GameField.ColumnIndex == columnIndex)
                .Steps;
        }

        public Cell GetAvailableCell(Paw paw, bool isPlayerForward)
        {
            Cell cell = null;
            var rowDelta = 1;
            while (cell == null)
            {
                foreach (var blowDelta in _blowColumnDelta)
                {
                    var newRow = isPlayerForward ? paw.RowIndexField - rowDelta : paw.RowIndexField + rowDelta;
                    var newColumn = paw.ColumnIndexField + blowDelta;
                    if (_levelModel.IsFreeField(newRow, newColumn))
                    {
                        cell = new Cell(newRow, newColumn);
                        break;
                    }
                }

                rowDelta++;
            }
            
            return cell;
        }

        public FieldStep[] GetAvailableCells(TypePaw typePaw, int rowIndex, int columnIndex)
        {
            var fieldSteps = typePaw switch
            {
                TypePaw.Small => GetAvailableItemsBySmall(rowIndex, columnIndex),
                TypePaw.Normal => GetAvailableItemsByNormal(rowIndex, columnIndex),
                TypePaw.Big => GetAvailableItemsByBig(rowIndex, columnIndex),
                _ => throw new ArgumentOutOfRangeException()
            };

            return fieldSteps.Length == 0
                ? null
                : fieldSteps.ToArray();
        }

        private void CheckAvailablePaws(bool isAll = false)
        {
            _availableItems.Clear();
            if (isAll)
            {
                var paws = _levelModel.PlayerPaws.Where(x => x.RowIndexField != 0);
                foreach (var playerPaw in paws)
                {
                    var availableGameFields = playerPaw.TypePaw switch
                    {
                        TypePaw.Small => GetAvailableItemsBySmall(playerPaw.RowIndexField,
                            playerPaw.ColumnIndexField),
                        TypePaw.Normal => GetAvailableItemsByNormal(playerPaw.RowIndexField,
                            playerPaw.ColumnIndexField),
                        TypePaw.Big => GetAvailableItemsByBig(playerPaw.RowIndexField, playerPaw.ColumnIndexField),
                        _ => throw new ArgumentOutOfRangeException()
                    };
                    if (availableGameFields.Length != 0)
                        _availableItems.Add(
                            new AvailableItem { Paw = playerPaw, AvailableGameFields = availableGameFields });
                }
            }
            else
            {
                var maxIndex = _levelModel.PlayerPaws.Select(paw => paw.RowIndexField).Prepend(0).Max();
                while (_availableItems.Count == 0)
                {
                    var paws = _levelModel.PlayerPaws.Where(x => x.RowIndexField == maxIndex);
                    foreach (var playerPaw in paws)
                    {
                        var availableGameFields = playerPaw.TypePaw switch
                        {
                            TypePaw.Small => GetAvailableItemsBySmall(playerPaw.RowIndexField,
                                playerPaw.ColumnIndexField),
                            TypePaw.Normal => GetAvailableItemsByNormal(playerPaw.RowIndexField,
                                playerPaw.ColumnIndexField),
                            TypePaw.Big => GetAvailableItemsByBig(playerPaw.RowIndexField, playerPaw.ColumnIndexField),
                            _ => throw new ArgumentOutOfRangeException()
                        };
                        if (availableGameFields.Length != 0)
                            _availableItems.Add(
                                new AvailableItem { Paw = playerPaw, AvailableGameFields = availableGameFields });
                    }

                    if (_availableItems.Count == 0)
                    {
                        maxIndex--;
                    }
                }
            }
        }
        
        #endregion
        
        #region bot

        /// <summary>
        /// Only for Bot's
        /// </summary>
        /// <returns></returns>
        public AvailableItem GetAvailableItem()
        {
            var availableItems = new List<AvailableItem>();
            var maxIndex = _levelModel.BotPaws.Select(paw => paw.RowIndexField).Min();
            while (availableItems.Count == 0)
            {
                var paws = _levelModel.BotPaws.Where(x=>x.RowIndexField == maxIndex && !x.OnFreeze).ToArray();
                if(paws.Length != 0)
                    foreach (var botPaw in paws)
                    {
                        if(botPaw.RowIndexField == 7) continue;
                    
                        var availableGameFields = botPaw.TypePaw switch
                        {
                            TypePaw.Small => GetAvailableItemsBySmall(botPaw.RowIndexField, botPaw.ColumnIndexField, true),
                            TypePaw.Normal => GetAvailableItemsByNormal(botPaw.RowIndexField, botPaw.ColumnIndexField, true),
                            TypePaw.Big => GetAvailableItemsByBig(botPaw.RowIndexField, botPaw.ColumnIndexField, true),
                            _ => throw new ArgumentOutOfRangeException()
                        };
                        if (availableGameFields.Length != 0)
                            availableItems.Add(
                                new AvailableItem { Paw = botPaw, AvailableGameFields = availableGameFields });
                    }

                if (availableItems.Count == 0)
                    maxIndex++;
            }
            
            var rndIndex = UnityEngine.Random.Range(0, availableItems.Count);
            return availableItems[rndIndex];
        }

        public FieldStep GetAvailableCellForBot(TypePaw typePaw, int rowIndex, int columnIndex)
        {
            var fieldSteps = typePaw switch
            {
                TypePaw.Small => GetAvailableItemsBySmall(rowIndex, columnIndex, true),
                TypePaw.Normal => GetAvailableItemsByNormal(rowIndex, columnIndex, true),
                TypePaw.Big => GetAvailableItemsByBig(rowIndex, columnIndex, true),
                _ => throw new ArgumentOutOfRangeException()
            };

            return fieldSteps.Length == 0 
                ? null 
                : fieldSteps[UnityEngine.Random.Range(0, fieldSteps.Length)];
        }
        
        #endregion

        private FieldStep[] GetAvailableItemsBySmall(int pawRowIndex, int pawColumnIndex, bool isBot = false)
        {
            var fieldSteps = new List<FieldStep>();
            for (var dir = 0; dir < 3; dir++)
            {
                var newRow = isBot
                    ? pawRowIndex - _deltaRow[dir]
                    : pawRowIndex + _deltaRow[dir];
                var newColumn = pawColumnIndex + _deltaColumn[dir];
                var startPawIndex = isBot ? 0 : 7;
                if (newRow != startPawIndex && _levelModel.IsFreeField(newRow, newColumn, true, isBot))
                {
                    fieldSteps.Add(new FieldStep
                    {
                        GameField = new Cell(newRow, newColumn),
                        Steps = new []{new Cell(newRow, newColumn)}
                    });
                }
            }

            return fieldSteps.ToArray();
        }

        private FieldStep[] GetAvailableItemsByNormal(int pawRowIndex, int pawColumnIndex, bool isBot = false)
        {
            var fieldSteps = new List<FieldStep>();
            for (var dir = 0; dir < 3; dir++)
            {
                var newFirstX = isBot
                    ? pawRowIndex - _deltaRow[dir]
                    : pawRowIndex + _deltaRow[dir];
                var newFirstY = pawColumnIndex + _deltaColumn[dir];
                if (newFirstX != 0 && newFirstX != 7 && _levelModel.IsFreeField(newFirstX, newFirstY, false, isBot))
                {
                    foreach (var newDir in GetDirArray(dir))
                    {
                        var newSecondX = isBot
                            ? newFirstX - _deltaRow[newDir]
                            : newFirstX + _deltaRow[newDir];
                        var newSecondY = newFirstY + _deltaColumn[newDir];
                        if (_levelModel.IsFreeField(newSecondX, newSecondY, true, isBot))
                        {
                            if (!fieldSteps.Any(x => x.GameField.RowIndex == newSecondX
                                                     && x.GameField.ColumnIndex == newSecondY))
                                fieldSteps.Add(new FieldStep
                                {
                                    GameField = new Cell(newSecondX, newSecondY),
                                    Steps = new[]
                                    {
                                        new Cell(newFirstX, newFirstY),
                                        new Cell(newSecondX, newSecondY)
                                    }
                                });
                        }
                    }
                }
            }

            return fieldSteps.ToArray();
        }

        private FieldStep[] GetAvailableItemsByBig(int pawRowIndex, int pawColumnIndex, bool isBot = false)
        {
            var fieldSteps = new List<FieldStep>();
            for (var dir = 0; dir < 3; dir++)
            {
                var newFirstX = isBot
                    ? pawRowIndex - _deltaRow[dir]
                    : pawRowIndex + _deltaRow[dir];
                var newFirstY = pawColumnIndex + _deltaColumn[dir];
                if (newFirstX != 0 && newFirstX != 7 && _levelModel.IsFreeField(newFirstX, newFirstY, false, isBot))
                {
                    foreach (var newDir in GetDirArray(dir))
                    {
                        var newSecondX = isBot
                            ? newFirstX - _deltaRow[newDir]
                            : newFirstX + _deltaRow[newDir];
                        var newSecondY = newFirstY + _deltaColumn[newDir];
                        if (newSecondX != 0 && newSecondX !=7 && _levelModel.IsFreeField(newSecondX, newSecondY, false, isBot))
                        {
                            foreach (var secondStepDir in GetDirArray(newDir))
                            {
                                var newThirdX = isBot
                                    ? newSecondX - _deltaRow[secondStepDir]
                                    : newSecondX + _deltaRow[secondStepDir];
                                var newThirdY = newSecondY + _deltaColumn[secondStepDir];
                                if (_levelModel.IsFreeField(newThirdX, newThirdY, true, isBot))
                                    if (!fieldSteps.Any(x => x.GameField.RowIndex == newThirdX
                                                             && x.GameField.ColumnIndex == newThirdY))
                                        fieldSteps.Add(new FieldStep
                                        {
                                            GameField = new Cell(newThirdX, newThirdY),
                                            Steps = new[]
                                            {
                                                new Cell(newFirstX, newFirstY),
                                                new Cell(newSecondX, newSecondY),
                                                new Cell(newThirdX, newThirdY)
                                            }
                                        });
                            }
                        }
                    }
                }
            }
            
            return fieldSteps.ToArray();
        }
        
        private int[] GetDirArray(int currentIndex)
        {
            var array = new List<int>{ 0, 1, 2 };
            switch (currentIndex)
            {
                case 2 : array.Remove(1);
                    break;
                case 1: array.Remove(2);
                    break;
            }
            
            return array.ToArray();
        }
    }
}