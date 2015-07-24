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
        public const int MapWidth = 500;
        public const int MapHeight = 500;
        public const int CityRange = 30;
        public const int CitySmall = 10;
        public const int CityLarge = 30;
        public const int CityCount = 3;
        public const int WorldBlotches = 3000;
        public const int WorldBlotchMin = 10;
        public const int WorldBlotchMax = 15;
        public const int SpawnerCount = 100;

        Tile[,] _map = new Tile[MapWidth, MapHeight];

        public Tile this[int i, int j]
        {
            get
            {
                if ((i < 0) || (j < 0) || (i > MapWidth - 1) || (j > MapHeight - 1))
                    return null;
                return _map[i, j];
            }
        }

        public List<Vector2> _cities;

        public Map()
        {
            for (int i = 0; i < MapWidth; i++)
            {
                for (int j = 0; j < MapHeight; j++)
                {
                    _map[i, j] = TileFactory.Blank(); //TileFactory.Grass();
                }
            }

            Random rndm = new Random();

            //PCG:
            //place 8 cities
            List<Vector2> _cPathA = new List<Vector2>();
            List<Vector2> _cPathB = new List<Vector2>();

            _cities = new List<Vector2>();
            for (int i = 0; i < CityCount; i++)
            {
                bool tooClose = true;
                var pnt = new Vector2();

                while (tooClose)
                {
                    pnt.X = rndm.Next(100, MapWidth - 100);
                    pnt.Y = rndm.Next(100, MapHeight - 100);

                    tooClose = false;
                    foreach (var pnt2 in _cities)
                    {
                        if ((pnt - pnt2).Length() < CityRange)
                        {
                            tooClose = true;
                        }
                    }
                }

                if (_cities.Count() > 1)
                {
                    _cPathA.Add(_cities.Last());
                    _cPathB.Add(pnt);
                }

                _cities.Add(pnt);

                //Fill city temporarily
                int size = rndm.Next(CitySmall, CityLarge);
                for (int q = (int)(pnt.X - size / 2); q < (int)(pnt.X + size / 2); q++)
                {
                    for (int r = (int)(pnt.Y - size / 2); r < (int)(pnt.Y + size / 2); r++)
                    {
                        _map[q % MapWidth, r % MapHeight] = TileFactory.Dirt();
                    }
                }

                //TODO: Generate buildings
            }

            //Chain cities together with a simple pathway:
            
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
                            int r_q = 4;
                            for (int q = q_a; q < q_b; q++)
                            {
                                r_q += rndm.Next(3) - 1;
                                r_q = (int)MathHelper.Clamp(r_q, 4, 8);
                                for (int r = (int)(pA.X - r_q); r < pA.X + r_q; r++)
                                    _map[r, q] = (_map[r, q].Pathable == true ? _map[r, q] : TileFactory.Path());
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
                                r_q += rndm.Next(3) - 1;
                                r_q = (int)MathHelper.Clamp(r_q, 4, 8);
                                for (int r = (int)(pB.X - r_q); r < pB.X + r_q; r++)
                                    _map[r, q] = (_map[r, q].Pathable == true ? _map[r, q] : TileFactory.Path());
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
                                r_q += rndm.Next(3) - 1;
                                r_q = (int)MathHelper.Clamp(r_q, 4, 8);
                                for (int r = m_y - r_q; r < m_y + r_q; r++)
                                {
                                    _map[q, r] = (_map[q, r].Pathable == true ? _map[q, r] : TileFactory.Path());
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
                            int r_q = 4;
                            for (int q = q_a; q < q_b; q++)
                            {
                                r_q += rndm.Next(3) - 1;
                                r_q = (int)MathHelper.Clamp(r_q, 4, 8);
                                for (int r = (int)(pA.Y - r_q); r < pA.Y + r_q; r++)
                                    _map[q, r] = (_map[q, r].Pathable == true ? _map[q, r] : TileFactory.Path());
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
                                r_q += rndm.Next(3) - 1;
                                r_q = (int)MathHelper.Clamp(r_q, 4, 8);
                                for (int r = (int)(pB.Y - r_q); r < pB.Y + r_q; r++)
                                    _map[q, r] = (_map[q, r].Pathable == true ? _map[q, r] : TileFactory.Path());
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
                                r_q += rndm.Next(3) - 1;
                                r_q = (int)MathHelper.Clamp(r_q, 4, 8);
                                for (int r = m_x - r_q; r < m_x + r_q; r++)
                                {
                                    _map[r, q] = (_map[r, q].Pathable == true ? _map[r, q] : TileFactory.Path());
                                }
                            }
                        } break;
                }
            }

            //build a fully connected and interesting world map
            List<Point> Connections = new List<Point>();
            for (int i = 0; i < WorldBlotches; i++)
            {
                Connections.Add(new Point(rndm.Next(100, Map.MapWidth - 100), rndm.Next(100, Map.MapHeight - 100)));
            }

            Connections = (from item in Connections
                           where PointHasPointNextToIt(Connections, item)
                           select item).ToList();

            foreach (var pnt in Connections)
            {
                int r = rndm.Next(WorldBlotchMin, WorldBlotchMax);
                for (int i = pnt.X - r; i < pnt.X + r; i++)
                {
                    for (int j = pnt.Y - r; j < pnt.Y + r; j++)
                    {
                        if ((i < 0) || (i > MapWidth - 1) || (j < 0) || (j > MapHeight - 1)) continue;

                        if (Math.Sqrt((pnt.X - i)*(pnt.X - i) + (pnt.Y - j)*(pnt.Y -j)) <= r)
                        {
                            _map[i, j] = (_map[i, j].Pathable == true ? _map[i, j] : TileFactory.Grass());
                        }
                    }
                }
            }

            //flood fill the map with structures:
            for (int i = 0; i < MapWidth; i++)
            {
                for (int j = 0; j < MapHeight; j++)
                {
                    if (_map[i,j].RenderInfo.BackColor == Color.Red)
                    {
                        FloodFillFeature(i, j, rndm);
                    }
                }
            }

            //Add in 5000 spawners:
            for (int i = 0; i < SpawnerCount; i++)
            {
                int q = rndm.Next(MapWidth);
                int r = rndm.Next(MapHeight);
                while (_map[q,r].Pathable == false)
                {
                    q = rndm.Next(MapWidth);
                    r = rndm.Next(MapHeight);
                }

                _map[q, r].Contained = new Spawner { X = q, Y = r};
                registeredActors.Add(_map[q, r].Contained);
            }
        }

        private void FloodFillFeature(int i, int j, Random rndm)
        {
            //int chosen = rndm.Next(99);
            //if (chosen < 75)
            //{
                FloodFillTrees(i, j);
            //}
            //else
            //{
            //    FloodFillMountains(i, j);
            //}
        }

        private void FloodFillMountains(int i, int j)
        {
            throw new NotImplementedException();
        }

        private void FloodFillTrees(int i, int j)
        {
            _map[i, j] = TileFactory.Tree();
            //TODO: fix stack overflowing
            //if (i > 1)
            //{
            //    if (_map[i - 1, j].RenderInfo.BackColor == Color.Red)
            //    {
            //        FloodFillTrees(i - 1, j);
            //    }
            //}
            //if (j > 1)
            //{
            //    if (_map[i, j - 1].RenderInfo.BackColor == Color.Red)
            //    {
            //        FloodFillTrees(i, j - 1);
            //    }
            //}
            //if (i < 1999)
            //{
            //    if (_map[i + 1, j].RenderInfo.BackColor == Color.Red)
            //    {
            //        FloodFillTrees(i + 1, j);
            //    }
            //}
            //if (j < 1999)
            //{
            //    if (_map[i, j + 1].RenderInfo.BackColor == Color.Red)
            //    {
            //        FloodFillTrees(i, j + 1);
            //    }
            //}
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
                    if ((c_x > 0) && (c_x < MapWidth) && (c_y >= 0) && (c_y < MapHeight))
                    {
                        adj.Add(new Point(c_x, c_y));
                    }
                }
            }

            return adj;
        }

        private bool PointHasPointNextToIt(List<Point> points, Point pnt)
        {
            foreach (var point in points)
            {
                double dst = Math.Sqrt((pnt.X - point.X)*(pnt.X - point.X) + (pnt.Y - point.Y)*(pnt.Y - point.Y));
                if ((dst >= 1) && (dst <= 2))
                {
                    return true;
                }
            }

            return false;
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
                    if ((c_x > 0) && (c_x < MapWidth) && (c_y >= 0) && (c_y < MapHeight))
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
            UpRight,
            Random
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="toMove"></param>
        /// <param name="direction"></param>
        /// <returns>Distance moved.</returns>
        public Actor MoveActor(Actor toMove, Direction direction)
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
                case Direction.Random: Random rndm = new Random(); n_x = toMove.X + rndm.Next(3) - 1; n_y = toMove.Y + rndm.Next(3) - 1; tileTo = this[n_x, n_y]; distanceMoved = 1.4142d; break;
            }

            if (tileTo != null)
            {
                if ((tileTo.Pathable) && (tileTo.Contained == null))
                {
                    tileTo.Contained = toMove;
                    tileAt.Contained = null;

                    toMove.X = n_x;
                    toMove.Y = n_y;

                    return null;
                }
            }

            if (tileTo.Contained != null) return tileTo.Contained;

            return null;
        }

        public List<Actor> registeredActors = new List<Actor>();

        internal void Update(Actor player)
        {
            //TODO: better way to do this
            for (int i = registeredActors.Count - 1; i > -1; i--)
            {
                var item = registeredActors[i];
                item.Update(this, player);
                if (item is Creature)
                {
                    if (item != player)
                    {
                        if ((item as Creature).get(Creature.CURRHP) <= 0)
                        {
                            //remove, dead!
                            var i_temp = item;
                            registeredActors.Remove(item);
                            _map[i_temp.X, i_temp.Y].Contained = null;
                        }
                    }
                }
            }
            //for (int i = 0; i < 2000; i++)
            //    for (int j = 0; j < 2000; j++)
        }

        public Direction AStarTo(Actor _from, Actor _target)
        {
            //TODO: proper pathfinding
            double d_up = Math.Sqrt(Math.Pow(_from.X - _target.X, 2) + Math.Pow(_from.Y - 1 - _target.Y, 2));
            double d_down = Math.Sqrt(Math.Pow(_from.X - _target.X, 2) + Math.Pow(_from.Y + 1 - _target.Y, 2));
            double d_left = Math.Sqrt(Math.Pow(_from.X - 1 - _target.X, 2) + Math.Pow(_from.Y - _target.Y, 2));
            double d_right = Math.Sqrt(Math.Pow(_from.X + 1 - _target.X, 2) + Math.Pow(_from.Y - _target.Y, 2));

            double[] distances = new double[4]{ d_up, d_down, d_left, d_right };
            Direction[] directions = new Direction[4] { Direction.Up, Direction.Down, Direction.Left, Direction.Right };
            for (int inner = 0; inner < 3; inner++)
            {
                for (int outer = inner + 1; outer < 4; outer++)
                {
                    if (distances[inner] > distances[outer])
                    {
                        double inside = distances[inner];
                        Direction dir_inside = directions[inner];

                        distances[inner] = distances[outer];
                        directions[inner] = directions[outer];

                        distances[outer] = inside;
                        directions[outer] = dir_inside;
                    }
                }
            }

            List<Direction> _choosable_directions = new List<Direction>();

            double closest_move = distances[0];
            for (int i = 0; i < 4; i++)
            {
                if (distances[i] == closest_move)
                    _choosable_directions.Add(directions[i]);
            }

            Random rndm = new Random();

            return _choosable_directions[rndm.Next(_choosable_directions.Count)];
        }
    }
}
