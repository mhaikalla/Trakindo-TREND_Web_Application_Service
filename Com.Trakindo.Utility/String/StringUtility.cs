using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Trakindo.Utility
{
    public class StringUtility
    {
        public static string GenerateRandom(int maxLength)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[maxLength];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new System.String(stringChars);
            return finalString;
        }
    }
}
