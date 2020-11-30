using System.Collections.Generic;

namespace BattleShip.Projections 
{
    public class InMemoryProjection : IProjectionStore
    {
        private static Dictionary<(int x, int y), bool> _shotsTaken = new Dictionary<(int x, int y), bool>();
        private static int _boardSize;
        public IReadOnlyDictionary<(int x, int y), bool> ShotsTaken => _shotsTaken;

        public int BoardSize => _boardSize;

        public void AddHit(int x, int y)
        {
            //Idempotent
            _shotsTaken.TryAdd((x,y), true);
        }

        public void AddMiss(int x, int y)
        {
             _shotsTaken.TryAdd((x,y), false);
        }

        public void SetBoardSize(int dimension)
        {
            _boardSize = dimension;
        }
    }
}