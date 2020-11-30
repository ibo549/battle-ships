using System;
using CQRSlite.Events;

namespace BattleShip.Domain.Events
{
    public class ShipAreaAllocated : IEvent 
	{
        public readonly int ShipId;
        public readonly int AllocationId;
        public readonly int AllocationSize;
        public readonly BoardPoint Head;
        public readonly BoardPoint Tail;
        public ShipAreaAllocated(Guid id, int shipId, int allocationId, int allocationSize,
        BoardPoint head, BoardPoint tail) 
        {
            Id = id;
            ShipId = shipId;
            AllocationId = allocationId;
            AllocationSize = allocationSize;
            Head = head;
            Tail = tail;
        }

        public Guid Id { get; set; }
        public int Version { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
	}
}