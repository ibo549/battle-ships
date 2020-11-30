using System;
using CQRSlite.Commands;

namespace BattleShip.Domain.Commands
{
    public class CreateBoard : ICommand 
	{
        public CreateBoard(int boardSize)
        {
            Id = Guid.NewGuid();
            BoardSize = boardSize;
        }
        public Guid Id { get; private set; }
        public readonly int BoardSize;	    
      
	}
}