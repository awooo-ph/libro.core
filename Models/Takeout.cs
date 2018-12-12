using System;
using System.Collections.Generic;
using System.Linq;
using Libro.Data;

namespace Libro.Models
{
    /// <summary>
    /// Takeout represents a book in a Transaction.
    /// </summary>
    public class Takeout : ModelBase<Takeout>, IEquatable<Takeout>
    {
        /// <summary>
        /// The Id of the book.
        /// </summary>
        public long BookId { get; set; }

        public long BorrowerId { get; set; }

        [Ignore] public Borrower Borrower => Models.Borrower.Cache.FirstOrDefault(x=>x.Id == BorrowerId);

        [Ignore] public Book Book => Book.Cache.FirstOrDefault(x => x.Id == BookId);
        
        private bool _isReturned;
        /// <summary>
        /// Whether this book has been returned.
        /// </summary>
        public bool IsReturned
        {
            get { return _isReturned; }
            set
            {
                if (value == _isReturned) return;
                _isReturned = value;
                OnPropertyChanged();
            }
        }
        
        private string _returnNote;
        /// <summary>
        /// Notes or remarks when the book was returned.
        /// </summary>
        public string ReturnNote
        {
            get { return _returnNote; }
            set
            {
                if (value == _returnNote) return;
                _returnNote = value;
                OnPropertyChanged();
            }
        }

        private string _returnCondition;
        /// <summary>
        /// The condition of the book upon return.
        /// </summary>
        public string ReturnCondition
        {
            get { return _returnCondition; }
            set
            {
                if (value == _returnCondition) return;
                _returnCondition = value;
                OnPropertyChanged();
            }
        }

        private DateTime _returned;
        /// <summary>
        /// The time and date the book was returned.
        /// </summary>
        public DateTime Returned
        {
            get
            {
                if (_returned ==default(DateTime)) return TakeoutDate;
                return _returned;
            }
            set
            {
                if (value.Equals(_returned)) return;
                _returned = value;
                OnPropertyChanged();
            }
        }

        private string _takeOutNotes;
        /// <summary>
        /// Takeout notes.
        /// </summary>
        public string TakeOutNote
        {
            get { return _takeOutNotes; }
            set
            {
                if (value == _takeOutNotes) return;
                _takeOutNotes = value;
                OnPropertyChanged();
            }
        }

        private string _takeOutCondition;
        /// <summary>
        /// Condition of the book when before take out.
        /// </summary>
        public string TakeOutCondition
        {
            get { return _takeOutCondition; }
            set
            {
                if (value == _takeOutCondition) return;
                _takeOutCondition = value;
                OnPropertyChanged();
            }
        }
        
        private DateTime _takeoutDate;

        public DateTime TakeoutDate
        {
            get
            {
                if (_takeoutDate == DateTime.MinValue) _takeoutDate = DateTime.Now;
                return _takeoutDate;
            }
            set
            {
                if (value.Equals(_takeoutDate)) return;
                _takeoutDate = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Returned));
            }
        }

        private int? _bookRating;

        public Takeout()
        {
            TakeoutDate = DateTime.Now;
        }

        public int? BookRating
        {
            get { return _bookRating; }
            set
            {
                if (value == _bookRating) return;
                _bookRating = value;
                OnPropertyChanged();
            }
        }

        [Ignore]
        public bool HasPenalty => Penalty > 0;

        private bool _paid;

        public bool Paid
        {
            get { return _paid; }
            set
            {
                if (value == _paid) return;
                _paid = value;
                OnPropertyChanged();
            }
        }

        private double _penalty;

        public double Penalty
        {
            get { return _penalty; }
            set
            {
                if (value.Equals(_penalty)) return;
                _penalty = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasPenalty));
            }
        }

        private DateTime _returnTime;

        [Ignore]
        public DateTime ReturnTime
        {
            get
            {
                if(_returnTime == DateTime.MinValue) _returnTime = DateTime.Now;
                return _returnTime;
            }
            set
            {
                if (value.Equals(_returnTime)) return;
                _returnTime = value;
                OnPropertyChanged();
               
            }
        }

        public long UserId { get; set; }


        /// <summary>
        /// Save changes to this Takeout
        /// </summary>
        public override void Save()
        {
            if (IsReturned)
            {
                if (DateTime.TryParse($"{Returned:d} {ReturnTime:HH:mm:ss tt}", out var date))
                {
                    Returned = date;
                }
            }
            else
            {
                Returned = default;
            }

            var insert = Id == 0;
            base.Save();
            if(insert) DailyUsage.GetByDate(TakeoutDate.Date).Increment(Borrower.DepartmentType);
        }
        
        protected override bool GetIsEmpty()
        {
            return false;
        }

        public static List<Takeout> GetUnreturnedByBook(long? id)
        {
            if((id ?? 0) == 0)
                return new List<Takeout>();
            return Db.Where<Takeout>("BookId=@id AND NOT IsReturned", new Dictionary<string, object>() { { "id", id } });
        }

        public static List<Takeout> GetByBook(long? id)
        {
            if((id ?? 0) == 0)
                return new List<Takeout>();
            return Db.Where<Takeout>("BookId=@id ORDER BY IsReturned ASC, TakeoutDate DESC LIMIT 0,100", new Dictionary<string, object>() { { "id", id } });
        }

        public static List<Takeout> GetByBorrower(long? id)
        {
            if((id??0)==0) return new List<Takeout>();
            return Db.Where<Takeout>("BorrowerId=@id ORDER BY IsReturned ASC,TakeoutDate DESC LIMIT 0,100", new Dictionary<string, object>() {{"id", id}});
        }

        public static List<Takeout> GetByUser(long? id)
        {
            if((id ?? 0) == 0)
                return new List<Takeout>();
            return Db.Where<Takeout>("UserId=@id ORDER BY TakeoutDate DESC LIMIT 0,100", new Dictionary<string, object>() { { "id", id } });
        }


        public static Takeout GetById(long id)
        {
            if(id == 0) return null;
            return Cache.FirstOrDefault(x => x.Id == id);
        }

        public static double GetRatingByBook(long id)
        {
            if (id == 0) return 0;
            if(!Db.TableExists("Takeouts"))
                return 0;
            return (Db.ExecuteScalar<double>("SELECT AVG(BookRating) FROM Takeouts WHERE BookId=@id",
                new Dictionary<string, object>() {{"id", id}}));
        }

        public static List<Takeout> GetUnreturnedByBorrower(long? id)
        {
            if((id ?? 0) == 0)
                return new List<Takeout>();

            return Db.Where<Takeout>("BorrowerId=@id AND NOT IsReturned", new Dictionary<string, object>() { { "id", id } });
        }

        public static double GetBookVotes(long id)
        {
            if(id == 0) return 0;
            if (!Db.TableExists("Takeouts")) return 0;
            return (Db.ExecuteScalar<double>("SELECT COUNT(BookRating) FROM Takeouts WHERE BookId=@id",
                new Dictionary<string, object>() { { "id", id } }));
        }

        public override bool Equals(object obj)
        {
            return (obj as Takeout)?.Id == Id;   
        }

        public bool Equals(Takeout other)
        {
            return other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static bool operator ==(Takeout left, Takeout right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Takeout left, Takeout right)
        {
            return !Equals(left, right);
        }

        public static List<Takeout> GetUnpaidByBorrower(long? id)
        {
            if((id ?? 0) == 0) return new List<Takeout>();

            return Db.Where<Takeout>("BorrowerId=@id AND NOT Paid AND IsReturned AND Penalty>0", new Dictionary<string, object>() { { "id", id } });
        }
    }
}
