using System;
using System.Collections.Generic;
using CQRSlite.Domain;
using BattleShip.Domain.Events;
using System.Linq;

namespace BattleShip.Domain
{
    public class BattleShipBoard : AggregateRoot
    {
        private const int _battleShipSize = 5;
        private const int _destroyerSize = 4;
        private Board _board;
        private IDictionary<int, Ship> _ships = new Dictionary<int, Ship>();
        private IList<BoardPoint> _shotsTaken = new List<BoardPoint>();
        private static readonly Object _searchForShipLock = new Object();
        private BattleShipBoard() { }
        public BattleShipBoard(Guid id, int boardSize)
        {
            Id = id;
            ApplyChange(new BoardCreated(id, boardSize));
        }
        public void Shoot(BoardPoint target)
        {
            //Validate target
            if (!_board.IsPointInRange(target))
            {
                ApplyChange(new BattleShipOperationFailed(Id, "Target is out of the Board Range"));
                return;
            }

            if (_shotsTaken.Any(s => s.Equals(target)))
            {
                ApplyChange(new BattleShipOperationFailed(Id, "Target already Shot"));
                return;
            }

            if (IsHit(target))
            {
                var shipId = _board.GetObjectIdOnBoard(target.X, target.Y);
                var ship = this._ships[shipId.Value];

                ApplyChange(new ShipHit(Id, target, ship.Name));

                if (ship.IsSunk())
                {
                    ApplyChange(new ShipSunk(Id, ship.Name));
                    if (HasPlayerWon()) ApplyChange(new AllShipsSunk(Id));
                }

            }
            else
            {
                ApplyChange(new ShotMissed(Id, target));
            }
        }
        public void AddBattleShipRandomly()
        {
            this.AddShipRandomly("Battle Ship", _battleShipSize);
        }
        public void AddDestroyerRandomly()
        {
            this.AddShipRandomly("Destroyer", _destroyerSize);
        }

        private void AddShipRandomly(string name, int size)
        {
            lock (_searchForShipLock)
            {
                var shipId = this.GetNextShipId();
                var availableArea = _board.AllocateRandomAvailableArea(size, shipId);
                if (!availableArea.HasValue)
                {
                    ApplyChange(new BattleShipOperationFailed(Id, $"Board is overcrowded to add a {name}"));
                    return;
                }
                var area = availableArea.Value;

                ApplyChange(new ShipAreaAllocated(Id, shipId, area.allocationId, size, area.head, area.tail));
                ApplyChange(new ShipAdded(Id, shipId, area.allocationId, name, size,
                    area.head, area.tail));
            }

        }
        private bool IsHit(BoardPoint target)
        {
            return _board.IsOccupied(target);
        }
        private int GetNextShipId()
        {
            return _ships.Count;
        }
        private bool HasPlayerWon()
        {
            if (_ships.All(s => s.Value.IsSunk() == true))
                return true;

            return false;
        }

        #region PlayEvents
        private void Apply(BoardCreated e)
        {
            _board = new Board(e.BoardSize);           
        }
        private void Apply(ShipAdded e)
        {
            _board.FullFillAllocation(e.ShipId, e.AllocationReference, e.Size);
            _ships.Add(e.ShipId, new Ship(e.ShipId, e.Name, e.Size));
        }
        private void Apply(ShipAreaAllocated e)
        {
           //Check if reservation exists
           if(_board.GetObjectIdOnBoard(e.Head.X, e.Head.Y) != e.AllocationId)
           {
               //Restore the allocation
               _board.AllocateGivenArea(e.Head, e.Tail, e.AllocationId);
           }          
        }
        private void Apply(ShipHit e)
        {
            this._shotsTaken.Add(e.Target);
            var shipId = _board.GetObjectIdOnBoard(e.Target.X, e.Target.Y);
            _ships[shipId.Value].Shoot();
        }
        private void Apply(ShotMissed e)
        {
            this._shotsTaken.Add(e.Target);
        }
        #endregion
    }
}