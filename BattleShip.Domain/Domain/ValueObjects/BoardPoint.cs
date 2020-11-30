namespace BattleShip.Domain
{ 
    public struct BoardPoint
    {
        public int X { get; }
        public int Y { get; }
        public BoardPoint(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

}

