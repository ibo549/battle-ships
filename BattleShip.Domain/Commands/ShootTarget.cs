using System;
using CQRSlite.Commands;

namespace BattleShip.Domain.Commands
{
    public class ShootTarget : ICommand 
	{
        public BoardPoint Target { get; }
        public ShootTarget(Guid id, BoardPoint target)
        {
            Id = id;
            Target = target;
        }
        public Guid Id { get; private set; }	    
      
	}
}