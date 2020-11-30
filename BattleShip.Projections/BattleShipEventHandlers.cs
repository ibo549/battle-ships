using System.Threading.Tasks;
using CQRSlite.Events;
using BattleShip.Domain.Events;

namespace BattleShip.Projections 
{

    public class BattleShipEventHandlers : 
    IEventHandler<BoardCreated>,
    IEventHandler<ShipAdded>,
    IEventHandler<ShipHit>,
    IEventHandler<ShotMissed>,
    IEventHandler<ShipSunk>,
    IEventHandler<AllShipsSunk>,
    IEventHandler<BattleShipOperationFailed>
    {
        private readonly IGameDisplay _gameDisplay;
        private readonly IProjectionStore _store;
        public BattleShipEventHandlers(IGameDisplay gameDisplay, 
            IProjectionStore store)
        {
            _gameDisplay = gameDisplay;
            _store = store;
        }
        public Task Handle(BoardCreated message)
        {   
            _store.SetBoardSize(message.BoardSize);
            _gameDisplay.DisplayBoardCreated(message.BoardSize);
            _gameDisplay.DisplayBoard(_store.ShotsTaken, _store.BoardSize);         
            return Task.CompletedTask;
        }

        public Task Handle(ShipAdded message)
        {
            var direction = message.Head.X == message.Tail.X ? "vertical" : "horizontal";
           
            _gameDisplay.DisplayShipAdded(message.Name, message.Size, 
            $"{direction} - head: {message.Head.ToLetterCoordinate()}" );
            return Task.CompletedTask;
        }

        public Task Handle(ShipHit message)
        {
            _gameDisplay.DisplayHit(message.ShipName);
            _store.AddHit(message.Target.X, message.Target.Y);
            _gameDisplay.DisplayBoard(_store.ShotsTaken, _store.BoardSize);
            return Task.CompletedTask;
        }


        public Task Handle(ShotMissed message)
        {   
            _gameDisplay.DisplayMissed();
            _store.AddMiss(message.Target.X, message.Target.Y);
            _gameDisplay.DisplayBoard(_store.ShotsTaken, _store.BoardSize);
            return Task.CompletedTask;
        }

        public Task Handle(ShipSunk message)
        {
            _gameDisplay.DisplayShipSunk(message.ShipName);
            return Task.CompletedTask;
        }

        public Task Handle(AllShipsSunk message)
        {
            _gameDisplay.DisplayWon();
            return Task.CompletedTask;
        }

        public Task Handle(BattleShipOperationFailed message)
        {        
            _gameDisplay.DisplayOperationFailure(message.Reason);
            return Task.CompletedTask;
        }
    }


}