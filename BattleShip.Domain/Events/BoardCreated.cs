using System;
using CQRSlite.Events;

namespace BattleShip.Domain.Events
{
    public class BoardCreated : IEvent 
	{
        public readonly int BoardSize;
        public BoardCreated(Guid id, int boardSize) 
        {
            Id = id;
            BoardSize = boardSize;
        }

        public Guid Id { get; set; }
        public int Version { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
	}
}