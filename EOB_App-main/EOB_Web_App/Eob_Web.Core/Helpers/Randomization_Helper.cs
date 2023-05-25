using System;

namespace Eob_Web.Frontend.Helpers
{
    public static class Randomization_Helper
    {
        const string RND_CHARS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public static string GenerateRandomString(int count)
        {
            Random rnd = new Random();
            string finalStr = "";
            for (int i = 0; i < count; i++)
            {
                finalStr += RND_CHARS[rnd.Next(RND_CHARS.Length)];

            }
            return finalStr;
        }
    }
}
