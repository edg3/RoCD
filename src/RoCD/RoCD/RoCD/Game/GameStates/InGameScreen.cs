using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RapidXNA.Interfaces;
using RoCD.Helpers;
using RoCD.Helpers.Tiles;
using RoCD.Mechanics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RapidXNA;
using RapidXNA.Models;
using System.IO;

namespace RoCD.Game.GameStates
{
    public class InGameScreen : GameScreen
    {
        private Texture2D _spriteSheet;

        public Texture2D SpriteSheet
        {
            get { return _spriteSheet; }
        }

        Actor player = new Actor() { X = 19, Y = 19, Identity = "you" };
        Map _map = new Map();

        public override void Load()
        {
            Engine.ScreenService.DrawEnabled = true;

            _spriteSheet = Engine.ContentManager.Load<Texture2D>("12x12");

            var pnt = _map._cities[0];

            player.X = (int)(pnt.X);
            player.Y = (int)(pnt.Y);
            _map[(int)(pnt.X), (int)(pnt.Y)].Contained = player;

            //TODO: Decent UI stack
            _uiPermanents.Add(new UIRenderInfo()
            {
                RenderInfo = new TileRenderInfo()
                {
                    BackColor = Color.Black,
                    TileColor = Color.White,
                    TileX = 6,
                    TileY = 14
                },
                X = 0,
                Y = 0
            });
            for (int i = 1; i < 38; i++)
            {
                _uiPermanents.Add(new UIRenderInfo()
                {
                    RenderInfo = new TileRenderInfo()
                    {
                        BackColor = Color.Black,
                        TileColor = Color.White,
                        TileX = 13,
                        TileY = 12
                    },
                    X = i,
                    Y = 0
                });
                _uiPermanents.Add(new UIRenderInfo()
                {
                    RenderInfo = new TileRenderInfo()
                    {
                        BackColor = Color.Black,
                        TileColor = Color.White,
                        TileX = 13,
                        TileY = 12
                    },
                    X = i,
                    Y = 38
                });
                _uiPermanents.Add(new UIRenderInfo()
                {
                    RenderInfo = new TileRenderInfo()
                    {
                        BackColor = Color.Black,
                        TileColor = Color.White,
                        TileX = 10,
                        TileY = 11
                    },
                    X = 0,
                    Y = i
                });
                _uiPermanents.Add(new UIRenderInfo()
                {
                    RenderInfo = new TileRenderInfo()
                    {
                        BackColor = Color.Black,
                        TileColor = Color.White,
                        TileX = 10,
                        TileY = 11
                    },
                    X = 38,
                    Y = i
                });
                _uiPermanents.Add(new UIRenderInfo()
                {
                    RenderInfo = new TileRenderInfo()
                    {
                        BackColor = Color.Black,
                        TileColor = Color.White,
                        TileX = 10,
                        TileY = 11
                    },
                    X = 65,
                    Y = i
                });
            }
            _uiPermanents.Add(new UIRenderInfo()
            {
                RenderInfo = new TileRenderInfo()
                {
                    BackColor = Color.Black,
                    TileColor = Color.White,
                    TileX = 11,
                    TileY = 12
                },
                X = 38,
                Y = 0
            });
            _uiPermanents.Add(new UIRenderInfo()
            {
                RenderInfo = new TileRenderInfo()
                {
                    BackColor = Color.Black,
                    TileColor = Color.White,
                    TileX = 10,
                    TileY = 12
                },
                X = 38,
                Y = 38
            });
            _uiPermanents.Add(new UIRenderInfo()
            {
                RenderInfo = new TileRenderInfo()
                {
                    BackColor = Color.Black,
                    TileColor = Color.White,
                    TileX = 8,
                    TileY = 12
                },
                X = 0,
                Y = 38
            });

            for (int i = 39; i < 65; i++)
            {
                _uiPermanents.Add(new UIRenderInfo()
                {
                    RenderInfo = new TileRenderInfo()
                    {
                        BackColor = Color.Black,
                        TileColor = Color.White,
                        TileX = 13,
                        TileY = 12
                    },
                    X = i,
                    Y = 0
                });
                _uiPermanents.Add(new UIRenderInfo()
                {
                    RenderInfo = new TileRenderInfo()
                    {
                        BackColor = Color.Black,
                        TileColor = Color.White,
                        TileX = 13,
                        TileY = 12
                    },
                    X = i,
                    Y = 38
                });
            }

            //65

        }

        bool showExtras = false;

        double count = 0;
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            var ks = Engine.InputService;

            if (count <= 0)
            {
                bool moved = false;
                if (ks.Keyboard.KeyHeld(Keys.Right) || ks.Keyboard.KeyHeld(Keys.D))
                {
                    _map.MoveActor(player, Map.Direction.Right);
                    moved = true;
                }
                if (ks.Keyboard.KeyHeld(Keys.Left) || ks.Keyboard.KeyHeld(Keys.A))
                {
                    _map.MoveActor(player, Map.Direction.Left);
                    moved = true;
                }
                if (ks.Keyboard.KeyHeld(Keys.Up) || ks.Keyboard.KeyHeld(Keys.W))
                {
                    _map.MoveActor(player, Map.Direction.Up);
                    moved = true;
                }
                if (ks.Keyboard.KeyHeld(Keys.Down) || ks.Keyboard.KeyHeld(Keys.S))
                {
                    _map.MoveActor(player, Map.Direction.Down);
                    moved = true;
                }
                if (ks.Keyboard.KeyHeld(Keys.Q))
                {
                    _map.MoveActor(player, Map.Direction.UpLeft);
                    moved = true;
                }
                if (ks.Keyboard.KeyHeld(Keys.E))
                {
                    _map.MoveActor(player, Map.Direction.UpRight);
                    moved = true;
                }
                if (ks.Keyboard.KeyHeld(Keys.Z))
                {
                    _map.MoveActor(player, Map.Direction.DownLeft);
                    moved = true;
                }
                if (ks.Keyboard.KeyHeld(Keys.C))
                {
                    _map.MoveActor(player, Map.Direction.DownRight);
                    moved = true;
                }

                if (moved)
                {
                    count += 5;
                    _map.Update(player);
                }
            }
            else if (count > 0)
            {
                count -= gameTime.ElapsedGameTime.Milliseconds;
            }

