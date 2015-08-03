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
using RoCD.Mechanics.AI.Agents;
using RoCD.Mechanics.Items;

namespace RoCD.Game.GameStates
{
    public class InGameScreen : GameScreen
    {
        private Texture2D _spriteSheet;

        public Texture2D SpriteSheet
        {
            get { return _spriteSheet; }
        }

        Creature player = new Creature() { X = 19, Y = 19, Identity = "you" };
        Map _map = new Map();

        public override void Load()
        {
            Engine.ScreenService.DrawEnabled = true;

            _spriteSheet = Engine.ContentManager.Load<Texture2D>("12x12");

            SpritesheetHelper.Init(Engine.SpriteBatch, _spriteSheet);

            var pnt = _map._cities[0];

            player.X = (int)(pnt.X);
            player.Y = (int)(pnt.Y);
            _map[(int)(pnt.X), (int)(pnt.Y)].Contained = player;

            var istack = new ItemStack();
            istack.item = new Weapon() { Name = "Dagger", WeaponATK = 5 };
            _map[(int)(pnt.X) - 3, (int)(pnt.Y)].items.items.Add(istack);
        }

        bool showExtras = false;

        double count = 0;
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            var ks = Engine.InputService;

            bool can_statup = (player.stat_buypoints > 0); //ignores cost
            if (can_statup)
            {
                if (ks.Keyboard.KeyPress(Keys.D1))
                {
                    player.increaseStat(Creature.INT);
                    player.updateSecondary_INT();
                }
                else if (ks.Keyboard.KeyPress(Keys.D2))
                {
                    player.increaseStat(Creature.DEX);
                    player.updateSecondary_DEX();
                }
                else if (ks.Keyboard.KeyPress(Keys.D3))
                {
                    player.increaseStat(Creature.VIT);
                    player.updateSecondary_VIT();
                }
                else if (ks.Keyboard.KeyPress(Keys.D4))
                {
                    player.increaseStat(Creature.AGI);
                    player.updateSecondary_AGI();
                }
                else if (ks.Keyboard.KeyPress(Keys.D5))
                {
                    player.increaseStat(Creature.STR);
                    player.updateSecondary_STR();
                }
            }

            if (count <= 0)
            {
                bool moved = false;
                Actor atPos = null;
                if (ks.Keyboard.KeyHeld(Keys.Right) || ks.Keyboard.KeyHeld(Keys.D))
                {
                    atPos = _map.MoveActor(player, Map.Direction.Right);
                    moved = true;
                }
                if (ks.Keyboard.KeyHeld(Keys.Left) || ks.Keyboard.KeyHeld(Keys.A))
                {
                    atPos = _map.MoveActor(player, Map.Direction.Left);
                    moved = true;
                }
                if (ks.Keyboard.KeyHeld(Keys.Up) || ks.Keyboard.KeyHeld(Keys.W))
                {
                    atPos = _map.MoveActor(player, Map.Direction.Up);
                    moved = true;
                }
                if (ks.Keyboard.KeyHeld(Keys.Down) || ks.Keyboard.KeyHeld(Keys.S))
                {
                    atPos = _map.MoveActor(player, Map.Direction.Down);
                    moved = true;
                }
                if (ks.Keyboard.KeyHeld(Keys.Q))
                {
                    atPos = _map.MoveActor(player, Map.Direction.UpLeft);
                    moved = true;
                }
                if (ks.Keyboard.KeyHeld(Keys.E))
                {
                    atPos = _map.MoveActor(player, Map.Direction.UpRight);
                    moved = true;
                }
                if (ks.Keyboard.KeyHeld(Keys.Z))
                {
                    atPos = _map.MoveActor(player, Map.Direction.DownLeft);
                    moved = true;
                }
                if (ks.Keyboard.KeyHeld(Keys.C))
                {
                    atPos = _map.MoveActor(player, Map.Direction.DownRight);
                    moved = true;
                }

                if (moved)
                {
                    count += 128;
                    if (null != atPos)
                    {
                        if (atPos is Creature)
                        {
                            player.meleeAttack(atPos as Creature);
                            ((atPos as Creature).AIAgent as PassiveEnemyAgent).agressive = true;
                        }
                    }
                    _map.Update(player);
                    player.regenerate();
                }
            }
            else if (count > 0)
            {
                count -= gameTime.ElapsedGameTime.Milliseconds;
            }

