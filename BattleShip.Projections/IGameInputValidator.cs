namespace BattleShip.Projections
{
    public interface IGameInputValidator 
    {
        bool IsCoordinateValid(string input);
        bool IsBoardSizeValid(int boardSize);
    }
}