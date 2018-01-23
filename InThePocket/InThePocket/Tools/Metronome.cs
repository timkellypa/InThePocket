using System;
using System.Collections.Generic;
using System.Timers;

using System.ComponentModel;
using System.Linq;

using InThePocket.Data.Model;

namespace InThePocket.Tools
{
    public class Metronome : INotifyPropertyChanged, IDisposable
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public const int DEFAULT_BEAT_DELAY = 100;
        private const int COUNT_OUT_MEASURES = 2;
        private const int PRE_START_COUNT_MEASURES = 3;

        private int _measure;
        public int Measure
        {
            get
            {
                return _measure;
            }
            set
            {
                int oldMeasure = _measure;
                _measure = value;
                if (_measure <= 0 || TempoQueue.Count == 0 || TempoQueue.First().NumberOfBars == 0)
                {
                    return;
                }

                // Pop off the number of measures that we've incremented off the queue.
                for (int i = 1; i <= _measure - oldMeasure; ++i)
                {
                    bool poppedOne = false;
                    do
                    {
                        if (TempoQueue.First().NumberOfBars == 0)
                        {
                            TempoQueue.RemoveAt(0);
                            continue;
                        }
                        else
                        {
                            TempoQueue.First().NumberOfBars--;
                            poppedOne = true;
                        }
                    } while (!poppedOne && TempoQueue.Count > 0);

                    // If we ended on an empty tempo (no measures)... then remove until we have one that is populated, or none.
                    while (TempoQueue.Count > 0 && TempoQueue.First().NumberOfBars == 0)
                    {
                        TempoQueue.RemoveAt(0);
                    }
                }
            }
        }

        private int _count;
        public int Count
        {
            get
            {
                return _count;
            }
            protected set
            {
                _count = value;

                if (_count == 0)
                {
                    return;
                }
                else if (TempoQueue == null || TempoQueue.Count == 0)
                {
                    Count = 0;
                    return;
                }

                if (_count > TempoQueue.First().BeatsPerBar)
                {
                    Measure++;
                    _count = TempoQueue.Count == 0 ? 0 : 1;
                }
            }
        }
        public List<SongTempo> TempoList;
        public List<SongTempo> TempoQueue;
        public bool Active { get; private set; }

        private bool _inClick = false;
        public bool InClick
        {
            get
            {
                return _inClick;
            }
            set
            {
                if (_inClick != value)
                {
                    _inClick = value;

                    NotifyPropertyChanged("InClick");
                    NotifyPropertyChanged("InPrimaryClick");
                    NotifyPropertyChanged("InSecondaryClick");
                }
            }
        }

        public bool InPrimaryClick
        {
            get => InClick && CurrentTempo.AccentCounts.Contains(Count);
        }

        public bool InSecondaryClick
        {
            get => InClick && !CurrentTempo.AccentCounts.Contains(Count);
        }

        public SongTempo CurrentTempo
        {
            get
            {
                if (TempoQueue != null && TempoQueue.Count > 0)
                    return TempoQueue[0];
                else if (TempoList != null && TempoList.Count > 0)
                    return TempoList[0];
                else
                    return new SongTempo()
                    {
                        AccentBeatsPerBar = 1,
                        BeatsPerBar = 4,
                        BeatUnit = 4,
                        BPM = 0,
                        DottedQuarterAccent = false,
                        NumberOfBars = 0
                    };
            }
        }

        public double BPM
        {
            get
            {
                return CurrentTempo.CalculatedBPM;
            }
        }

        private Timer _clickDurationTimer;
        private Timer _nextClickTimer;

        public Metronome(List<SongTempo> tempoList)
        {
            _clickDurationTimer = new Timer
            {
                Interval = DEFAULT_BEAT_DELAY,
                AutoReset = false
            };

            _nextClickTimer = new Timer
            {
                AutoReset = false
            };

            DelegateEvents();

            TempoList = tempoList;
        }

        public Metronome() : this(new List<SongTempo>()) { }

        private void DelegateEvents()
        {
            _clickDurationTimer.Elapsed += Off;
            _nextClickTimer.Elapsed += Click;
        }

        private void On()
        {
            InClick = true;
        }

        private void Off()
        {
            InClick = false;
        }

        private void Off(Object sender, ElapsedEventArgs e)
        {
            Off();

            // Don't continue if the count was set back to zero... That means, we need to stop.
            if (Count == 0)
                return;

            _nextClickTimer.Interval = ((1.0 / BPM) * 60 * 1000.0 - DEFAULT_BEAT_DELAY);
            _nextClickTimer.Start();
        }

        private void Click(Object sender, ElapsedEventArgs e)
        {
            Click();
        }

        private void Click()
        {
            Count++;
            if (Count != 0)
            {
                On();
                _clickDurationTimer.Start();
            }
        }

        public void CountOut()
        {
            if (TempoList == null || TempoList.Count == 0)
            {
                return;
            }
            Count = 0;
            Measure = -COUNT_OUT_MEASURES + 1;
            TempoQueue = new List<SongTempo>() { TempoList.First().Clone() as SongTempo };
            TempoQueue.First().NumberOfBars = 1;
            Click();
        }

        public void Start()
        {
            if (TempoList == null || TempoList.Count == 0)
            {
                return;
            }

            Count = 0;
            Measure = -PRE_START_COUNT_MEASURES;
            TempoQueue = new List<SongTempo>();
            foreach (SongTempo tempo in TempoList)
            {
                SongTempo tempoCopy = tempo.Clone() as SongTempo;
                if (tempoCopy.NumberOfBars == 0)
                {
                    tempoCopy.NumberOfBars = -1;
                }
                TempoQueue.Add(tempo.Clone() as SongTempo);
            }
            Click();
        }

        public void Stop()
        {
            _nextClickTimer.Stop();
            _clickDurationTimer.Stop();
            TempoQueue = new List<SongTempo>();
            Count = 0;
            Off();
        }

        private void UnDelegateEvents()
        {
            _clickDurationTimer.Elapsed -= Off;
            _nextClickTimer.Elapsed -= Click;
        }

        public void Dispose()
        {
            UnDelegateEvents();
            _clickDurationTimer.Dispose();
            _nextClickTimer.Dispose();
            TempoList.Clear();
            TempoQueue.Clear();
            TempoList = null;
            TempoQueue = null;
        }
    }
}
