using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPL.Misc.Utils
{
    public static class StringExtension
    {
        public static string ToUnicode(this string str)
        {
            byte[] bytes = Encoding.Default.GetBytes(str);
            return Encoding.GetEncoding(932).GetString(bytes); // 932 is code for Shift-JIS encoding
        }

        public static string ToBCrypt(this string input)
        {
            return BCrypt.Net.BCrypt.HashPassword(input);
        }

        public static string ToTitleCase(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;
            return input.First().ToString().ToUpper() + input.ToLower().Substring(1);
        }
    }
}
