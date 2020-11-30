using System;
using System.Collections.Generic;
using System.Linq;
using BattleShip.Domain;
using BattleShip.Domain.Commands;
using BattleShip.Domain.CommandHandlers;
using BattleShip.Domain.Events;
using CQRSlite.Events;
using BattleShip.Domain.Tests.TestFixtures;
using Xunit;

namespace BattleShip.Domain.Tests
{
    public class WhenShotMissed : Specification<BattleShipBoard, BattleShipCommandHandlers, ShootTarget>
    {
        private Guid _boardId;
        protected override BattleShipCommandHandlers BuildHandler()
        {
            return new BattleShipCommandHandlers(Session);
        }

        protected override IEnumerable<IEvent> Given()
        {
            _boardId = Guid.NewGuid();
            return new List<IEvent>
            {
                new BoardCreated(_boardId, 10) {Version = 1},
                new ShipAreaAllocated(_boardId, 0, 0, 5, new BoardPoint(0,0), new BoardPoint(0,5)) {Version = 2},
                new ShipAdded(_boardId, 0, 0, "Battle Ship", 5, new BoardPoint(0,0), new BoardPoint(0,5)) {Version =3}
            };
        }

        protected override ShootTarget When()
        {
            return new ShootTarget(_boardId, new BoardPoint(7,9));
        }

        [Then]
        public void Should_create_one_event()
        {
            Assert.Equal(1, PublishedEvents.Count);
        }

        [Then]
        public void Should_create_correct_event()
        {
            Assert.IsType<ShotMissed>(PublishedEvents.First());
        }

       
        [Then]
        public void Should_save_missed_coordinate()
        {
            Assert.Equal(new BoardPoint(7,9), ((ShotMissed)PublishedEvents.First()).Target);
        }
    }
   
}