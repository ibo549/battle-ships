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
    public class WhenShipAdded_BattleShip : Specification<BattleShipBoard, BattleShipCommandHandlers, AddBattleShip>
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
                new BoardCreated(_boardId, 10) {Version = 1}
            };
        }

        protected override AddBattleShip When()
        {
            return new AddBattleShip(_boardId);
        }

        [Then]
        public void Should_create_two_events()
        {
            Assert.Equal(2, PublishedEvents.Count);
        }

        [Then]
        public void Should_create_correct_events()
        {
            Assert.IsType<ShipAreaAllocated>(PublishedEvents[0]);
            Assert.IsType<ShipAdded>(PublishedEvents[1]);
        }

        [Then]
        public void Should_allocate_size_of_five_for_battleship()
        {
            Assert.Equal(5, ((ShipAreaAllocated)PublishedEvents[0]).AllocationSize);
            Assert.Equal("Battle Ship", ((ShipAdded)PublishedEvents[1]).Name);
            Assert.Equal(5, ((ShipAdded)PublishedEvents[1]).Size);
        }
    }
    public class WhenShipAdded_Destroyer : Specification<BattleShipBoard, BattleShipCommandHandlers, AddDestroyer>
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
                new BoardCreated(_boardId, 10) {Version = 1}
            };
        }

        protected override AddDestroyer When()
        {
            return new AddDestroyer(_boardId);
        }

        [Then]
        public void Should_create_two_events()
        {
            Assert.Equal(2, PublishedEvents.Count);
        }

        [Then]
        public void Should_create_correct_events()
        {
            Assert.IsType<ShipAreaAllocated>(PublishedEvents[0]);
            Assert.IsType<ShipAdded>(PublishedEvents[1]);
        }

        [Then]
        public void Should_allocate_size_of_four_for_destroyer()
        {
            Assert.Equal(4, ((ShipAreaAllocated)PublishedEvents[0]).AllocationSize);
            Assert.Equal("Destroyer", ((ShipAdded)PublishedEvents[1]).Name);
            Assert.Equal(4, ((ShipAdded)PublishedEvents[1]).Size);
        }
    }

}