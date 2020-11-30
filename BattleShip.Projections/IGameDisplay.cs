using System.Collections.Generic;

namespace BattleShip.Projections 
{
    public interface IGameDisplay
    {
        void DisplayBoardCreated(int dimension);
        void DisplayShipAdded(string shipName, int size, string hint = null);
        void DisplayMissed();
        void DisplayHit(string shipName);
        void DisplayWon(string playerName = null);
        void DisplayShipSunk(string shipName);
        void DisplayOperationFailure(string reason);
        void DisplayBoard(IReadOnlyDictionary<(int x, int y), bool> shotsTaken, int size);
    }
}