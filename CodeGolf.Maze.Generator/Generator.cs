using System;
using System.Collections;
using CodeGolf.Maze.Core;

namespace CodeGolf.Maze.Generator
{
    public class Generator
    {
        private readonly int _dimension;
        
        public Generator(int dimension)
        {
            _dimension = dimension;
        }

        public Core.Maze Generate()
        {
            if (_dimension < 5) throw new ArgumentOutOfRangeException();

            var maze = new Core.Maze(_dimension);
            maze.Initialize();

            // Set start position
            maze.CurrentCell = maze.Cells[0, 0];
            maze.VisitedCellCount = 1;

            var random = new Random();
            var cellStack = new Stack();

            while (maze.VisitedCellCount < maze.TotalCells)
            {
                // Gather cells to the left, right, above and below which have all walls intact
                ArrayList adjacentCellsWithWalls = maze.GetCellNeighborsWithWallsWorthBreaking(maze.CurrentCell);
                
                if (adjacentCellsWithWalls.Count > 0)
                {
                    // Choose a cell at random, otherwise the maze is really lame.
                    int index = random.Next(0, adjacentCellsWithWalls.Count);

                    var nextCell = ((Cell)adjacentCellsWithWalls[index]);

                    // This is actually walls since each cell has four walls, 
                    // I need to remove the current cell's wall and the adjectent cell's wall.
                    maze.CurrentCell.RemoveWall(nextCell);

                    // We need the stack in case we hit a deadend where 
                    // there are no adjacent cells with walls and we need to backtrack
                    cellStack.Push(maze.CurrentCell); 

                    maze.CurrentCell = nextCell; 
                    maze.VisitedCellCount++;
                }
                else
                {
                    // This is the deadend. Move backwards until available walls are found.
                    maze.CurrentCell = (Cell) cellStack.Pop();
                }
            }

            // Break start and end walls.
            maze.Cells[0, 0].Walls[(int)Enums.WallOrientation.North] = Enums.WallStates.Down;
            maze.Cells[maze.Dimension - 1, maze.Dimension - 1].Walls[(int)Enums.WallOrientation.West] = Enums.WallStates.Down;

            return maze;
        }
    }
}