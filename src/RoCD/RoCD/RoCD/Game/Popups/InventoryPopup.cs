using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RapidXNA.Models;
using RoCD.Mechanics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoCD.Game.Popups
{
    public class InventoryPopup : GameScreen
    {
        public InventoryPopup(Texture2D _spritesheet, SpriteBatch _spritebatch, Actor _player)
        {
            spriteSheet = _spritesheet;
            spriteBatch = _spritebatch;
            player = _player;
        }

        private Texture2D spriteSheet { get; set; }
        private SpriteBatch spriteBatch { get; set; }
        private Actor player { get; set; }

        public override void Load()
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(GameTime gameTime)
        {
            
        }

        public override void PreLoad()
        {
            
        }

        public override void LoadUpdate(GameTime gameTime)
        {
            
        }

        public override void LoadDraw(GameTime gameTime)
        {
            
        }

        public override void OnPop()
        {
            
        }
    }
}
