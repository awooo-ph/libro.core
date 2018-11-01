using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Libro.Annotations;
using Libro.Data;

namespace Libro.Models
{
    public abstract class ModelBase<T> : IModel, IDataErrorInfo, INotifyPropertyChanged where T : ModelBase<T>
    {
        protected ModelBase() { }

        private ICommand _saveCommand;
        public ICommand SaveCommand => _saveCommand ?? (_saveCommand = new DelegateCommand(_Save, CanSave));

        protected virtual bool CanSave(object obj)
        {
            return GetIsValid();
        }

        public bool IsDeleted { get; set; }

        private void _Save(object obj)
        {
            Save();
        }

        public void Remove()
        {
            Cache.Remove((T) this);
        }

        public virtual void Save()
        {
            if (!CanSave(null)) return;
            Db.SaveItem((T) this);
            if(!Cache.Contains((T)this))
                Cache.Add((T)this);
        }

        public virtual void Update<TT>(string column, TT value)
        {
            //this[column] = value;
            var model = FastMember.TypeAccessor.Create(typeof(T));
            model[this, column] = value;
            //this.Update(column);
            Db.Update<T>(Id, column,value);
        }

        [Ignore]
        public object this[string prop]
        {
            get { return GetProperty(prop); }
            set
            {
                SetProperty(prop,value);
                OnPropertyChanged(prop);
            }
        }

        protected virtual object GetProperty(string prop)
        {
            var model = FastMember.ObjectAccessor.Create(this);
            return model[prop];
        }

        protected virtual void SetProperty(string prop, object value)
        {
            var model = FastMember.TypeAccessor.Create(typeof(T));
            model[this, prop] = value;
        }

        private ICommand _deleteCommand;
        public ICommand DeleteCommand => _deleteCommand ?? (_deleteCommand = new DelegateCommand(d => Delete(),CanDelete));

        protected virtual bool CanDelete(object obj)
        {
            return true;
        }

        public virtual void Delete()
        {
            if (_cache.ContainsKey(GetType()))
            Db.Delete<T>(Id);
            _cache[GetType()].Remove((T)this);
        }
        
        private long _id;

        /// <summary>
        /// Database record Identifier
        /// </summary>
        [PrimaryKey]
        public long Id
        {
            get { return _id; }
            set
            {
                if (value == _id) return;
                _id = value;
                OnPropertyChanged();
            }
        }

        internal void OnSaved()
        {
            OnPropertyChanged(nameof(Id));
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsValid)));
        }

        private static Dictionary<Type, ObservableCollection<T>> _cache = new Dictionary<Type, ObservableCollection<T>>();
        public static ObservableCollection<T> GetAll()
        {
            if (_cache.ContainsKey(typeof(T)))
                return _cache[typeof(T)];
            var list = new ObservableCollection<T>(Db.GetAll<T>());
            _cache.Add(typeof(T),list);
            return list;
        }

        public static ObservableCollection<T> Cache => GetAll();

        protected abstract bool GetIsEmpty();

        [Ignore]
        public bool IsEmpty => GetIsEmpty();

        [Ignore]
        public bool IsValid => GetIsValid();

        protected virtual bool GetIsValid()
        {
            return !GetIsEmpty();
        }

        protected virtual string GetErrorInfo(string prop)
        {
            return null;
        }

        string IDataErrorInfo.this[string columnName] => GetErrorInfo(columnName);

        string IDataErrorInfo.Error => null;
    }
}
