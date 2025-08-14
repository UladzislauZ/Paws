using System.Linq;
using FruityPaw.Scripts.Fruits;
using FruityPaw.Scripts.Level.Availables;
using FruityPaw.Scripts.Level.FieldObjects;
using FruityPaw.Scripts.Level.Paws;

namespace FruityPaw.Scripts.Level
{
    public class LevelModel
    {
        public Paw[] BotPaws;
        public Paw[] PlayerPaws;
        public GameField[,] GameFields;
        public bool OnEnd;
        public bool IsExtraMove;
        public Paw CurrentPaw;
        public Statistic BotStatistic = new ();
        public Statistic PlayerStatistic = new ();

        public void Clear()
        {
            CurrentPaw = null;
            OnEnd = false;
            BotStatistic.Clear();
            PlayerStatistic.Clear();
        }

        public bool IsFreeField(int row, int column)
        {
            if (row >= GameFields.GetLength(0) || row < 0 ||
                column >= GameFields.GetLength(1) || column < 0)
                return false;
            
            if (BotPaws.Any(x => x.RowIndexField == row && x.ColumnIndexField == column))
                return false;
            
            if (PlayerPaws.Any(x => x.RowIndexField == row && x.ColumnIndexField == column))
                return false;
            
            return true;
        }

        public bool IsFreeField(int row, int column, bool isEnd, bool isBot)
        {
            if (row >= GameFields.GetLength(0) || row < 0 ||
                column >= GameFields.GetLength(1) || column < 0)
                return false;

            var startIndex = isBot ? 0 : 7;
            var endIndex = isBot ? 7 : 0;
            if (row == startIndex) return false;
            
            if (!isEnd && row == endIndex) return false;
            
            if (isBot)
            {
                if (BotPaws.Any(x => x.RowIndexField == row && x.ColumnIndexField == column))
                    return false;

                if (PlayerPaws.Any(x => x.RowIndexField == row && x.ColumnIndexField == column)) 
                    return isEnd;
            }
            else
            {
                if (PlayerPaws.Any(x => x.RowIndexField == row && x.ColumnIndexField == column))
                    return false;

                if (BotPaws.Any(x => x.RowIndexField == row && x.ColumnIndexField == column)) 
                    return isEnd;
            }

            return true;
        }

        public GameField[] GetGameFieldsByCells(Cell[] cells)
        {
            return cells.Select(cell => GameFields[cell.RowIndex, cell.ColumnIndex]).ToArray();
        }
    }
}