            if (ks.Keyboard.KeyHeld(Keys.LeftAlt))
            {
                showExtras = true;
            }
            else
            {
                showExtras = false;
            }
        }

        List<UIRenderInfo> _uiPermanents = new List<UIRenderInfo>();

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            // TODO: Add your drawing code here
            for (int i = player.X - 18; i < player.X + 19; i++)
            {
                for (int j = player.Y - 18; j < player.Y + 19; j++)
                {
                    if ((i < 0) || (j < 0) || (i >= Map.MapWidth) || (j >= Map.MapHeight)) continue;
                    SpritesheetHelper.RenderTile(_map[i, j], Engine.SpriteBatch, SpriteSheet, new Rectangle((i - player.X + 19) * 12, (j - player.Y + 19) * 12, 12, 12));


                }
            }

            foreach (var item in _uiPermanents)
            {
                SpritesheetHelper.RenderTile(item.RenderInfo, Engine.SpriteBatch, SpriteSheet, new Rectangle(item.X * 12, item.Y * 12, 12, 12));
            }

            var ks = Engine.InputService;
            if (ks.Keyboard.KeyPress(Keys.P))
            {
                Texture2D t2d = new Texture2D(Engine.GraphicsDevice, Map.MapWidth, Map.MapHeight);
                uint[] colordata = new uint[Map.MapWidth * Map.MapHeight];
                for (int i = 0; i < Map.MapWidth; i++)
                {
                    for (int j = 0; j < Map.MapHeight; j++)
                    {
                        var color = _map[i, j].RenderInfo.BackColor;
                        colordata[i + j * Map.MapHeight] = (uint)((color.R << 24) | (color.B << 16) | (color.G << 8) | (color.A << 0));
                    }
                }
                t2d.SetData<uint>(colordata);
                using (var fs = new FileStream(DateTime.Now.ToString("RoCD_test_worldmap_yyyyMMdd-hhmmss") + ".png", FileMode.CreateNew))
                {
                    t2d.SaveAsPng(fs, Map.MapWidth, Map.MapHeight);
                }
            }

            if (showExtras)
            {
                for (int i = player.X - 18; i < player.X + 19; i++)
                {
                    for (int j = player.Y - 18; j < player.Y + 19; j++)
                    {
                        if ((i < 0) || (j < 0) || (i >= Map.MapWidth) || (j >= Map.MapHeight)) continue;
                        if (_map[i, j].Contained != null)
                        {
                            ShowInfo(_map[i, j].Contained, Engine.SpriteBatch, SpriteSheet, new Rectangle((i - player.X + 19) * 12, (j - player.Y + 19) * 12, 12, 12));
                        }
                    }
                }
            }
        }

        private void ShowInfo(Actor actor, SpriteBatch spriteBatch, Texture2D SpriteSheet, Rectangle target)
        {
            for (int i = 0; i < actor.Identity.Length; i++)
            {
                Point tl = GetCharacterPoint(actor.Identity[i]);
                TileRenderInfo bti = new TileRenderInfo { TileColor = Color.Black, TileX = 11, TileY = 13};
                TileRenderInfo ti = new TileRenderInfo { TileColor = Color.White, TileX = (byte)tl.X, TileY = (byte)tl.Y};
                SpritesheetHelper.RenderTile(bti, spriteBatch, _spriteSheet, target);
                SpritesheetHelper.RenderTile(ti, spriteBatch, _spriteSheet, target);
                target = new Rectangle(target.X + 12, target.Y, target.Width, target.Height);
            }
        }

        private Point GetCharacterPoint(char p)
        {
            switch (p)
            {
                case 'a': return new Point(1, 6); break;
                case 'b': return new Point(2, 6); break;
                case 'c': return new Point(3, 6); break;
                case 'd': return new Point(4, 6); break;
                case 'e': return new Point(5, 6); break;
                case 'f': return new Point(6, 6); break;
                case 'g': return new Point(7, 6); break;
                case 'h': return new Point(8, 6); break;
                case 'i': return new Point(9, 6); break;
                case 'j': return new Point(10, 6); break;
                case 'k': return new Point(11, 6); break;
                case 'l': return new Point(12, 6); break;
                case 'm': return new Point(13, 6); break;
                case 'n': return new Point(14, 6); break;
                case 'o': return new Point(15, 6); break;

                case 'p': return new Point(0, 7); break;
                case 'q': return new Point(1, 7); break;
                case 'r': return new Point(2, 7); break;
                case 's': return new Point(3, 7); break;
                case 't': return new Point(4, 7); break;
                case 'u': return new Point(5, 7); break;
                case 'v': return new Point(6, 7); break;
                case 'w': return new Point(7, 7); break;
                case 'x': return new Point(8, 7); break;
                case 'y': return new Point(9, 7); break;
                case 'z': return new Point(10, 7); break;
            }

            return new Point(0, 2);
        }

        public override void PreLoad()
        {

        }

        public override void LoadUpdate(Microsoft.Xna.Framework.GameTime gameTime)
        {

        }

        public override void LoadDraw(Microsoft.Xna.Framework.GameTime gameTime)
        {

        }

        public override void OnPop()
        {

        }
    }
}
