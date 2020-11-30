using System.Collections.Generic;

namespace BattleShip.Projections 
{ 
    public interface IProjectionStore
    {
        IReadOnlyDictionary<(int x, int y), bool> ShotsTaken { get; }
        int BoardSize { get; }
        void AddHit(int x, int y);
        void AddMiss(int x, int y);
        void SetBoardSize(int dimension);
    }
}