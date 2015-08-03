using Microsoft.Xna.Framework;
using RoCD.Helpers.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoCD.Helpers
{
    public class UIRenderer
    {
        static TileRenderInfo t_horr_bar = new TileRenderInfo()
        {
            TileX = 13,
            TileY = 12
        };

        static TileRenderInfo t_vert_bar = new TileRenderInfo()
        {
            TileX = 10,
            TileY = 11
        };

        static TileRenderInfo t_upright = new TileRenderInfo()
        {
            TileX = 11,
            TileY = 11
        };

        static TileRenderInfo t_upleft = new TileRenderInfo()
        {
            TileX = 9,
            TileY = 12
        };

        static TileRenderInfo t_downleft = new TileRenderInfo()
        {
            TileX = 8,
            TileY = 12
        };

        static TileRenderInfo t_downright = new TileRenderInfo()
        {
            TileX = 12,
            TileY = 11
        };

        static TileRenderInfo t_vert_t_up = new TileRenderInfo()
        {
            TileX = 11,
            TileY = 12
        };

        static TileRenderInfo t_vert_t_down = new TileRenderInfo()
        {
            TileX = 10,
            TileY = 12
        };


        public static void RenderBox(int x, int y, int w, int h, Color backColor, Color foreColor)
        {
            t_horr_bar.BackColor = backColor;
            t_vert_bar.BackColor = backColor;
            t_upleft.BackColor = backColor;
            t_upright.BackColor = backColor;
            t_downleft.BackColor = backColor;
            t_downright.BackColor = backColor;

            t_horr_bar.TileColor = foreColor;
            t_vert_bar.TileColor = foreColor;
            t_upleft.TileColor = foreColor;
            t_upright.TileColor = foreColor;
            t_downleft.TileColor = foreColor;
            t_downright.TileColor = foreColor;

            for (int i = x + 1; i < x + w; i++)
            {
                SpritesheetHelper.RenderTile(t_horr_bar, new Rectangle(i * 12, y * 12, 12, 12));
                SpritesheetHelper.RenderTile(t_horr_bar, new Rectangle(i * 12, (y + h) * 12, 12, 12));
            }

            for (int i = y + 1; i < y + h; i++)
            {
                SpritesheetHelper.RenderTile(t_vert_bar, new Rectangle(x * 12, i * 12, 12, 12));
                SpritesheetHelper.RenderTile(t_vert_bar, new Rectangle((x + w) * 12, i * 12, 12, 12));
            }

            SpritesheetHelper.RenderTile(t_upleft, new Rectangle(x * 12, y * 12, 12, 12));
            SpritesheetHelper.RenderTile(t_upright, new Rectangle((x + w) * 12, y * 12, 12, 12));
            SpritesheetHelper.RenderTile(t_downleft, new Rectangle(x * 12, (y + h) * 12, 12, 12));
            SpritesheetHelper.RenderTile(t_downright, new Rectangle((x + w) * 12, (y + h) * 12, 12, 12));
        }

        public static void RenderVerticalTBar(int x, int y, int h, Color backColor, Color foreColor)
        {
            t_vert_bar.BackColor = backColor;
            t_vert_bar.TileColor = foreColor;

            t_vert_t_up.BackColor = backColor;
            t_vert_t_up.TileColor = foreColor;

            t_vert_t_down.BackColor = backColor;
            t_vert_t_down.TileColor = foreColor;

            for (int i = y + 1; i < y + h; i++)
            {
                SpritesheetHelper.RenderTile(t_vert_bar, new Rectangle(x * 12, i * 12, 12, 12));
            }

            SpritesheetHelper.RenderTile(t_vert_t_up, new Rectangle(x * 12, y * 12, 12, 12));
            SpritesheetHelper.RenderTile(t_vert_t_down, new Rectangle(x * 12, (y + h) * 12, 12, 12));
        }

        public static void ShowInfo(string message, Rectangle target, Color backColor, Color foreColor)
        {
            for (int i = 0; i < message.Length; i++)
            {
                Point tl = GetCharacterPoint(message[i]);
                TileRenderInfo bti = new TileRenderInfo { BackColor = backColor, TileColor = foreColor, TileX = 11, TileY = 13 };
                TileRenderInfo ti = new TileRenderInfo { BackColor = backColor, TileColor = foreColor, TileX = (byte)tl.X, TileY = (byte)tl.Y };
                SpritesheetHelper.RenderTile(bti, target);
                SpritesheetHelper.RenderTile(ti, target);
                target = new Rectangle(target.X + 12, target.Y, target.Width, target.Height);
            }
        }

        private static Point GetCharacterPoint(char p)
        {
            switch (p)
            {
                case 'a': return new Point(1, 6);
                case 'b': return new Point(2, 6);
                case 'c': return new Point(3, 6);
                case 'd': return new Point(4, 6);
                case 'e': return new Point(5, 6);
                case 'f': return new Point(6, 6);
                case 'g': return new Point(7, 6);
                case 'h': return new Point(8, 6);
                case 'i': return new Point(9, 6);
                case 'j': return new Point(10, 6);
                case 'k': return new Point(11, 6);
                case 'l': return new Point(12, 6);
                case 'm': return new Point(13, 6);
                case 'n': return new Point(14, 6);
                case 'o': return new Point(15, 6);

                case 'p': return new Point(0, 7);
                case 'q': return new Point(1, 7);
                case 'r': return new Point(2, 7);
                case 's': return new Point(3, 7);
                case 't': return new Point(4, 7);
                case 'u': return new Point(5, 7);
                case 'v': return new Point(6, 7);
                case 'w': return new Point(7, 7);
                case 'x': return new Point(8, 7);
                case 'y': return new Point(9, 7);
                case 'z': return new Point(10, 7);

                case '0': return new Point(0, 3);
                case '1': return new Point(1, 3);
                case '2': return new Point(2, 3);
                case '3': return new Point(3, 3);
                case '4': return new Point(4, 3);
                case '5': return new Point(5, 3);
                case '6': return new Point(6, 3);
                case '7': return new Point(7, 3);
                case '8': return new Point(8, 3);
                case '9': return new Point(9, 3);

                case '.': return new Point(14, 2);
                case '-': return new Point(15, 0);
                
                case 'A': return new Point(1, 4);
                case 'B': return new Point(2, 4);
                case 'C': return new Point(3, 4);
                case 'D': return new Point(4, 4);
                case 'E': return new Point(5, 4);
                case 'F': return new Point(6, 4);
                case 'G': return new Point(7, 4);
                case 'H': return new Point(8, 4);
                case 'I': return new Point(9, 4);
                case 'J': return new Point(10, 4);
                case 'K': return new Point(11, 4);
                case 'L': return new Point(12, 4);
                case 'M': return new Point(13, 4);
                case 'N': return new Point(14, 4);
                case 'O': return new Point(15, 4);

                case 'P': return new Point(0, 5);
                case 'Q': return new Point(1, 5);
                case 'R': return new Point(2, 5);
                case 'S': return new Point(3, 5);
                case 'T': return new Point(4, 5);
                case 'U': return new Point(5, 5);
                case 'V': return new Point(6, 5);
                case 'W': return new Point(7, 5);
                case 'X': return new Point(8, 5);
                case 'Y': return new Point(9, 5);
                case 'Z': return new Point(10, 5);

                case '!': return new Point(1, 2);
                case '"': return new Point(2, 2);
                case '#': return new Point(3, 2);
                case '$': return new Point(4, 2);
                case '%': return new Point(5, 2);
                case '&': return new Point(6, 2);
                case '\'': return new Point(7, 2);
                case '(': return new Point(8, 2);
                case ')': return new Point(9, 2);

                case '+': return new Point(11, 2);
                case ',': return new Point(12, 2);
                case '/': return new Point(15, 2);

                case ':': return new Point(10, 3);
            }

            return new Point(0, 2);
        }

        internal static void ShowInfo(string p, Rectangle rectangle)
        {
            ShowInfo(p, rectangle, Color.Black, Color.White);
        }
    }
}
