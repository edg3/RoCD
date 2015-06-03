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
        Tile[,] _map = new Tile[2000, 2000];

        public Tile this[int i, int j]
        {
            get
            {
                if ((i < 0) || (j < 0) || (i > 1999) || (j > 1999))
                    return null;
                return _map[i, j];
            }
        }

        public List<Vector2> _cities;

        public Map()
        {
            for (int i = 0; i < 2000; i++)
            {
                for (int j = 0; j < 2000; j++)
                {
                    _map[i, j] = TileFactory.Blank(); //TileFactory.Grass();
                }
            }

            Random rndm = new Random();

            //PCG:
            //place 8 cities
            _cities = new List<Vector2>();
            for (int i = 0; i < 8; i++)
            {
                bool tooClose = true;
                var pnt = new Vector2();

                while (tooClose)
                {
                    pnt.X = rndm.Next(100, 1900);
                    pnt.Y = rndm.Next(100, 1900);

                    tooClose = false;
                    foreach (var pnt2 in _cities)
                    {
                        if ((pnt - pnt2).Length() < 300)
                        {
                            tooClose = true;
                        }
                    }
                }

                _cities.Add(pnt);

                //Fill city temporarily
                int size = rndm.Next(70, 180);
                for (int q = (int)(pnt.X - size / 2); q < (int)(pnt.X + size / 2); q++)
                {
                    for (int r = (int)(pnt.Y - size / 2); r < (int)(pnt.Y + size / 2); r++)
                    {
                        _map[q, r] = TileFactory.Dirt();
                    }
                }

                //TODO: Generate buildings
            }

            //Chain cities together with a simple pathway:
            List<Vector2> _cPathA = new List<Vector2>();
            List<Vector2> _cPathB = new List<Vector2>();
            for (int p = 0; p < _cities.Count; p++)
            {
                var pA = _cities[p];
                Vector2 pB = new Vector2(9999,9999);
                for (int q = 0; q < _cities.Count; q++)
                {
                    if (q == p) continue; //Skip the current city
                    
                    if ((pA - pB).Length() > (pA - _cities[q]).Length())
                    {
                        pB = _cities[q];
                    }
                }

                _cPathA.Add(pA);
                _cPathB.Add(pB);
            }
            for (int p = 0; p < _cPathA.Count; p++)
            {
                var pA = _cPathA[p];
                var pB = _cPathB[p];

                int favour = rndm.Next(2);
                switch (favour)
                {
                    case 0:
                        { //Vertical
                            //mid point: //TODO: Make this a random point
                            int m_y = (int)(pA.Y + pB.Y) / 2;
                            int q_a = m_y;
                            int q_b = (int)pA.Y;
                            if (q_b < q_a)
                            {
                                int t = q_a;
                                q_a = q_b;
                                q_b = t;
                            }
                            for (int q = q_a; q < q_b; q++)
                            {
                                for (int r = (int)(pA.X - 4); r < pA.X + 4; r++)
                                    _map[r, q] = TileFactory.Grass();
                            }
                            q_a = m_y;
                            q_b = (int)pB.Y;
                            if (q_b < q_a)
                            {
                                int t = q_a;
                                q_a = q_b;
                                q_b = t;
                            }
                            for (int q = q_a; q < q_b; q++)
                            {
                                for (int r = (int)(pB.X - 4); r < pB.X + 4; r++)
                                    _map[r, q] = TileFactory.Grass();
                            }
                            q_a = (int)pA.X;
                            q_b = (int)pB.X;
                            if (q_b < q_a)
                            {
                                int t = q_a;
                                q_a = q_b;
                                q_b = t;
                            }
                            for (int q = q_a; q < q_b; q++)
                            {
                                for (int r = m_y - 4; r < m_y + 4; r++)
                                {
                                    _map[q, r] = TileFactory.Grass();
                                }
                            }
                        } break;
                    case 1:
                        { //Horrizontal
                            //mid point: //TODO: Make this a random point
                            int m_x = (int)(pA.X + pB.X) / 2;
                            int q_a = m_x;
                            int q_b = (int)pA.X;
                            if (q_b < q_a)
                            {
                                int t = q_a;
                                q_a = q_b;
                                q_b = t;
                            }
                            for (int q = q_a; q < q_b; q++)
                            {
                                for (int r = (int)(pA.Y - 4); r < pA.Y + 4; r++)
                                    _map[q, r] = TileFactory.Grass();
                            }
                            q_a = m_x;
                            q_b = (int)pB.X;
                            if (q_b < q_a)
                            {
                                int t = q_a;
                                q_a = q_b;
                                q_b = t;
                            }
                            for (int q = q_a; q < q_b; q++)
                            {
                                for (int r = (int)(pB.Y - 4); r < pB.Y + 4; r++)
                                    _map[q, r] = TileFactory.Grass();
                            }
                            q_a = (int)pA.Y;
                            q_b = (int)pB.Y;
                            if (q_b < q_a)
                            {
                                int t = q_a;
                                q_a = q_b;
                                q_b = t;
                            }
                            for (int q = q_a; q < q_b; q++)
                            {
                                for (int r = m_x - 4; r < m_x + 4; r++)
                                {
                                    _map[r, q] = TileFactory.Grass();
                                }
                            }
                        } break;
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
                        adj.Add(new Point(c_x, c_y));
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
                case Direction.Down: n_x = toMove.X; n_y = toMove.Y + 1; tileTo = this[toMove.X, toMove.Y + 1]; distanceMoved = 1; break;
                case Direction.DownLeft: n_x = toMove.X - 1; n_y = toMove.Y + 1; tileTo = this[toMove.X - 1, toMove.Y + 1]; distanceMoved = 1.4142d; break;
                case Direction.DownRight: n_x = toMove.X + 1; n_y = toMove.Y + 1; tileTo = this[toMove.X + 1, toMove.Y + 1]; distanceMoved = 1.4142d; break;
                case Direction.Left: n_x = toMove.X - 1; n_y = toMove.Y; tileTo = this[toMove.X - 1, toMove.Y]; distanceMoved = 1; break;
                case Direction.Right: n_x = toMove.X + 1; n_y = toMove.Y; tileTo = this[toMove.X + 1, toMove.Y]; distanceMoved = 1; break;
                case Direction.Up: n_x = toMove.X; n_y = toMove.Y - 1; tileTo = this[toMove.X, toMove.Y - 1]; distanceMoved = 1; break;
                case Direction.UpLeft: n_x = toMove.X - 1; n_y = toMove.Y - 1; tileTo = this[toMove.X - 1, toMove.Y - 1]; distanceMoved = 1.4142d; break;
                case Direction.UpRight: n_x = toMove.X + 1; n_y = toMove.Y - 1; tileTo = this[toMove.X + 1, toMove.Y - 1]; distanceMoved = 1.4142d; break;
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
