using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoCD.Mechanics.Items
{
    public class ItemStack
    {
        public Item item;

        public int Count = 1;

        public double Weight { get { return Count * item.Weight; } }
    }
}
