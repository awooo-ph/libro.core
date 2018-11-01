using System.Linq;
using System.Windows.Input;
using Libro.Data;

namespace Libro.Models
{
    public class User : ModelBase<User>
    {

        private bool _isAdmin;

        public bool IsAdmin
        {
            get { return _isAdmin; }
            set
            {
                if (value == _isAdmin) return;
                _isAdmin = value;
                OnPropertyChanged();
            }
        }

        private string _password;

        public string Password
        {
            get { return _password; }
            set
            {
                if (value == _password) return;
                _password = value;
                OnPropertyChanged();
            }
        }

        private string _fullname;

        public string Fullname
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_fullname)) return Username;
                return _fullname;
            }
            set
            {
                if (value == _fullname) return;
                _fullname = value;
                OnPropertyChanged();
            }
        }

        private string _username;

        public string Username
        {
            get { return _username; }
            set
            {
                if (value == _username) return;
                _username = value;
                OnPropertyChanged();
            }
        }

        private static ICommand _resetPasswordCommand;

        public static ICommand ResetPasswordCommand
            => _resetPasswordCommand ?? (_resetPasswordCommand = new DelegateCommand<User>(ResetPassword));

        private static void ResetPassword(User obj)
        {
            obj?.Update("Password","");
        }

        private bool _isCurrentUser;

        [Ignore]
        public bool IsCurrentUser
        {
            get { return _isCurrentUser; }
            set
            {
                if (value == _isCurrentUser) return;
                _isCurrentUser = value;
                OnPropertyChanged();
            }
        }

        protected override bool GetIsEmpty()
        {
            return string.IsNullOrEmpty(Username);
        }

        protected override string GetErrorInfo(string prop)
        {
            if (prop == nameof(Username))
            {
                if (string.IsNullOrWhiteSpace(Username)) return "Username is required.";
                if (Cache.Any(x => x.Username.ToLower() == Username.ToLower() && x.Id != Id))
                    return "Username already exists.";
            }
            if (prop == nameof(IsAdmin) && !IsAdmin)
            {
                if (!Cache.Any(x => x.IsAdmin)) return "At least one (1) admin is required.";
            }
            return base.GetErrorInfo(prop);
        }

        protected override bool GetIsValid()
        {
            if(Cache.Any(x => x.Username?.ToLower() == Username?.ToLower() && x.Id != Id))
                return false;
            if(!IsAdmin && !Cache.Any(x => x.IsAdmin)) return false;

            return base.GetIsValid();
        }
    }
}
