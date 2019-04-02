using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RensvikSamfunnshus.Utility
{
    public static class RandomHelper
    {
        private static Random random;
        private static object syncLock;
        public static int RandomNumber(int min, int max)
        {
            if (random == null)
                random = new Random();
            if (syncLock == null)
                syncLock = new object();

            lock (syncLock)
            {
                return random.Next(min, max);
            }
        }
        public static int RandomNumber(int max = 696969)
        {
            if (random == null)
                random = new Random();
            if (syncLock == null)
                syncLock = new object();

            lock (syncLock)
            {
                return random.Next(0, max);
            }
        }
    }
}