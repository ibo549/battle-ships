using System;
using CQRSlite.Events;

namespace BattleShip.Domain.Events
{
    public class ShipHit : IEvent 
	{
        public readonly BoardPoint Target;
        public readonly string ShipName;
        public ShipHit(Guid id, BoardPoint target, string shipName) 
        {
            Id = id;
            Target = target;
            ShipName = shipName;
        }

        public Guid Id { get; set; }
        public int Version { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
	}
}