using System;
using System.Collections.Generic;
using System.Text;

using SQLite;

namespace InThePocket.Data.Model
{
    public class SongTempo : SortableModelBase, ICloneable
    {
        [Column("SongId")]
        public Guid SongId { get; set; }

        [Column("BPM")]
        public int BPM { get; set; }

        private int _beatsPerBar = 4;
        [Column("BeatsPerBar")]
        public int BeatsPerBar
        {
            get
            {
                return _beatsPerBar;
            }
            set
            {
                _beatsPerBar = value;
            }
        }

        private int _beatUnit = 4;
        [Column("BeatUnit")]
        public int BeatUnit
        {
            get
            {
                return _beatUnit;
            }
            set
            {
                _beatUnit = value;
            }
        }

        [Column("DottedQuarterAccent")]
        public bool DottedQuarterAccent { get; set; }

        private int _accentBeatsPerBar = 1;
        [Column("AccentBeatsPerBar")]
        public int AccentBeatsPerBar
        {
            get
            {
                return _accentBeatsPerBar;
            }
            set
            {
                _accentBeatsPerBar = value;
            }
        }

        [Column("NumberOfBars")]
        public int NumberOfBars { get; set; }

        [Ignore]
        public string DisplayText
        {
            get
            {
                string measureDisplay = NumberOfBars != 0 ? $" ({NumberOfBars} bars)" : "" ;
                return $"{BPM} BPM, {BeatsPerBar}/{BeatUnit}{measureDisplay}";
            }
        }

        public object Clone ()
        {
            return new SongTempo()
            {
                SongId = SongId,
                BPM = BPM,
                BeatsPerBar = BeatsPerBar,
                BeatUnit = BeatUnit,
                DottedQuarterAccent = DottedQuarterAccent,
                AccentBeatsPerBar = AccentBeatsPerBar,
                NumberOfBars = NumberOfBars
            };
        }
    }
}
