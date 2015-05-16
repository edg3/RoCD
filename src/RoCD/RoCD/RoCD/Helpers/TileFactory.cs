using Microsoft.Xna.Framework;
using RoCD.Helpers.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoCD.Helpers
{
    class TileFactory
    {
        private static Random rnd = new Random();

        private static Color[] col_GrassBack = new Color[] { new Color(56, 75, 0), new Color(47, 74, 0), new Color(83, 119, 0), new Color(0, 74, 0) };
        private static Color[] col_GrassFront = new Color[] { new Color(0, 119, 0), new Color(77, 164, 77), new Color(79, 164, 0) };
        private static Point[] til_GrassTiles = new Point[] { new Point(2, 2), new Point(7, 2), new Point(12, 2), new Point(14, 2), new Point(0, 6), new Point(0, 2), new Point(0, 2), new Point(0, 2), new Point(0, 2) };

        public static Tile Grass()
        {
            Point p = til_GrassTiles[rnd.Next(til_GrassTiles.Length)];
            Tile t = new Tile()
            {
                Pathable = true,
                RenderInfo = new TileRenderInfo()
                {
                    BackColor = col_GrassBack[rnd.Next(col_GrassBack.Length)],
                    TileColor = col_GrassFront[rnd.Next(col_GrassFront.Length)],
                    TileX = (byte)p.X,
                    TileY = (byte)p.Y
                }
            };

            return t;
        }

        private static Color[] col_DirtBack = new Color[] { new Color(75, 45, 0), new Color(75, 40, 0), new Color(74, 65, 0) };
        private static Color[] col_DirtFront = new Color[] { new Color(119, 100, 77), new Color(164, 145, 122), new Color(164, 124, 92) };
        private static Point[] til_DirtTiles = new Point[] { new Point(2, 2), new Point(7, 2), new Point(12, 2), new Point(14, 2), new Point(0, 6), new Point(0, 2), new Point(0, 2), new Point(0, 2), new Point(0, 2) };

        public static Tile Dirt()
        {
            Point p = til_DirtTiles[rnd.Next(til_DirtTiles.Length)];
            Tile t = new Tile()
            {
                Pathable = true,
                RenderInfo = new TileRenderInfo()
                {
                    BackColor = col_DirtBack[rnd.Next(col_DirtBack.Length)],
                    TileColor = col_DirtFront[rnd.Next(col_DirtFront.Length)],
                    TileX = (byte)p.X,
                    TileY = (byte)p.Y
                }
            };

            return t;
        }

        private static Color[] col_WaterBack = new Color[] { new Color(0, 115, 255), new Color(47, 119, 240), new Color(0, 86, 255) };
        private static Color[] col_WaterFront = new Color[] { new Color(47, 94, 240), new Color(77, 143, 255), new Color(168, 200, 255) };
        private static Point[] til_WaterTiles = new Point[] { new Point(14, 7), new Point(0, 2), new Point(0, 2) };

        public static Tile Water()
        {
            Point p = til_WaterTiles[rnd.Next(til_WaterTiles.Length)];
            Tile t = new Tile()
            {
                Pathable = false,
                RenderInfo = new TileRenderInfo()
                {
                    BackColor = col_WaterBack[rnd.Next(col_WaterBack.Length)],
                    TileColor = col_WaterFront[rnd.Next(col_WaterFront.Length)],
                    TileX = (byte)p.X,
                    TileY = (byte)p.Y
                }
            };

            return t;
        }
    }
}
