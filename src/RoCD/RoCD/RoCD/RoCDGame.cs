using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using RoCD.Helpers;
using RoCD.Helpers.Tiles;
using RoCD.Mechanics;

namespace RoCD
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class RoCDGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public RoCDGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        private Texture2D _spriteSheet;

        public Texture2D SpriteSheet
        {
            get { return _spriteSheet; }
        }

        Actor player = new Actor() { X = 19, Y = 19 };
        Map _map = new Map();
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _spriteSheet = Content.Load<Texture2D>("12x12");

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

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            KeyboardState ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.Right) || ks.IsKeyDown(Keys.D))
            {
                _map.MoveActor(player, Map.Direction.Right);
            }
            if (ks.IsKeyDown(Keys.Left) || ks.IsKeyDown(Keys.A))
            {
                _map.MoveActor(player, Map.Direction.Left);
            }
            if (ks.IsKeyDown(Keys.Up) || ks.IsKeyDown(Keys.W))
            {
                _map.MoveActor(player, Map.Direction.Up);
            }
            if (ks.IsKeyDown(Keys.Down) || ks.IsKeyDown(Keys.S))
            {
                _map.MoveActor(player, Map.Direction.Down);
            }
            if (ks.IsKeyDown(Keys.Q))
            {
                _map.MoveActor(player, Map.Direction.UpLeft);
            }
            if (ks.IsKeyDown(Keys.E))
            {
                _map.MoveActor(player, Map.Direction.UpRight);
            }
            if (ks.IsKeyDown(Keys.Z))
            {
                _map.MoveActor(player, Map.Direction.DownLeft);
            }
            if (ks.IsKeyDown(Keys.C))
            {
                _map.MoveActor(player, Map.Direction.DownRight);
            }

            base.Update(gameTime);
        }

        List<UIRenderInfo> _uiPermanents = new List<UIRenderInfo>();

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            // TODO: Add your drawing code here
            for (int i = player.X - 18; i < player.X + 19; i++)
            {
                for (int j = player.Y - 18; j < player.Y + 19; j++)
                {
                    if ((i < 0) || (j < 0) || (i >= 200) || (j >= 200)) continue;
                    SpritesheetHelper.RenderTile(_map[i, j], spriteBatch, SpriteSheet, new Rectangle((i - player.X + 19) * 12, (j - player.Y + 19) * 12, 12, 12));
                }
            }

            foreach (var item in _uiPermanents)
            {
                SpritesheetHelper.RenderTile(item.RenderInfo, spriteBatch, SpriteSheet, new Rectangle(item.X * 12, item.Y * 12, 12, 12));
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
