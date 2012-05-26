using System;
using System.Collections.Generic;

namespace CodeGolf.Maze.Core
{
    public static class Enums
    {
        public enum CellStates
        {
            OnTrack,
            OffTrack
        }

        public enum WallStates
        {
            Up,
            Down
        }

        public enum WallOrientation
        {
            North = 0,
            East = 1,
            South = 2,
            West = 3
        }

        // this allows foreach (CompassDirection d in DirectionsEnum) {..}
        public static IEnumerable<WallOrientation> WallOrientations
        {
            get
            {
                yield return WallOrientation.North;
                yield return WallOrientation.East;
                yield return WallOrientation.South;
                yield return WallOrientation.West;
            }
        }
    }
}