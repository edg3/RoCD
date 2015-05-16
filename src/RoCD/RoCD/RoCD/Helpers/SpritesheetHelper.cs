using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoCD.Helpers.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoCD.Helpers
{
    class SpritesheetHelper
    {
        private static Rectangle _renderFrom = new Rectangle(0, 0, 12, 12);
        public static void RenderTile(Tile tile, SpriteBatch spriteBatch, Texture2D tex2d, Rectangle target)
        {
            TileRenderInfo ti = tile.RenderInfo;

            _renderFrom.X = 11 * 12;
            _renderFrom.Y = 13 * 12;

            spriteBatch.Draw(tex2d, target, _renderFrom, ti.BackColor);

            if (tile.Contained == null)
            {
                _renderFrom.X = ti.TileX * 12;
                _renderFrom.Y = ti.TileY * 12;

                spriteBatch.Draw(tex2d, target, _renderFrom, ti.TileColor);
            }
            else
            {
                _renderFrom.X = tile.Contained.TileX * 12;
                _renderFrom.Y = tile.Contained.TileY * 12;

                spriteBatch.Draw(tex2d, target, _renderFrom, tile.Contained.DrawColor);
            }
        }

        public static void RenderTile(TileRenderInfo ti, SpriteBatch spriteBatch, Texture2D tex2d, Rectangle target)
        {

            _renderFrom.X = 11 * 12;
            _renderFrom.Y = 13 * 12;

            spriteBatch.Draw(tex2d, target, _renderFrom, ti.BackColor);

            _renderFrom.X = ti.TileX * 12;
            _renderFrom.Y = ti.TileY * 12;

            spriteBatch.Draw(tex2d, target, _renderFrom, ti.TileColor);
        }

        public static Color[] GrassBackColor = new Color[3] { new Color(0, 51, 0), new Color(0, 102, 51), new Color(51, 102, 51) };
        public static Color[] GrassForeColor = new Color[3] { new Color(0, 102, 0), new Color(51, 153, 61), new Color(51, 204, 51) };
    }
}
