using Microsoft.Xna.Framework;
using RoCD.Mechanics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoCD.Helpers.Tiles
{
    public class Map
    {
        Tile[,] _map = new Tile[200, 200];

        public Tile this[int i, int j]
        {
            get {
                if ((i < 0) || (j < 0) || (i > 199) || (j > 199))
                    return null;
                return _map[i, j];
            }
        }

        public Map()
        {
            for (int i = 0; i < 200; i++)
            {
                for (int j = 0; j < 200; j++)
                {
                    _map[i, j] = TileFactory.Grass();
                }
            }
        }

        public List<Point> Adjacent(int x, int y)
        {
            List<Point> adj = new List<Point>();
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if ((i == 0) && (j == 0)) continue;

                    int c_x = x + i;
                    int c_y = y + j;

                    //TODO: Adjust for wrapping and multiple map files
                    if ((c_x > 0) && (c_x < 200) && (c_y >= 0) && (c_y < 200))
                    {
                        adj.Add(new Point(c_x,c_y));
                    }
                }
            }

            return adj;
        }

        public List<Point> Adjacent(int x, int y, bool pathable)
        {
            List<Point> adj = new List<Point>();
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if ((i == 0) && (j == 0)) continue;

                    int c_x = x + i;
                    int c_y = y + j;

                    //TODO: Adjust for wrapping and multiple map files
                    if ((c_x > 0) && (c_x < 200) && (c_y >= 0) && (c_y < 200))
                    {
                        if (_map[c_x, c_y].Pathable == pathable)
                            adj.Add(new Point(c_x, c_y));
                    }
                }
            }

            return adj;
        }

        public enum Direction
        {
            Right,
            DownRight,
            Down,
            DownLeft,
            Left,
            UpLeft,
            Up,
            UpRight
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="toMove"></param>
        /// <param name="direction"></param>
        /// <returns>Distance moved.</returns>
        public double MoveActor(Actor toMove, Direction direction)
        {
            var tileAt = this[toMove.X, toMove.Y];
            Tile tileTo = null;

            int n_x = 0;
            int n_y = 0;

            double distanceMoved = 0;

            switch (direction)
            {
                case Direction.Down:      n_x = toMove.X; n_y = toMove.Y + 1;       tileTo = this[toMove.X, toMove.Y + 1]; distanceMoved = 1; break;
                case Direction.DownLeft:  n_x = toMove.X - 1; n_y = toMove.Y + 1;   tileTo = this[toMove.X - 1, toMove.Y + 1];  distanceMoved = 1.4142d; break;
                case Direction.DownRight: n_x = toMove.X + 1; n_y = toMove.Y + 1;   tileTo = this[toMove.X + 1, toMove.Y + 1];  distanceMoved = 1.4142d; break;
                case Direction.Left:      n_x = toMove.X - 1; n_y = toMove.Y;       tileTo = this[toMove.X - 1, toMove.Y];      distanceMoved = 1; break;
                case Direction.Right:     n_x = toMove.X + 1; n_y = toMove.Y;       tileTo = this[toMove.X + 1, toMove.Y];      distanceMoved = 1; break;
                case Direction.Up:        n_x = toMove.X; n_y = toMove.Y - 1;       tileTo = this[toMove.X, toMove.Y - 1];      distanceMoved = 1; break;
                case Direction.UpLeft:    n_x = toMove.X - 1; n_y = toMove.Y - 1;   tileTo = this[toMove.X - 1, toMove.Y - 1];  distanceMoved = 1.4142d; break;
                case Direction.UpRight:   n_x = toMove.X + 1; n_y = toMove.Y - 1;   tileTo = this[toMove.X + 1, toMove.Y - 1]; distanceMoved = 1.4142d; break;
            }

            if (tileTo != null)
            {
                if ((tileTo.Pathable) && (tileTo.Contained == null))
                {
                    tileTo.Contained = toMove;
                    tileAt.Contained = null;

                    toMove.X = n_x;
                    toMove.Y = n_y;

                    return distanceMoved;
                }
            }

            return 0;
        }

    }
}
