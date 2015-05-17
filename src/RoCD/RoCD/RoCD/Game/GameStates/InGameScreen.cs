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

namespace RoCD.Game.GameStates
{
    public class InGameScreen : GameScreen
    {
        private Texture2D _spriteSheet;

        public Texture2D SpriteSheet
        {
            get { return _spriteSheet; }
        }

        Actor player = new Actor() { X = 19, Y = 19 };
        Map _map = new Map();

        public override void Load()
        {
            Engine.ScreenService.DrawEnabled = true;

            _spriteSheet = Engine.ContentManager.Load<Texture2D>("12x12");

            _map[19, 19].Contained = player;

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
            }
            _uiPermanents.Add(new UIRenderInfo()
            {
                RenderInfo = new TileRenderInfo()
                {
                    BackColor = Color.Black,
                    TileColor = Color.White,
                    TileX = 11,
                    TileY = 11
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
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            var ks = Engine.InputService;
            if (ks.Keyboard.KeyPress(Keys.Right) || ks.Keyboard.KeyPress(Keys.D))
            {
                _map.MoveActor(player, Map.Direction.Right);
            }
            if (ks.Keyboard.KeyPress(Keys.Left) || ks.Keyboard.KeyPress(Keys.A))
            {
                _map.MoveActor(player, Map.Direction.Left);
            }
            if (ks.Keyboard.KeyPress(Keys.Up) || ks.Keyboard.KeyPress(Keys.W))
            {
                _map.MoveActor(player, Map.Direction.Up);
            }
            if (ks.Keyboard.KeyPress(Keys.Down) || ks.Keyboard.KeyPress(Keys.S))
            {
                _map.MoveActor(player, Map.Direction.Down);
            }
            if (ks.Keyboard.KeyPress(Keys.Q))
            {
                _map.MoveActor(player, Map.Direction.UpLeft);
            }
            if (ks.Keyboard.KeyPress(Keys.E))
            {
                _map.MoveActor(player, Map.Direction.UpRight);
            }
            if (ks.Keyboard.KeyPress(Keys.Z))
            {
                _map.MoveActor(player, Map.Direction.DownLeft);
            }
            if (ks.Keyboard.KeyPress(Keys.C))
            {
                _map.MoveActor(player, Map.Direction.DownRight);
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
                    if ((i < 0) || (j < 0) || (i >= 200) || (j >= 200)) continue;
                    SpritesheetHelper.RenderTile(_map[i, j], Engine.SpriteBatch, SpriteSheet, new Rectangle((i - player.X + 19) * 12, (j - player.Y + 19) * 12, 12, 12));
                }
            }

            foreach (var item in _uiPermanents)
            {
                SpritesheetHelper.RenderTile(item.RenderInfo, Engine.SpriteBatch, SpriteSheet, new Rectangle(item.X * 12, item.Y * 12, 12, 12));
            }
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
