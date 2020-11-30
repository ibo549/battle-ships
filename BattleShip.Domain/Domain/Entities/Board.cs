using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleShip.Domain
{
    public class Board
    {
        private int _boardSize { get; }
        private int?[,] _board;
        private static readonly Object _randomAreaAllocationLock = new Object();
        public Board(int size)
        {
            _boardSize = size;
            _board = new int?[size, size];
        }

        public void FullFillAllocation(int objectIdToPlace, int allocationId, int allocationSize)
        {

            int fullFilledSize = 0;

            for (int x = 0; x < _boardSize; x++)
            {
                for (int y = 0; y < _boardSize; y++)
                {
                    if (_board[x, y] == allocationId)
                    {
                        _board[x, y] = objectIdToPlace;
                        fullFilledSize++;
                    }

                    if (fullFilledSize == allocationSize) return;
                }
            }
        }
        public int? GetObjectIdOnBoard(int x, int y)
        {
            return _board[x, y];
        }
        public bool IsPointInRange(BoardPoint point)
        {
            if (point.X < 0 || point.Y < 0) return false;
            if (point.X < _boardSize && point.Y < _boardSize) return true;

            return false;
        }
        public bool IsOccupied(BoardPoint point)
        {
            if (!IsPointInRange(point))
                throw new ArgumentException($"{nameof(point)} is not in range.");

            var objectId = _board[point.X, point.Y];

            if (objectId == null)
                return false;

            return true;
        }
        public (int allocationId, BoardPoint head, BoardPoint tail)? AllocateRandomAvailableArea(int size, int objectId)
        {
            var rand = new Random(DateTime.UtcNow.Millisecond);
            Dictionary<(int x, int y), bool> scannedSoFar = new Dictionary<(int x, int y), bool>();
            int maxSpotsOnBoard = (this._boardSize * this._boardSize);

            lock (_randomAreaAllocationLock)
            {
                //First available random area that can house as much as {size} either at X or Y axis, forward or backward
                //As Non-Deterministic as possible, starts the search from a random point, 
                //axis and back-forward search direction are determined at run-tim randomly.              
                while (scannedSoFar.Count != (maxSpotsOnBoard))
                {
                    int x = rand.Next(0, _boardSize);
                    int y = rand.Next(0, _boardSize);

                    if (IsPointAvailable(x, y) && !scannedSoFar.ContainsKey((x, y)))
                    {
                        //Prioritize vertical search?
                        bool colFirst = GetRandomBool(rand);
                        //Prioritize going forward first?
                        bool forwardDirectionFirst = GetRandomBool(rand);

                        var tail = FindRandomTail(x, y, size, colFirst, forwardDirectionFirst, 0);

                        if (tail.HasValue)//random area found
                        {
                            //Allocate the area
                            var allocationId = -(objectId);

                            var head = new BoardPoint(x, y);
                            AllocateGivenArea(head, tail.Value, allocationId);

                            //Return allocation Key, head and tail
                            return (allocationId, new BoardPoint(x, y), tail.Value);
                        }

                    }

                    scannedSoFar.TryAdd((x, y), true);
                }
            }

            return null; //No available place for allocation with given size
        }
        public void AllocateGivenArea(BoardPoint head, BoardPoint tail, int allocationId)
        {
            var allocationArea = this.Slice((head.X, head.Y), (tail.X, tail.Y));
            foreach (var c in allocationArea)
            {
                _board[c.x, c.y] = allocationId;
            }

        }
        
        private IEnumerable<(int x, int y)> Slice((int x, int y) head, (int x, int y) tail)
        {
            bool vertical = head.x == tail.x;
            return vertical ?
                          _board.SliceColumn(head.x, head.y, tail.y) :
                          _board.SliceRow(head.y, head.x, tail.x);
        }
        private BoardPoint? FindRandomTail(int x, int y, int size,
            bool verticalFirst, bool forwardDirectionFirst, int directionsSearched)
        {
            //Recursively search all directions from a given point
            int maxDirections = 4;
            directionsSearched++;

            if (directionsSearched > maxDirections) //exit condition
            {
                return null;
            }

            if (directionsSearched > 2)//one axis already scanned, flip the axis
            {
                verticalFirst = !verticalFirst;
            }

            int emptyConsecutiveSpots = 1; //starting spot is already empty

            for (int i = verticalFirst ? y : x; (i >= 0 && i < this._boardSize);)
            {
                i = forwardDirectionFirst ? (i + 1) : (i - 1);

                if ((i == this._boardSize) || i < 0)// beyond edge, flip scan direction
                    return FindRandomTail(x, y, size, verticalFirst, !forwardDirectionFirst, directionsSearched);

                var pointToCheck = verticalFirst ? _board[x, i] : _board[i, y] ;

                if (pointToCheck.HasValue)//occupied, flip scan direction
                {
                    return FindRandomTail(x, y, size, verticalFirst, !forwardDirectionFirst, directionsSearched);
                }

                emptyConsecutiveSpots++;
                bool areaFound = emptyConsecutiveSpots == size;

                if (areaFound)
                {
                    BoardPoint tailPoint = verticalFirst
                    ? new BoardPoint(x, i)
                    : new BoardPoint(i, y);
                    return tailPoint;
                }
            }

            //Reached edge, flip the direction and search again
            return FindRandomTail(x, y, size, verticalFirst, !forwardDirectionFirst, directionsSearched);
        }
        private bool GetRandomBool(Random rnd)
        {
            return Convert.ToBoolean(rnd.Next(0, 2));
        }
        private bool IsPointAvailable(int x, int y)
        {
            return _board[x, y] == null;
        }
    }

}

