using System;
using CQRSlite.Events;

namespace BattleShip.Domain.Events
{
    public class ShotMissed : IEvent 
	{
        public readonly BoardPoint Target;
        public ShotMissed(Guid id, BoardPoint target) 
        {
            Id = id;
            Target = target;
        }

        public Guid Id { get; set; }
        public int Version { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
	}
}