            if (ks.Keyboard.KeyPress(Keys.R))
            {
                int playerHealth = player.get(Creature.CURRHP);
                while (((player.get(Creature.CURRHP) < player.get(Creature.MAXHP)) || (player.get(Creature.CURRSP) < player.get(Creature.MAXSP))) && (playerHealth <= player.get(Creature.CURRHP)))
                {
                    playerHealth = player.get(Creature.CURRHP);
                    player.regenerate();
                    _map.Update(player);
                }
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

        double altDraw = 0.0;
        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            altDraw = (altDraw + gameTime.ElapsedGameTime.Milliseconds / 666.0) % 2;

            // TODO: Add your drawing code here
            for (int i = player.X - 18; i < player.X + 19; i++)
            {
                for (int j = player.Y - 18; j < player.Y + 19; j++)
                {
                    if ((i < 0) || (j < 0) || (i >= Map.MapWidth) || (j >= Map.MapHeight)) continue;
                    if (_map[i,j].items.items.Count == 0 || altDraw < 1)
                    {
                        SpritesheetHelper.RenderTile(_map[i, j], new Rectangle((i - player.X + 19) * 12, (j - player.Y + 19) * 12, 12, 12));
                    }
                    else
                    {
                        SpritesheetHelper.RenderTileAlt(_map[i, j], new Rectangle((i - player.X + 19) * 12, (j - player.Y + 19) * 12, 12, 12));
                    }
                }
            }

            UIRenderer.RenderBox(0, 0, 65, 38, Color.Black, Color.Gray);
            UIRenderer.RenderVerticalTBar(38, 0, 38, Color.Black, Color.Gray);

            //Debug map Screenshot
            //var ks = Engine.InputService;
            //if (ks.Keyboard.KeyPress(Keys.P))
            //{
            //    Texture2D t2d = new Texture2D(Engine.GraphicsDevice, Map.MapWidth, Map.MapHeight);
            //    uint[] colordata = new uint[Map.MapWidth * Map.MapHeight];
            //    for (int i = 0; i < Map.MapWidth; i++)
            //    {
            //        for (int j = 0; j < Map.MapHeight; j++)
            //        {
            //            var color = _map[i, j].RenderInfo.BackColor;
            //            colordata[i + j * Map.MapHeight] = (uint)((color.R << 24) | (color.B << 16) | (color.G << 8) | (color.A << 0));
            //        }
            //    }
            //    t2d.SetData<uint>(colordata);
            //    using (var fs = new FileStream(DateTime.Now.ToString("RoCD_test_worldmap_yyyyMMdd-hhmmss") + ".png", FileMode.CreateNew))
            //    {
            //        t2d.SaveAsPng(fs, Map.MapWidth, Map.MapHeight);
            //    }
            //}

            if (showExtras)
            {
                for (int i = player.X - 18; i < player.X + 19; i++)
                {
                    for (int j = player.Y - 18; j < player.Y + 19; j++)
                    {
                        if ((i < 0) || (j < 0) || (i >= Map.MapWidth) || (j >= Map.MapHeight)) continue;
                        if (_map[i, j].Contained != null)
                        {
                            ShowInfo(_map[i, j].Contained, new Rectangle((i - player.X + 19) * 12, (j - player.Y + 19) * 12, 12, 12));
                        }
                    }
                }
            }

            //render log
            var lst = CombatLog.Get();
            if (lst.Count > 0)
            {
                for (int i = 0; i < lst.Count; i++)
                {
                    //X 39, Y 32
                    UIRenderer.ShowInfo(lst[i], new Rectangle((39) * 12, (37 - i) * 12, 12, 12));
                }
            }

            UIRenderer.ShowInfo("m." + ((int)(Engine.InputService.Mouse.Position().X / 12)).ToString() + "." + ((int)(Engine.InputService.Mouse.Position().Y / 12)).ToString(), new Rectangle(0, 39 * 12, 12, 12));

            bool can_statup = (player.stat_buypoints > 0); //ignores cost

            UIRenderer.ShowInfo("HP: " + player.get(Creature.CURRHP).ToString() + "/" + player.get(Creature.MAXHP), new Rectangle(12 * 39, 12 * 1, 12, 12), Color.Black, Color.Red);
            UIRenderer.ShowInfo("SP: " + player.get(Creature.CURRSP).ToString() + "/" + player.get(Creature.MAXSP), new Rectangle(12 * 39, 12 * 2, 12, 12), Color.Black, Color.Blue);

            UIRenderer.ShowInfo((can_statup ? "1" : " ") + " INT " + player.get(Creature.INT).ToString(), new Rectangle(12 * 39, 12 * 4, 12, 12));
            UIRenderer.ShowInfo((can_statup ? "2" : " ") + " DEX " + player.get(Creature.DEX).ToString(), new Rectangle(12 * 39, 12 * 5, 12, 12));
            UIRenderer.ShowInfo((can_statup ? "3" : " ") + " VIT " + player.get(Creature.VIT).ToString(), new Rectangle(12 * 39, 12 * 6, 12, 12));
            UIRenderer.ShowInfo((can_statup ? "4" : " ") + " AGI " + player.get(Creature.AGI).ToString(), new Rectangle(12 * 39, 12 * 7, 12, 12));
            UIRenderer.ShowInfo((can_statup ? "5" : " ") + " STR " + player.get(Creature.STR).ToString(), new Rectangle(12 * 39, 12 * 8, 12, 12));
        }

        private void ShowInfo(Actor actor, Rectangle target)
        {
            UIRenderer.ShowInfo(actor.Identity, target);
            //for (int i = 0; i < actor.Identity.Length; i++)
            //{
            //    Point tl = GetCharacterPoint(actor.Identity[i]);
            //    TileRenderInfo bti = new TileRenderInfo { TileColor = Color.Black, TileX = 11, TileY = 13 };
            //    TileRenderInfo ti = new TileRenderInfo { TileColor = Color.White, TileX = (byte)tl.X, TileY = (byte)tl.Y };
            //    SpritesheetHelper.RenderTile(bti, spriteBatch, _spriteSheet, target);
            //    SpritesheetHelper.RenderTile(ti, spriteBatch, _spriteSheet, target);
            //    target = new Rectangle(target.X + 12, target.Y, target.Width, target.Height);
            //}
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
