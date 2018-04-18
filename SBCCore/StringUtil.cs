using System;
using System.Collections.Generic;
using System.Text;
using Blake2Sharp;

namespace NoobChain
{
    static class StringUtil
    {
        public static string ApplyBlacke2(this string data)
        {
            var p = Blake2B.ComputeHash(System.Text.Encoding.UTF8.GetBytes(data));
            return Convert.ToBase64String(p);
        }
    }
}
