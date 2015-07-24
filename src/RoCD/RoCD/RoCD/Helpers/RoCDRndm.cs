using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoCD.Helpers
{
    public class RoCDRndm
    {
        private static Random rndm = new Random();

        public static int Next()
        {
            return rndm.Next();
        }

        public static int Next(int upper)
        {
            return rndm.Next(upper);
        }

        public static double NextDouble()
        {
            return rndm.NextDouble();
        }

        public static int Next(int minValue, int maxValue)
        {
            return rndm.Next(minValue, maxValue);
        }
    }
}
