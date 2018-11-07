using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Libro.Data;

namespace Libro.Models
{
    public class Notification:ModelBase<Notification>
    {
        public enum NotificationTypes
        {
            TakeoutExpired,
            Others
        }

        protected override bool GetIsEmpty()
        {
            return false;
        }
        
        private static bool _dbTypeSet;


        private NotificationTypes _NotificationType = NotificationTypes.Others;

        public NotificationTypes NotificationType
        {
            get => _NotificationType;
            set
            {
                if (value == _NotificationType) return;
                _NotificationType = value;
                OnPropertyChanged(nameof(NotificationType));
            }
        }

        private Takeout _takeout;
        public Takeout Takeout => _takeout ?? (NotificationType==NotificationTypes.TakeoutExpired ? _takeout = Takeout.GetById(RecordId):null);
        public Borrower Borrower => Takeout?.Borrower;

        private string _Thumbnail;

        public string Thumbnail
        {
            get => _Thumbnail;
            set
            {
                if (value == _Thumbnail) return;
                _Thumbnail = value;
                OnPropertyChanged(nameof(Thumbnail));
                OnPropertyChanged(nameof(HasThumbnail));
            }
        }

        public bool HasThumbnail => !string.IsNullOrEmpty(Thumbnail) && File.Exists(Thumbnail);
        
        private bool _Read;
        
        public bool Read
        {
            get => _Read;
            set
            {
                if (value == _Read) return;
                _Read = value;
                OnPropertyChanged(nameof(Read));
            }
        }
        
        private string _Title;

        public string Title
        {
            get => _Title;
            set
            {
                if (value == _Title) return;
                _Title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        private string _Message;

        public string Message
        {
            get => _Message;
            set
            {
                if (value == _Message) return;
                _Message = value;
                OnPropertyChanged(nameof(Message));
            }
        }

        private long _RecordId;

        public long RecordId
        {
            get => _RecordId;
            set
            {
                if (value == _RecordId) return;
                _RecordId = value;
                OnPropertyChanged(nameof(RecordId));
            }
        }
        
        private DateTime _Created = DateTime.Now;

        public DateTime Created
        {
            get => _Created;
            set
            {
                if (value == _Created) return;
                _Created = value;
                OnPropertyChanged(nameof(Created));
            }
        }


    }
}
