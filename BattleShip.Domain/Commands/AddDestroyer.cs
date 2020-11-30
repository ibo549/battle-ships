using System;
using CQRSlite.Commands;

namespace BattleShip.Domain.Commands
{
    public class AddDestroyer : ICommand 
	{
        public AddDestroyer(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; private set; }	    
      
	}
}