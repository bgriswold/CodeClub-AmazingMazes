using System;
using CodeGolf.Maze.Core;

namespace CodeGolf.Maze.Solver
{
    public class Solver
    {
        private readonly Core.Maze _maze;

        public Solver(Core.Maze maze)
        {
            _maze = maze;
        }

        public Core.Maze Solve()
        {
            if (_maze == null) throw new ArgumentOutOfRangeException();

            bool solved;
            
            do
            {
                solved = true;

                // Loop through the cells, marking any cell with less than 2 possible moves as OffTrack.
                // All remaining cells should have two available moves, backwards and 1 forward direction along the solution. 
                // (I'm worried there's a loop hole where a cell can exist with more than two moves. Haven't seen it yet though.)

                // If I wanted to capture the step-by-step solution, an algorithm similar to what is used in the generator could be used.
                // This would be nice because not all cells would need to be visited in order to solve the maze. Cells which were
                // visited and then backed out of could have a different state and different color and be represented as part of the 
                // attempted solution. I did the stack solution on paper before this solution came up.

                // Of course, I could still use this approach and then walk the maze building a sequence of OnTrack cells which will 
                // produce the ordered solution path.
                for (int i = 0; i < _maze.Dimension; i++)
                {
                    for (int j = 0; j < _maze.Dimension; j++)
                    {
                        // All cells are assumed to be OnTrack in first pass
                        if (_maze.Cells[i, j].CellState == Enums.CellStates.OnTrack)
                        {
                            if (!HasPossibleMoves(i, j))
                            {
                                _maze.Cells[i, j].CellState = Enums.CellStates.OffTrack;
                                solved = false;
                            }
                        }
                    }
                }
            } while (!solved);

            return _maze;
        }
        
        private bool HasPossibleMoves(int x, int y)
        {
            int possibleMoves = 4;
            
            // if north wall is up OR cell above is OffTrack the wall is assumed
            if (_maze.Cells[x, y].Walls[(int) Enums.WallOrientation.North] == Enums.WallStates.Up)
               possibleMoves--;
            else if (y > 0 && (_maze.Cells[x, y - 1].CellState == Enums.CellStates.OffTrack))
               possibleMoves--;

            // change direction...perform same check
            if (_maze.Cells[x, y].Walls[(int)Enums.WallOrientation.East] == Enums.WallStates.Up)
               possibleMoves--;
            else if (x > 0 && _maze.Cells[x - 1, y].CellState == Enums.CellStates.OffTrack)
               possibleMoves--;

            if (_maze.Cells[x, y].Walls[(int)Enums.WallOrientation.South] == Enums.WallStates.Up)
               possibleMoves--;
            else if (y < _maze.Dimension - 1 && _maze.Cells[x, y + 1].CellState == Enums.CellStates.OffTrack)
               possibleMoves--;

            if (_maze.Cells[x, y].Walls[(int)Enums.WallOrientation.West] == Enums.WallStates.Up)
               possibleMoves--;
            else if (x < _maze.Dimension - 1 && _maze.Cells[x + 1, y].CellState == Enums.CellStates.OffTrack)
               possibleMoves--;

            // Can move backwards and at least 1 direction forward 
            return possibleMoves > 1;
        }
    }
}