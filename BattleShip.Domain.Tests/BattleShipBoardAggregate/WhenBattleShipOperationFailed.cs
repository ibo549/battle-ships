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
    public class Fails_WhenAddingNewShipToPopulatedBoard : Specification<BattleShipBoard, BattleShipCommandHandlers, AddBattleShip>
    {
        private Guid _boardId;
        protected override BattleShipCommandHandlers BuildHandler()
        {
            return new BattleShipCommandHandlers(Session);
        }

        protected override IEnumerable<IEvent> Given()
        {
            _boardId = Guid.NewGuid();
            //Only size of 4 available on board
            return new List<IEvent>
            {
                new BoardCreated(_boardId, 5) {Version = 1},
                new ShipAreaAllocated(_boardId, 0, 0, 5, new BoardPoint(0,0), new BoardPoint(0,4)) {Version = 2},
                new ShipAdded(_boardId, 0, 0, "Battle Ship", 5, new BoardPoint(0,0), new BoardPoint(0,4)) {Version =3},
                new ShipAreaAllocated(_boardId, 1, -1, 4, new BoardPoint(1,0), new BoardPoint(1,3)) {Version = 4},
                new ShipAdded(_boardId, 1, -1, "Destroyer", 4, new BoardPoint(1,0), new BoardPoint(1,3)) {Version = 5},
                new ShipAreaAllocated(_boardId, 2, -2, 4, new BoardPoint(2,0), new BoardPoint(2,3)) {Version = 6},
                new ShipAdded(_boardId, 2, -2, "Destroyer", 4, new BoardPoint(2,0), new BoardPoint(2,3)) {Version = 7},
                new ShipAreaAllocated(_boardId, 3, -3, 4, new BoardPoint(3,0), new BoardPoint(3,3)) {Version = 8},
                new ShipAdded(_boardId, 3, -3, "Destroyer", 4, new BoardPoint(3,0), new BoardPoint(3,3)) {Version = 9},
                new ShipAreaAllocated(_boardId, 4, -4, 4, new BoardPoint(4,0), new BoardPoint(4,3)) {Version = 10},
                new ShipAdded(_boardId, 4, -4, "Destroyer", 4, new BoardPoint(4,0), new BoardPoint(4,3)) {Version = 11},
            };
        }

        protected override AddBattleShip When()
        {
            return new AddBattleShip(_boardId);
        }

        [Then]
        public void Should_create_one_event()
        {
            Assert.Equal(1, PublishedEvents.Count);
        }

        [Then]
        public void Should_create_correct_event()
        {
            Assert.IsType<BattleShipOperationFailed>(PublishedEvents[0]);
        }

    }

    public class Fails_WhenShootingOutOfBoardRange : Specification<BattleShipBoard, BattleShipCommandHandlers, ShootTarget>
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
                new ShipAreaAllocated(_boardId, 0, 0, 5, new BoardPoint(0,0), new BoardPoint(0,4)) {Version = 2},
                new ShipAdded(_boardId, 0, 0, "Battle Ship", 5, new BoardPoint(0,0), new BoardPoint(0,4)) {Version =3}
            };
        }

        protected override ShootTarget When()
        {
            return new ShootTarget(_boardId, new BoardPoint(11, 22));
        }

        [Then]
        public void Should_create_one_event()
        {
            Assert.Equal(1, PublishedEvents.Count);
        }

        [Then]
        public void Should_create_correct_event()
        {
            Assert.IsType<BattleShipOperationFailed>(PublishedEvents[0]);
        }

    }
    public class Fails_WhenShootingAlreadyShotLocation : Specification<BattleShipBoard, BattleShipCommandHandlers, ShootTarget>
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
                new ShipAreaAllocated(_boardId, 0, 0, 5, new BoardPoint(0,0), new BoardPoint(0,4)) {Version = 2},
                new ShipAdded(_boardId, 0, 0, "Battle Ship", 5, new BoardPoint(0,0), new BoardPoint(0,4)) {Version =3},
                new ShipHit(_boardId, new BoardPoint(0,3), "Battle Ship") {Version = 4}
            };
        }

        protected override ShootTarget When()
        {
            return new ShootTarget(_boardId, new BoardPoint(0, 3));
        }

        [Then]
        public void Should_create_one_event()
        {
            Assert.Equal(1, PublishedEvents.Count);
        }

        [Then]
        public void Should_create_correct_event()
        {
            Assert.IsType<BattleShipOperationFailed>(PublishedEvents[0]);
        }

    }

}