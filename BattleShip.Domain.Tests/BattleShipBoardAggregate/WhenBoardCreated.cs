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
    public class WhenBoardCreated : Specification<BattleShipBoard, BattleShipCommandHandlers, CreateBoard>
    {
        private Guid _id;
        protected override BattleShipCommandHandlers BuildHandler()
        {
            return new BattleShipCommandHandlers(Session);
        }

        protected override IEnumerable<IEvent> Given()
        {
            _id = Guid.NewGuid();
            return new List<IEvent>();
        }

        protected override CreateBoard When()
        {
            return new CreateBoard(10);
        }

        [Then]
        public void Should_create_one_event()
        {
            Assert.Equal(1, PublishedEvents.Count);
        }

        [Then]
        public void Should_create_correct_event()
        {
            Assert.IsType<BoardCreated>(PublishedEvents.First());
        }

        [Then]
        public void Should_save_boardsize()
        {
            Assert.Equal(10, ((BoardCreated)PublishedEvents.First()).BoardSize);
        }
    }
}