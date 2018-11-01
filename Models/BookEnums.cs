using System.Collections.Generic;

namespace Libro.Models
{
    public static class BookEnums
    {
        private static List<string> _categories;
        public static List<string> Categories
        {
            get
            {
                if (_categories != null) return _categories;
                _categories = new List<string>()
                {
                    "Arts","Computers","Bibliographies","Library",
                };
                return _categories;
            }
        }
    }
}
