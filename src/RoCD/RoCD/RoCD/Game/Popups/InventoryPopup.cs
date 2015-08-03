using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RapidXNA.Models;
using RoCD.Helpers;
using RoCD.Mechanics;
using RoCD.Mechanics.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoCD.Game.Popups
{
    public class InventoryPopup : GameScreen
    {
        private Helpers.Tiles.Tile tile;

        public InventoryPopup(Helpers.Tiles.Tile tile)
        {
            this.tile = tile;
        }

        private Texture2D spriteSheet { get; set; }
        private SpriteBatch spriteBatch { get; set; }
        private Actor player { get; set; }

        public override void Load()
        {

        }

        public override void Update(GameTime gameTime)
        {
            if (Engine.InputService.Keyboard.KeyPress(Microsoft.Xna.Framework.Input.Keys.Q))
            {
                mystate = InventoryStates.Floor;
            }
            else if (Engine.InputService.Keyboard.KeyPress(Microsoft.Xna.Framework.Input.Keys.W))
            {
                mystate = InventoryStates.Carrying;
            }
            else if (Engine.InputService.Keyboard.KeyPress(Microsoft.Xna.Framework.Input.Keys.E))
            {
                mystate = InventoryStates.Equipped;
            }

            if (mystate == InventoryStates.Floor)
            {
                if (Engine.InputService.Keyboard.KeyPress(Microsoft.Xna.Framework.Input.Keys.Up))
                {
                    _selectedFloorItem = ((_selectedFloorItem - 1) + tile.items.items.Count) % tile.items.items.Count;
                }
                else if (Engine.InputService.Keyboard.KeyPress(Microsoft.Xna.Framework.Input.Keys.Down))
                {
                    _selectedFloorItem = (_selectedFloorItem + 1) % tile.items.items.Count;
                }
            }

            if (Engine.InputService.Keyboard.KeyPress(Microsoft.Xna.Framework.Input.Keys.I))
            {
                Engine.ScreenService.RemovePopup();
            }
        }

        enum InventoryStates
        {
            Floor,
            Carrying,
            Equipped
        }

        InventoryStates mystate = InventoryStates.Floor; //defaults to floor for now

        public override void Draw(GameTime gameTime)
        {
            UIRenderer.RenderEmpty(6, 6, 53, 27, Color.Black);
            UIRenderer.RenderBox(5, 5, 55, 29, Color.Black, Color.White);
            UIRenderer.ShowInfo("Inventory", new Rectangle(6 * 12, 5 * 12, 12, 12), Color.White, Color.Black);
            UIRenderer.RenderVerticalTBar(17, 5, 29, Color.Black, Color.White);

            if (mystate == InventoryStates.Floor)
            {
                UIRenderer.ShowInfo("Floor", new Rectangle(8 * 12, 7 * 12, 12, 12), Color.Black, Color.Yellow);

                RenderFloorItems();
            }
            else
            {
                UIRenderer.ShowInfo("Q", new Rectangle(6 * 12, 7 * 12, 12, 12), Color.Black, Color.Purple);
                UIRenderer.ShowInfo("Floor", new Rectangle(8 * 12, 7 * 12, 12, 12), Color.Black, Color.White);
            }

            if (mystate == InventoryStates.Carrying)
            {
                UIRenderer.ShowInfo("Carried", new Rectangle(8 * 12, 8 * 12, 12, 12), Color.Black, Color.Yellow);
            }
            else
            {
                UIRenderer.ShowInfo("W", new Rectangle(6 * 12, 8 * 12, 12, 12), Color.Black, Color.Purple);
                UIRenderer.ShowInfo("Carried", new Rectangle(8 * 12, 8 * 12, 12, 12), Color.Black, Color.White);
            }

            if (mystate == InventoryStates.Equipped)
            {
                UIRenderer.ShowInfo("Equipped", new Rectangle(8 * 12, 9 * 12, 12, 12), Color.Black, Color.Yellow);
            }
            else
            {
                UIRenderer.ShowInfo("E", new Rectangle(6 * 12, 9 * 12, 12, 12), Color.Black, Color.Purple);
                UIRenderer.ShowInfo("Equipped", new Rectangle(8 * 12, 9 * 12, 12, 12), Color.Black, Color.White);
            }
        }

        int _selectedFloorItem = -1;
        private void RenderFloorItems()
        {
            if ((_selectedFloorItem == -1) && (tile.items.items.Count > 0))
            {
                _selectedFloorItem = 0;
            }
            else if (tile.items.items.Count == 0)
            {
                UIRenderer.ShowInfo("No items on the ground here...", new Rectangle(18 * 12, 7 * 12, 12, 12));
                return;
            }

            for (int i = _selectedFloorItem - 1; i < _selectedFloorItem + 2; i++)
            {
                UIRenderer.ShowInfo(tile.items.items[(i + tile.items.items.Count) % tile.items.items.Count].item.Name, new Rectangle(26 * 12, (7 + (i - _selectedFloorItem)) * 12, 12, 12), Color.Black, (((i + tile.items.items.Count) % tile.items.items.Count) == _selectedFloorItem ? Color.Yellow : Color.Gray));
            }

            UIRenderer.ShowInfo((_selectedFloorItem + 1).ToString() + " of " + tile.items.items.Count.ToString(), new Rectangle(18 * 12, 7 * 12, 12, 12), Color.Black, Color.Gray);

            //Render the rest of the information for the item
            var number = tile.items.items[_selectedFloorItem].Count;
            var item = tile.items.items[_selectedFloorItem].item;
            if (item is Weapon)
            {
                UIRenderer.ShowInfo(number.ToString() + " * " + item.Name, new Rectangle(19 * 12, 11 * 12, 12, 12), Color.Black, Color.Red);
                UIRenderer.ShowInfo("WeaponATK: " + (item as Weapon).WeaponATK.ToString() + (true ? "" : ""), new Rectangle(20 * 12, 12 * 12, 12, 12)); //TODO: show increase/decrease from current weapon equipped
                UIRenderer.ShowInfo("Range: Melee", new Rectangle(20 * 12, 13 * 12, 12, 12));

                UIRenderer.ShowInfo("This is a description.", new Rectangle(20 * 12, 15 * 12, 12, 12), Color.Black, Color.Gray);
            }

            UIRenderer.ShowInfo("(a: pickall) (p: pickselected) (up/down: select)", new Rectangle(9 * 12, 39 * 12, 12, 12), Color.Black, Color.Gray);
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
