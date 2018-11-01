using System;
using System.Collections.Generic;
using Libro.Data;

namespace Libro.Models
{
    /// <summary>
    /// A Transaction is created when a borrower borrows books. A transaction may contain multiple books.
    /// </summary>
    public class Transaction : ModelBase<Transaction>
    {
        /// <summary>
        /// The Id of the user on duty.
        /// </summary>
        public long UserId { get; set; }

        private DateTime _date;

        /// <summary>
        /// Date and time the books were taken out by the borrower.
        /// </summary>
        public DateTime Date
        {
            get { return _date; }
            set
            {
                if (value.Equals(_date)) return;
                _date = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The Id of the borrower.
        /// </summary>
        public long BorrowerId { get; set; }

        private string _note;

        /// <summary>
        /// Notes or remarks of this transaction.
        /// </summary>
        public string Note
        {
            get { return _note; }
            set
            {
                if (value == _note) return;
                _note = value;
                OnPropertyChanged();
            }
        }

        public static List<Transaction> GetByBorrower(long? id)
        {
            if(id==null) return new List<Transaction>();
            return Db.Where<Transaction>($"[{nameof(BorrowerId)}]=@{nameof(BorrowerId)}",
                new Dictionary<string, object>() {{nameof(BorrowerId), id}});
        }

        protected override bool GetIsEmpty()
        {
            return UserId == 0 || BorrowerId == 0;
        }
    }
}
