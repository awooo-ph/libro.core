using System.Linq;
using Libro.Data;

namespace Libro.Models
{
    public class Borrower : ModelBase<Borrower>
    {

        private string _contactNumber="";

        public string ContactNumber
        {
            get { return _contactNumber; }
            set
            {
                if(value == _contactNumber)
                    return;
                _contactNumber = value;
                OnPropertyChanged();
            }
        }

        private string _facebook="";

        public string Facebook
        {
            get { return _facebook; }
            set
            {
                if (value == _facebook) return;
                _facebook = value;
                OnPropertyChanged();
            }
        }

        private string _norsuId="";
        
        public string SchoolId
        {
            get { return _norsuId; }
            set
            {
                if (value == _norsuId) return;
                _norsuId = value;
                OnPropertyChanged();
            }
        }

        private string _course = "";

        public string Course
        {
            get { return _course; }
            set
            {
                if (value == _course) return;
                _course = value;
                OnPropertyChanged();
            }
        }

        private string _lastname="";

        public string Lastname
        {
            get { return _lastname; }
            set
            {
                if (value == _lastname) return;
                _lastname = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Fullname));
            }
        }

        private string _firstname="";

        public string Firstname
        {
            get { return _firstname; }
            set
            {
                if (value == _firstname) return;
                _firstname = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Fullname));
            }
        }

        [Ignore]
        public string Fullname => $"{Lastname}, {Firstname}";

        private bool _isStudent;
        /// <summary>
        /// Whether this borrower is a student or a faculty member.
        /// </summary>
        public bool IsStudent
        {
            get { return _isStudent; }
            set
            {
                if (value == _isStudent) return;
                _isStudent = value;
                OnPropertyChanged();
            }
        }

        [Ignore]
        public bool IsNew => Id==0;

        public override bool Equals(object obj)
        {
            if (!(obj is Borrower b)) return false;
            return b.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        protected override string GetErrorInfo(string prop)
        {
            switch (prop)
            {
                case nameof(Barcode):
                    return GetLibraryIdError();
                case nameof(SchoolId):
                    return GetSchoolIdError();
                case nameof(Firstname):
                    return string.IsNullOrEmpty(Firstname) ? "Firstname is required." : null;
                case nameof(Lastname):
                    return string.IsNullOrEmpty(Lastname) ? "Lastname is required." : null;
                case nameof(Course):
                    return string.IsNullOrEmpty(Course) && IsStudent ? "Course is required." : null;
            }
            return base.GetErrorInfo(prop);
        }

        protected override bool GetIsEmpty()
        {
            return string.IsNullOrEmpty(Firstname) || string.IsNullOrEmpty(Lastname) || (string.IsNullOrEmpty(Course) && IsStudent) || string.IsNullOrEmpty(Barcode);
        }

        private string _barcode="";

        public string Barcode
        {
            get { return _barcode; }
            set
            {
                if (value == _barcode) return;
                _barcode = value;
                OnPropertyChanged();
            }
        }

        protected override bool GetIsValid()
        {
            if (GetLibraryIdError()!=null) return false;
            if (GetSchoolIdError() != null) return false;
            if (string.IsNullOrEmpty(Firstname)) return false;
                if(string.IsNullOrEmpty(Lastname)) return false;
                if(string.IsNullOrEmpty(Course) && IsStudent) return false;
            return true;
        }

        private string GetLibraryIdError()
        {
            if(string.IsNullOrEmpty(SchoolId))
                return "Library ID is required.";
            return Cache.Any(x=>x.Barcode == Barcode && x.Id!=Id) ? "Library Id exists." : null;
        }

        private string GetSchoolIdError()
        {
            if (string.IsNullOrEmpty(SchoolId)) return "School ID is required.";
            return Cache.Any(x=>x.SchoolId==SchoolId && x.Id!=Id) ? "School Id exists." : null;
        }

        public static Borrower GetById(long borrowerId)
        {
            return Db.GetById<Borrower>(borrowerId);
        }
    }
}
