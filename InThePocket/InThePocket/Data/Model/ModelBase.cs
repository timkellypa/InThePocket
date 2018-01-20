using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel;
using System.Threading.Tasks;
using SQLite;

namespace InThePocket.Data.Model
{
    public class ModelBase : INotifyPropertyChanged, IModel
    {
#pragma warning disable 0067
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 0067

        private Guid _id;

        [PrimaryKey]
        public Guid Id
        {
            get
            {
                if (_id == Guid.Empty)
                {
                    _id = Guid.NewGuid();
                }
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        public virtual async Task Save()
        {
            await DataAccess.Database.InsertOrReplaceAsync(this);
        }

        public virtual async Task Delete()
        {
            await DataAccess.Database.DeleteAsync(this);
        }

        public virtual Task InitTable()
        {
            return Task.CompletedTask;
        }
    }
}
