using System.Collections.Generic;

namespace BattleShip.Domain
{
    public static class MatrixExtensions
    {
        public static IEnumerable<(int x, int y)> SliceRow<T>(this T[,] array, int row, int colStart, int colEnd)
        {
            //TODO Extract flipping to a common private method
            bool forward = colEnd > colStart ? true : false;
            if (!forward)
            {
                //flip for iterator
                int col_s = colStart;
                colStart = colEnd;
                colEnd = col_s;
            }

            for (var i = colStart; i <= colEnd; i++)
            {
                yield return (i, row);
            }
        }
        public static IEnumerable<(int x, int y)> SliceColumn<T>(this T[,] array, int column, int rowStart, int rowEnd)
        {
            bool forward = rowEnd > rowStart ? true : false;
            if (!forward)
            {
                //flip for iterator
                int row_s = rowStart; 
                rowStart = rowEnd; 
                rowEnd = row_s;
            }

            for (var i = rowStart; i <= rowEnd; i++)
            {
                yield return (column, i);
            }
        }
    }
}