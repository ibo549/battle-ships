using System;
using CQRSlite.Events;

namespace BattleShip.Domain.Events
{
    public class ShipSunk : IEvent 
	{
        public readonly string ShipName;
        public ShipSunk(Guid id, string shipName) 
        {
            Id = id;
            ShipName = shipName;
        }

        public Guid Id { get; set; }
        public int Version { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
	}
}