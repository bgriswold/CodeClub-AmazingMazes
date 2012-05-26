using System;
using System.Linq;

namespace CodeGolf.Maze.Core
{
    public class Cell
    {
        public int Column { get; set; }
        public int Row { get; set; }
        public Enums.CellStates CellState { get; set; }

        public Enums.WallStates[] Walls = new[] { Enums.WallStates.Up, Enums.WallStates.Up, Enums.WallStates.Up, Enums.WallStates.Up };

        public Cell(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public bool HasAllWalls()
        {
            return Enums.WallOrientations.All(WallIsDown);
        }

        private bool WallIsDown(Enums.WallOrientation wallOrientation)
        {
            return Walls[(int)wallOrientation] != Enums.WallStates.Down;
        }

        public void RemoveWall(Cell cell)
        {
            // Each cell has four walls so, effectively, we have double walls separating cells.
            // Let's bust them both down. First break this cell's wall and then the neighbor's.
            var wallToRemove = FindAdjacentWall(cell);
            this.Walls[(int) wallToRemove] = Enums.WallStates.Down;

            var secondWallToRemove = FindOppositeWall(wallToRemove);
            cell.Walls[(int) secondWallToRemove] = Enums.WallStates.Down;
        }

        public Enums.WallOrientation FindAdjacentWall(Cell cell)
        {
            if (cell.Row == Row)
            {
                return cell.Column < Column ? Enums.WallOrientation.North : Enums.WallOrientation.South;
            }
            return cell.Row < Row ? Enums.WallOrientation.East : Enums.WallOrientation.West;
        }

        public Enums.WallOrientation FindOppositeWall(Enums.WallOrientation wall)
        {
            if (wall == Enums.WallOrientation.North) return Enums.WallOrientation.South;
            if (wall == Enums.WallOrientation.East) return Enums.WallOrientation.West;
            if (wall == Enums.WallOrientation.South) return Enums.WallOrientation.North;
            return Enums.WallOrientation.East;
        }
    }
}