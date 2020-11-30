using System;
using CQRSlite.Events;

namespace BattleShip.Domain.Events
{
    public class BattleShipOperationFailed : IEvent 
	{
        public readonly string Reason;
        public BattleShipOperationFailed(Guid id, string reason) 
        {
            Id = id;
            Reason = reason;
        }

        public Guid Id { get; set; }
        public int Version { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
	}
}