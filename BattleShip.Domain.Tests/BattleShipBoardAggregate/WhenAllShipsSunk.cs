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
    public class WhenAllShipsSunk : Specification<BattleShipBoard, BattleShipCommandHandlers, ShootTarget>
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
                new ShipAreaAllocated(_boardId, 1, -1, 4, new BoardPoint(2,2), new BoardPoint(2,5)) {Version = 4},
                new ShipAdded(_boardId, 1, -1, "Destroyer", 4, new BoardPoint(2,2), new BoardPoint(2,5)) {Version = 5},
                new ShipHit(_boardId, new BoardPoint(0,0), "Battle Ship") {Version = 6},
                new ShipHit(_boardId, new BoardPoint(0,1), "Battle Ship") {Version = 7},
                new ShipHit(_boardId, new BoardPoint(0,2), "Battle Ship") {Version = 8},
                new ShipHit(_boardId, new BoardPoint(0,3), "Battle Ship") {Version = 9},
                new ShipHit(_boardId, new BoardPoint(0,4), "Battle Ship") {Version = 10},
                new ShipSunk(_boardId, "Battle Ship") {Version = 11},
                new ShipHit(_boardId, new BoardPoint(2,2), "Destroyer") {Version = 12},
                new ShipHit(_boardId, new BoardPoint(2,3), "Destroyer") {Version = 13},
                new ShipHit(_boardId, new BoardPoint(2,4), "Destroyer") {Version = 14}
               
            };
        }

        protected override ShootTarget When()
        {
            return new ShootTarget(_boardId, new BoardPoint(2,5));
        }

        [Then]
        public void Should_create_two_event()
        {
            Assert.Equal(3, PublishedEvents.Count);
        }

        [Then]
        public void Should_create_correct_events()
        {
            Assert.IsType<ShipHit>(PublishedEvents[0]);
            Assert.IsType<ShipSunk>(PublishedEvents[1]);
            Assert.IsType<AllShipsSunk>(PublishedEvents[2]);
        }

       
        [Then]
        public void Should_save_hit_coordinate()
        {
            Assert.Equal(new BoardPoint(2,5), ((ShipHit)PublishedEvents[0]).Target);
        }

        [Then]
        public void Should_save_ship_name()
        {
            Assert.Equal("Destroyer", ((ShipSunk)PublishedEvents[1]).ShipName);
        }

    }
   
}