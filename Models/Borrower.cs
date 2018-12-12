using System.Collections.Generic;
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

        public bool HasCourse => DepartmentType == Departments.College;

        private Departments _DepartmentType;

        public Departments DepartmentType
        {
            get => _DepartmentType;
            set
            {
                if (value == _DepartmentType) return;
                _DepartmentType = value;
                OnPropertyChanged(nameof(DepartmentType));
                OnPropertyChanged(nameof(IsStudent));
                OnPropertyChanged(nameof(HasCourse));
                OnPropertyChanged(nameof(Course));
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
            get => HasCourse ? _course : DepartmentType.ToString();
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

        private bool _isStudent = true;
        /// <summary>
        /// Whether this borrower is a student or a faculty member.
        /// </summary>
        public bool IsStudent => DepartmentType!=Departments.Faculty;

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
                    return string.IsNullOrEmpty(Course) && HasCourse ? "Course is required." : null;
            }
            return base.GetErrorInfo(prop);
        }

        protected override bool GetIsEmpty()
        {
            return string.IsNullOrEmpty(Firstname) || string.IsNullOrEmpty(Lastname) || (string.IsNullOrEmpty(Course) && HasCourse) || string.IsNullOrEmpty(Barcode);
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

        public static List<(string Title, long Usage)> GetTopList(long count = 10)
        {
            if (count == 0) count = 10;
            var list = new List<(string Isbn, long Usage)>();
            var c = Db.Connection.CreateCommand();
            c.CommandText = $@"
SELECT 
    Borrowers.Id AS Id, 
    Borrowers.Firstname || "" "" || Borrowers.Lastname AS Title, 
    COUNT(*) AS Usage 
FROM Takeouts 
JOIN Borrowers ON Takeouts.BorrowerId=Borrowers.Id
GROUP BY Borrowers.Id
ORDER BY Usage DESC
LIMIT 0, {count};";
            var r = c.ExecuteReader();
            while (r.Read())
            {
                long.TryParse(r["Usage"].ToString(), out var usage);
                list.Add((r["Title"].ToString(), usage));
            }

            return list;
        }
    }
}
