using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libro.Models
{
    public class Notification:ModelBase<Notification>
    {
        protected override bool GetIsEmpty()
        {
            return false;
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
