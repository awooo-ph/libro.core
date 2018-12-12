using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Libro.Data;

namespace Libro.Models
{
    public class DailyUsage:ModelBase<DailyUsage>
    {
        private long _BookId;

        public long BookId
        {
            get => _BookId;
            set
            {
                if (value == _BookId) return;
                _BookId = value;
                OnPropertyChanged(nameof(BookId));
            }
        }

        private long _Elementary;

        public long Elementary
        {
            get => _Elementary;
            set
            {
                if (value == _Elementary) return;
                _Elementary = value;
                OnPropertyChanged(nameof(Elementary));
                OnPropertyChanged(nameof(Total));
                OnPropertyChanged(nameof(Students));
            }
        }

        private long _HighSchool;

        public long HighSchool
        {
            get => _HighSchool;
            set
            {
                if (value == _HighSchool) return;
                _HighSchool = value;
                OnPropertyChanged(nameof(HighSchool));
                OnPropertyChanged(nameof(Total));
                OnPropertyChanged(nameof(Students));
            }
        }

        private long _College;

        public long College
        {
            get => _College;
            set
            {
                if (value == _College) return;
                _College = value;
                OnPropertyChanged(nameof(College));
                OnPropertyChanged(nameof(Total));
                OnPropertyChanged(nameof(Students));
            }
        }

        public long Total => Students + Faculty;

        private long _Faculty;

        public long Faculty
        {
            get => _Faculty;
            set
            {
                if (value == _Faculty) return;
                _Faculty = value;
                OnPropertyChanged(nameof(Faculty));
                OnPropertyChanged(nameof(Total));
            }
        }

        [Ignore] public long Students => Elementary + HighSchool + College;

        private DateTime _Date = DateTime.Now;

        public DateTime Date
        {
            get => _Date;
            set
            {
                if (value == _Date) return;
                _Date = value;
                OnPropertyChanged(nameof(Date));
            }
        }


        public static DailyUsage GetByDate(DateTime date = default)
        {
            if (date == default) date = DateTime.Now;
            var usage = Cache.FirstOrDefault(x => x.Date.Date == date.Date);
            if(usage == null) usage = new DailyUsage()
            {
                Date = date
            };
            return usage;
        }

        public void Increment(Departments department)
        {
            switch (department)
            {
                case Departments.Elementary:
                    Elementary++;
                    break;
                case Departments.HighSchool:
                    HighSchool++;
                    break;
                case Departments.College:
                    College++;
                    break;
                case Departments.Faculty:
                    Faculty++;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(department), department, null);
            }

            Save();
        }
        
        protected override bool GetIsEmpty()
        {
            return false;
        }
    }
}
