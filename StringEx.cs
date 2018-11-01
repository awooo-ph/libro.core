using System.Linq;

namespace Libro
{
    public static class StringEx
    {
        public static string StripNonNumeric(this string s)
        {
            const string numbers = "0123456789";
            var n = "";
            for (var i = 0; i < s.Length; i++)
            {
                if (numbers.Contains(s[i]))
                    n += s[i];
            }
            return n;
        }
    }
}
