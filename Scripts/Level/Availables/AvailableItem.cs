using FruityPaw.Scripts.Level.Paws;

namespace FruityPaw.Scripts.Level.Availables
{
    public class AvailableItem
    {
        public Paw Paw;
        public FieldStep[] AvailableGameFields;
    }

    public class FieldStep
    {
        public Cell GameField;
        public Cell[] Steps;
    }

    public class Cell
    {
        public int RowIndex;
        public int ColumnIndex;

        public Cell(int rowIndex, int columnIndex)
        {
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
        }
    }
}