using System;
using CQRSlite.Events;

namespace BattleShip.Domain.Events
{
    public class ShipAdded : IEvent 
	{
        public readonly int ShipId;
        public readonly string Name;
        public readonly int Size;
        public readonly BoardPoint Head;
        public readonly BoardPoint Tail;

        public readonly int AllocationReference;
        public ShipAdded(Guid id, int shipId, int allocationRef, string name, int size, 
        BoardPoint head,
        BoardPoint tail) 
        {
            Id = id;
            ShipId = shipId;
            AllocationReference = allocationRef;
            Name = name;
            Size = size;
            Head = head;
            Tail = tail;
        }

        public Guid Id { get; set; }
        public int Version { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
	}
}