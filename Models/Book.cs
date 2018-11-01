using System;
using System.IO;
using System.Linq;
using Libro.Data;

namespace Libro.Models
{
    public class Book : ModelBase<Book>
    {
        public Book()
        {
        }
        /// <summary>
        /// Creates a new Book instance copying all properties from <param name="book">book</param> parameter except Id and AccessionNumber.
        /// </summary>
        /// <param name="book">The instance to be copied from.</param>
        public Book(Book book)
        {
            Author = book.Author;
            CallNumber = book.CallNumber;
            SubTitle = book.SubTitle;
            Edition = book.Edition;
            Height = book.Height;
            Isbn = book.Isbn;
            Pages = book.Pages;
            PublicationPlace = book.PublicationPlace;
            Publisher = book.Publisher;
            Subject = book.Subject;
            Synopsis = book.Synopsis;
            Thickness = book.Thickness;
            Thumbnail = book.Thumbnail;
            Title = book.Title;
            Type = book.Type;
            Width = book.Width;
            Published = book.Published;
            Volume = book.Volume;
            Series = book.Series;
            Coauthors = book.Coauthors;
            Illustrations = book.Illustrations;
            InitialPages = book.InitialPages;
        }

        protected override bool GetIsEmpty()
        {
            return string.IsNullOrEmpty(Title) && string.IsNullOrEmpty(Isbn) && string.IsNullOrEmpty(Author);
        }

        #region Properties

        private string _barcode;

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

        private string _condition;

        public string Condition
        {
            get { return _condition; }
            set
            {
                if (value == _condition) return;
                _condition = value;
                OnPropertyChanged();
            }
        }

        private int _conditionNumber;

        public int ConditionNumber
        {
            get { return _conditionNumber; }
            set
            {
                if (value == _conditionNumber) return;
                _conditionNumber = value;
                OnPropertyChanged();
            }
        }

        private string _coauthors = "";

        public string Coauthors
        {
            get { return _coauthors; }
            set
            {
                if (value == _coauthors) return;
                _coauthors = value;
                OnPropertyChanged();
            }
        }

        private string _series;

        public string Series
        {
            get { return _series; }
            set
            {
                if (value == _series) return;
                _series = value;
                OnPropertyChanged();
            }
        }

        private string _volume;

        public string Volume
        {
            get { return _volume; }
            set
            {
                if (value == _volume) return;
                _volume = value;
                OnPropertyChanged();
            }
        }

        private string _isbn = "";

        public string Isbn
        {
            get { return _isbn; }
            set
            {
                if(value == _isbn)
                    return;
                _isbn = value;
                OnPropertyChanged();
            }
        }

        private string _isbn13 = "";

        public string Isbn13
        {
            get { return _isbn13; }
            set
            {
                if(value == _isbn13)
                    return;
                _isbn13 = value;
                OnPropertyChanged();
            }
        }

        private string _otherId;

        public string OtherId
        {
            get { return _otherId; }
            set
            {
                if(value == _otherId)
                    return;
                _otherId = value;
                OnPropertyChanged();
            }
        }

        private string _issn = "";

        public string Issn
        {
            get { return _issn; }
            set
            {
                if(value == _issn)
                    return;
                _issn = value;
                OnPropertyChanged();
            }
        }

        private string _title = "";

        public string Title
        {
            get { return _title; }
            set
            {
                if(value == _title)
                    return;
                _title = value;
                OnPropertyChanged();
            }
        }

        private int _copies;

        public int Copies
        {
            get { return _copies; }
            set
            {
                if(value == _copies)
                    return;
                _copies = value;
                OnPropertyChanged();
            }
        }

        private string _location;

        public string Location
        {
            get { return _location; }
            set
            {
                if(value == _location)
                    return;
                _location = value;
                OnPropertyChanged();
            }
        }
        
        private string _subject;

        public string Subject
        {
            get { return _subject; }
            set
            {
                if(value == _subject)
                    return;
                _subject = value;
                OnPropertyChanged();
            }
        }

        private string _publicationPlace;

        /// <summary>
        ///     Place of publication.
        /// </summary>
        public string PublicationPlace
        {
            get { return _publicationPlace; }
            set
            {
                if(value == _publicationPlace)
                    return;
                _publicationPlace = value;
                OnPropertyChanged();
            }
        }



        private string _synopsis;

        public string Synopsis
        {
            get { return _synopsis; }
            set
            {
                if(value == _synopsis)
                    return;
                _synopsis = value;
                OnPropertyChanged();
            }
        }

        private int _pages;

        /// <summary>
        ///     Number of pages.
        /// </summary>
        public int Pages
        {
            get { return _pages; }
            set
            {
                if(value == _pages)
                    return;
                _pages = value;
                OnPropertyChanged();
            }
        }

        private string _height;

        public string Height
        {
            get { return _height; }
            set
            {
                if(_height==value)
                    return;
                _height = value;
                OnPropertyChanged();
            }
        }

        private string _width;

        public string Width
        {
            get { return _width; }
            set
            {
                if(_width==value)
                    return;
                _width = value;
                OnPropertyChanged();
            }
        }

        [Ignore]
        public bool HasThumbnail
        {
            get
            {
                if (string.IsNullOrEmpty(Thumbnail)) return false;
                return File.Exists(Thumbnail);
            }
        }

        private string _thumbnail;

