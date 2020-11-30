namespace BattleShip.Domain
{
    public class Ship 
    {
        public int Id { get; }
        public string Name { get; }
        public int Size { get; }
        public int HitsTaken { get; private set; }
        public Ship(int id, string name, int size)
        {
            Id = id;
            Name = name;
            Size = size;
            HitsTaken = 0;
        }

        public bool IsSunk()
        {
            return Size == HitsTaken;
        }
        public void Shoot()
        {
            if(!IsSunk())          
                HitsTaken++;
        }
    } 

}

