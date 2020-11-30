using System;
using CQRSlite.Events;

namespace BattleShip.Domain.Events
{
    public class AllShipsSunk : IEvent 
	{
        public AllShipsSunk(Guid id) 
        {
            Id = id;
        }

        public Guid Id { get; set; }
        public int Version { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
	}
}