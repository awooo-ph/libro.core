
namespace Libro.Models
{
    public class Author : ModelBase<Author>
    {
        private string _firstname;
        
        public string Fullname
        {
            get
            {
                var fn = Firstname;
                if (!string.IsNullOrEmpty(Middlename)) fn += " " + Middlename[0] + ".";
                fn += " " + Lastname;
                return fn.Trim();
            }
        }

        public string InvertedName
        {
            get
            {
                var fn = Lastname;
                if (!string.IsNullOrEmpty(Firstname))
                {
                    fn += ", " + Firstname;
                    if (!string.IsNullOrEmpty(Middlename))
                        fn += $" {Middlename[0]}.";
                }
                return fn.Trim();
            }
        }

        public string Firstname
        {
            get { return _firstname; }
            set
            {
                if (value == _firstname) return;
                _firstname = value;
                OnPropertyChanged();
            }
        }

        private string _lastname;

        public string Lastname
        {
            get { return _lastname; }
            set
            {
                if (value == _lastname) return;
                _lastname = value;
                OnPropertyChanged();
            }
        }

        private string _middlename;

        public string Middlename
        {
            get { return _middlename; }
            set
            {
                if (value == _middlename) return;
                _middlename = value;
                OnPropertyChanged();
            }
        }

        private string _code;

        public string Code
        {
            get { return _code; }
            set
            {
                if (value == _code) return;
                _code = value;
                OnPropertyChanged();
            }
        }

        protected override bool GetIsEmpty()
        {
            return false;
        }
    }
}
