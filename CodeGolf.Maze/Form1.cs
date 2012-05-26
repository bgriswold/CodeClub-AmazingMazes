using System.Drawing;
using System.Windows.Forms;
using CodeGolf.Maze.Core;

namespace CodeGolf.Maze
{
    public partial class Form1 : Form
    {
        private Core.Maze _maze;
        private const int Dimensions = 40;
        private const int CellSize = 10;
        private const int CellPadding = 5;
        private bool _isMazeDrawn;

        // Maze rendering to UI help: http://www.c-sharpcorner.com/UploadFile/mgold/Maze09222005021857AM/Maze.aspx
        public Form1()
        {
            InitializeComponent();

            SetBounds(Left, Top, (Dimensions + 2)*CellSize + CellPadding, (Dimensions + 5)*CellSize + CellPadding);

            Cursor = Cursors.WaitCursor;

            var generator = new Generator.Generator(Dimensions);
            _maze = generator.Generate();
            
            Cursor = Cursors.Arrow;
        }


        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            if (!_isMazeDrawn)
            {
                DrawMazeBackground(g);
                DrawMaze(g);
                _isMazeDrawn = true;
            }
            else
            {
                DrawSolution(g);
            }
        }

        private void DrawMazeBackground(Graphics g)
        {
            g.FillRectangle(Brushes.White, ClientRectangle);
        }

        public void DrawMaze(Graphics g)
        {
            for (int i = 0; i < _maze.Dimension; i++)
            {
                for (int j = 0; j < _maze.Dimension; j++)
                {
                    DrawCell(i, j, g);
                }
            }
        }

        public void DrawSolution(Graphics g)
        {
            for (int i = 0; i < _maze.Dimension; i++)
            {
                for (int j = 0; j < _maze.Dimension; j++)
                {
                    DrawCell(i, j, g, true);
                }
            }
        }
        public void DrawCell(int i, int j, Graphics g, bool drawSolution = false)
        {
            Cell cell = _maze.Cells[i, j];

            // Fill in the solution
            if (drawSolution && cell.CellState == Enums.CellStates.OnTrack)
            {
                var fillPen = new Pen(Color.LightGreen);
                Brush fillBrush = Brushes.LightGreen;

                // Color the cell
                g.FillRectangle(fillBrush, cell.Row * CellSize + CellPadding + 1, cell.Column * CellSize + CellPadding + 1,
                                CellSize - 1,
                                CellSize - 1);

                // Color the walls on the solution path the color of the solution cells
                if (cell.Walls[(int)Enums.WallOrientation.North] == Enums.WallStates.Down)
                    DrawWallNorth(cell, g, fillPen);
                if (cell.Walls[(int)Enums.WallOrientation.East] == Enums.WallStates.Down)
                    DrawWallEast(cell, g, fillPen);
                if (cell.Walls[(int)Enums.WallOrientation.South] == Enums.WallStates.Down)
                    DrawWallSouth(cell, g, fillPen);
                if (cell.Walls[(int)Enums.WallOrientation.West] == Enums.WallStates.Down)
                    DrawWallWest(cell, g, fillPen);
            }

            
            DrawCellWalls(cell, g);
        }

        private static void DrawCellWalls(Cell cell, Graphics g)
        {
            var fillPen = new Pen(Color.Blue);

            if (cell.Walls[(int)Enums.WallOrientation.North] == Enums.WallStates.Up)
                DrawWallNorth(cell, g, fillPen);

            if (cell.Walls[(int)Enums.WallOrientation.East] == Enums.WallStates.Up)
                DrawWallEast(cell, g, fillPen);

            if (cell.Walls[(int)Enums.WallOrientation.South] == Enums.WallStates.Up)
                DrawWallSouth(cell, g, fillPen);

            if (cell.Walls[(int)Enums.WallOrientation.West] == Enums.WallStates.Up)
                DrawWallWest(cell, g, fillPen);
        }

        private static void DrawWallWest(Cell cell, Graphics g, Pen fillPen)
        {
            g.DrawLine(fillPen, (cell.Row + 1) * CellSize + CellPadding, cell.Column * CellSize + CellPadding,
                       (cell.Row + 1) * CellSize + CellPadding, (cell.Column + 1) * CellSize + CellPadding);
        }

        private static void DrawWallSouth(Cell cell, Graphics g, Pen fillPen)
        {
            g.DrawLine(fillPen, cell.Row * CellSize + CellPadding, (cell.Column + 1) * CellSize + CellPadding,
                       (cell.Row + 1) * CellSize + CellPadding, (cell.Column + 1) * CellSize + CellPadding);
        }

        private static void DrawWallEast(Cell cell, Graphics g, Pen fillPen)
        {
            g.DrawLine(fillPen, cell.Row * CellSize + CellPadding, cell.Column * CellSize + CellPadding,
                       cell.Row * CellSize + CellPadding,
                       (cell.Column + 1) * CellSize + CellPadding);
        }

        private static void DrawWallNorth(Cell cell, Graphics g, Pen fillPen)
        {
            g.DrawLine(fillPen, cell.Row * CellSize + CellPadding, cell.Column * CellSize + CellPadding,
                       (cell.Row + 1) * CellSize + CellPadding, cell.Column * CellSize + +CellPadding);
        }

        private void From1_DoubleClick(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            var solver = new Solver.Solver(_maze);
            _maze = solver.Solve();
            this.Refresh();
            Cursor = Cursors.Arrow;
        }
    }
}