        public string Thumbnail
        {
            get { return _thumbnail; }
            set
            {
                if(value == _thumbnail)
                    return;
                _thumbnail = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasThumbnail));
            }
        }

        private string _thickness;

        public string Thickness
        {
            get { return _thickness; }
            set
            {
                if(_thickness==value)
                    return;
                _thickness = value;
                OnPropertyChanged();
            }
        }

        private string _published;

        /// <summary>
        ///     Date the this book was published.
        /// </summary>
        public string Published
        {
            get { return _published; }
            set
            {
                if(_published==value)
                    return;
                _published = value;
                OnPropertyChanged();
            }
        }

        private string _edition;

        public string Edition
        {
            get { return _edition; }
            set
            {
                if(value == _edition)
                    return;
                _edition = value;
                OnPropertyChanged();
            }
        }

        private string _authorNumber;

        public string AuthorNumber
        {
            get { return _authorNumber; }
            set
            {
                if(value == _authorNumber)
                    return;
                _authorNumber = value;
                OnPropertyChanged();
            }
        }

        private string _accessionNumber;

        public string AccessionNumber
        {
            get { return _accessionNumber; }
            set
            {
                if(value == _accessionNumber)
                    return;

                if (!(Id > 0 && Cache.Any(x => x.AccessionNumber == value && x.Id != Id)))
                    _accessionNumber = value;

                OnPropertyChanged();
            }
        }

        private string _remarks1;

        public string Remarks
        {
            get { return _remarks1; }
            set
            {
                if (value == _remarks1) return;
                _remarks1 = value;
                OnPropertyChanged();
            }
        }

        private double _price;

        public double Price
        {
            get { return _price; }
            set
            {
                if (value ==_price) return;
                _price = value;
                OnPropertyChanged();
            }
        }

        private string _fund;

        public string Fund
        {
            get { return _fund; }
            set
            {
                if (value == _fund) return;
                _fund = value;
                OnPropertyChanged();
            }
        }

        private string _section = "";

        public string Section
        {
            get { return _section; }
            set
            {
                if (value == _section) return;
                _section = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _dateReceived;

        public DateTime? DateReceived
        {
            get { return _dateReceived; }
            set
            {
                if (value==(_dateReceived)) return;
                _dateReceived = value;
                OnPropertyChanged();
            }
        }

        private string _callNumber;

        public string CallNumber
        {
            get { return _callNumber; }
            set
            {
                if (value == _callNumber) return;
                _callNumber = value;
                if (string.IsNullOrEmpty(value)) return;
                var parts = value.Split(new string[] {Environment.NewLine, " "}, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length > 0)
                {
                    _location = parts[0];
                    OnPropertyChanged(nameof(Location));
                }
                if (parts.Length > 1)
                {
                    _classificationNumber = parts[1];
                    OnPropertyChanged(nameof(ClassificationNumber));
                }
                if (parts.Length > 2)
                {
                    _authorNumber = parts[2];
                    OnPropertyChanged(nameof(AuthorNumber));
                }
                OnPropertyChanged();
            }
        }

        private string _initialPages;

        public string InitialPages
        {
            get { return _initialPages; }
            set
            {
                if (value == _initialPages) return;
                _initialPages = value;
                OnPropertyChanged();
            }
        }

        private string _classificationNumber;

        public string ClassificationNumber
        {
            get { return _classificationNumber; }
            set
            {
                if(value == _classificationNumber)
                    return;
                _classificationNumber = value;
                OnPropertyChanged();
            }
        }

        private string _status;

        private string _publisher;

        public string Publisher
        {
            get { return _publisher; }
            set
            {
                if(value == _publisher)
                    return;
                _publisher = value;
                OnPropertyChanged();
            }
        }

        private string _author = "";

        public string Author
        {
            get { return _author; }
            set
            {
                if(value == _author)
                    return;
                _author = value;
                OnPropertyChanged();
            }
        }

        private string _type;
        /// <summary>
        /// Book, ArtWork, Chart, FlashCard, Journal, Magazine, Map,NewsLetter, NewsPaper, Pamphlet, Thesis, Other
        /// </summary>
        public string Type
        {
            get { return _type; }
            set
            {
                if (value == _type) return;
                _type = value;
                OnPropertyChanged();
            }
        }

        private string _illustrations;

        public string Illustrations
        {
            get { return _illustrations; }
            set
            {
                if (value == _illustrations) return;
                _illustrations = value;
                OnPropertyChanged();
            }
        }

        private long _borrowerId;

        public long TakeoutId
        {
            get { return _borrowerId; }
            set
            {
                if (value == _borrowerId) return;
                _borrowerId = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsBorrowed));
            }
        }

        [Ignore]
        public bool IsBorrowed => TakeoutId > 0;

        private string _category;

        public string SubTitle
        {
            get { return _category; }
            set
            {
                if (value == _category) return;
                _category = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region ErrorInfo

        protected override string GetErrorInfo(string prop)
        {
            switch (prop)
            {
                case nameof(AccessionNumber):
                {
                        if(!string.IsNullOrEmpty(AccessionNumber) && Cache.Any(x => x.AccessionNumber == AccessionNumber && x.Id != Id))
                            return $"Accession Number already exists.";
                        break;
                    }
                case nameof(Barcode):
                {
                        if(!string.IsNullOrEmpty(Barcode) && Cache.Any(x => x.Barcode == Barcode && x.Id != Id))
                            return $"Barcode already exists.";
                        break;
                }
            }
            
            return base.GetErrorInfo(prop);
        }

        protected override bool CanSave(object obj)
        {
            if(string.IsNullOrEmpty(AccessionNumber))
                return false;
            return base.CanSave(obj);
        }

        protected override bool GetIsValid()
        {
            if(!string.IsNullOrEmpty(AccessionNumber) && Cache.Any(x => x.AccessionNumber == AccessionNumber && x.Id != Id)) return false;
            return !GetIsEmpty();
        }

        #endregion

        public static Book GetById(long id)
        {
            return Db.GetById<Book>(id);
        }

        public bool EqualsId(string value)
        {
            return Barcode == value || Isbn == value || Isbn13 == value || Issn == value;
        }
    }
}