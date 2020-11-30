using System.Threading.Tasks;
using CQRSlite.Commands;
using CQRSlite.Domain;
using BattleShip.Domain.Commands;

namespace BattleShip.Domain.CommandHandlers 
{
    public class BattleShipCommandHandlers : 
    ICommandHandler<CreateBoard>,
    ICommandHandler<AddBattleShip>,
    ICommandHandler<AddDestroyer>,
    ICommandHandler<ShootTarget>
    {
        private readonly ISession _session;

        public BattleShipCommandHandlers(ISession session)
        {
            _session = session;
        }

        public async Task Handle(CreateBoard message)
        {
            var board = new BattleShipBoard(message.Id, message.BoardSize);
            await _session.Add(board);
            await _session.Commit();
        }

        public async Task Handle(AddBattleShip message)
        {
            var board = await _session.Get<BattleShipBoard>(message.Id);
            board.AddBattleShipRandomly();
            await _session.Commit();
        }

        public async Task Handle(AddDestroyer message)
        {
            var board = await _session.Get<BattleShipBoard>(message.Id);
            board.AddDestroyerRandomly();
            await _session.Commit();
        }

        public async Task Handle(ShootTarget message)
        {
            var board = await _session.Get<BattleShipBoard>(message.Id);
            board.Shoot(message.Target);
            await _session.Commit();
        }
    }
}