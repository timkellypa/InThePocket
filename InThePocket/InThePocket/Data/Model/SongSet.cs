using SQLite;
using System;

namespace InThePocket.Data.Model
{
    [Table("SongSet")]
    public class SongSet : SortableModelBase
    {
        [Column("Name")]
        public string Name { get; set; }

        [Column("Location")]
        public string Location { get; set; }

        private DateTime _date;
        [Column("Date")]
        public DateTime Date
        {
            get
            {
                if (_date == null || _date.Ticks == 0)
                {
                    _date = DateTime.Today;
                }
                return _date;
            }
            set {
                _date = value;
            }
        }

        private Boolean _isMaster;
        [Column("IsMaster")]
        public Boolean IsMaster
        {
            get
            {
                return _isMaster;
            }
            set
            {
                _isMaster = value;
                Location = "";
                Date = new DateTime(0);
            }
        }
        public Boolean IsEvent => !IsMaster;

        public String Detail
        {
            get
            {
                if (IsMaster)
                {
                    return "Master List";
                }
                else
                {
                    return $"{Location} - {Date.ToShortDateString()}";
                }
            }
        }
    }
}
