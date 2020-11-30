using System;
using CQRSlite.Commands;

namespace BattleShip.Domain.Commands
{
    public class AddBattleShip : ICommand 
	{
        public AddBattleShip(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; private set; }	    
      
	}
}