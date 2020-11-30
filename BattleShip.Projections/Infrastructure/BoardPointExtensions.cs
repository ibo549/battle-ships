using BattleShip.Domain;

namespace BattleShip.Projections 
{
    public static class BoardPointExtensions
    {
        public static string ToLetterCoordinate(this BoardPoint point)
        {
            return point.X.ToLetterPart() + (point.Y + 1);
        }

        public static string ToLetterPart(this int zeroBasedColumnIndex)
        {
            return char.ConvertFromUtf32(zeroBasedColumnIndex + 65);
        }
    }

}