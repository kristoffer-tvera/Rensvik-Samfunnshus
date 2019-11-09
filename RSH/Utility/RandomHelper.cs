using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace RSH.Utility
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

        public static string RandomString(int length, string chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ")
        {
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                var data = new byte[length];
                byte[] buffer = null;

                var maxRandom = byte.MaxValue - ((byte.MaxValue + 1) % chars.Length);

                crypto.GetBytes(data);

                var result = new char[length];

                for (int i = 0; i < length; i++)
                {
                    byte value = data[i];

                    while (value > maxRandom)
                    {
                        if (buffer == null)
                        {
                            buffer = new byte[1];
                        }

                        crypto.GetBytes(buffer);
                        value = buffer[0];
                    }

                    result[i] = chars[value % chars.Length];
                }

                return new string(result);
            }
        }
    }
}