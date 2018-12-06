using System.Linq;
using System.Text;

namespace Libro
{
    public static class StringEx
    {
        public static string StripNonNumeric(this string s)
        {
            const string numbers = "0123456789";
            var sb = new StringBuilder();
            foreach (var t in s)
            {
                if (numbers.Contains(t))
                    sb.Append(t);
            }
            return sb.ToString();
        }
    }
}
