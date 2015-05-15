using Microsoft.Xna.Framework;
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
            get { return _map[i, j]; }
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

    }
}
