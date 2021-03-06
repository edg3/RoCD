﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoCD.Mechanics.Items
{
    public class Inventory
    {
        public List<ItemStack> items = new List<ItemStack>();

        public double totalWeight()
        {
            double weight = 0.0;

            foreach (var item in items)
            {
                weight += item.Weight;
            }

            return weight;
        }
    }
}
