using System.Collections;

namespace CodeGolf.Maze.Core
{
    public class Maze
    {
        public Cell[,] Cells;
        public Cell CurrentCell { get; set; }
        public int Dimension { get; private set; }
        public int VisitedCellCount { get; set; }

        public int TotalCells
        {
            get { return Dimension*Dimension; }
        }

        public Maze(int dimension)
        {
            VisitedCellCount = 1;
            Dimension = dimension;
            Initialize();
        }

        public void Initialize()
        {
            Cells = new Cell[Dimension,Dimension];

            for (int i = 0; i < Dimension; i++)
            {
                for (int j = 0; j < Dimension; j++)
                {
                    Cells[i, j] = new Cell(i, j);
                }
            }
        }

        public ArrayList GetCellNeighborsWithWallsWorthBreaking(Cell cell)
        {
            var neighbors = new ArrayList();

            for (int countRow = -1; countRow <= 1; countRow++)
            {
                for (int countCol = -1; countCol <= 1; countCol++)
                {
                    if (InMazeBounds(countRow, countCol, cell) && IsUpDownLeftRightAndNotSelf(countRow, countCol))
                    {
                        // The cell is left, right, above or below the current cell. Does it have all of it's walls?
                        if (Cells[cell.Row + countRow, cell.Column + countCol].HasAllWalls())
                        {
                            neighbors.Add(Cells[cell.Row + countRow, cell.Column + countCol]);
                        }
                    }
                }
            }

            return neighbors;
        }

        private bool InMazeBounds(int countRow, int countCol, Cell cell)
        {
            return (cell.Row + countRow < Dimension) &&
                   (cell.Row + countRow >= 0) &&
                   (cell.Column + countCol < Dimension) &&
                   (cell.Column + countCol >= 0);
        }

        private static bool IsUpDownLeftRightAndNotSelf(int countRow, int countCol)
        {
            // Must be up, down, left or right with no diagnols
            // and cell must be other than self, [0,0]
            return (countRow != countCol) && ((countCol == 0) || (countRow == 0));
        }

        public ArrayList GetCellNeighborsWhichHaveNotBeenVisited(Cell cell)
        {
            var neighbors = new ArrayList();

            // Look in each direction which cell can turn
            // If no wall and adject wall hasn't been visited.
            for (int countRow = -1; countRow <= 1; countRow++)
            {
                for (int countCol = -1; countCol <= 1; countCol++)
                {
                    if (InMazeBounds(countRow, countCol, cell) && IsUpDownLeftRightAndNotSelf(countRow, countCol))
                    {
                        if (Cells[cell.Row + countRow, cell.Column + countCol].HasAllWalls())
                        {
                            neighbors.Add(Cells[cell.Row + countRow, cell.Column + countCol]);
                        }
                    }
                }
            }

            return neighbors;
        }

    }
}