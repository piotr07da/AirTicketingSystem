using System;

namespace Ats.Domain
{
    public class DayTime
    {
        private readonly TimeSpan _t;

        public DayTime(int hours, int minutes)
        {
            _t = new TimeSpan(hours, minutes, 0);
        }

        public int Hours => _t.Hours;
        public int Minutes => _t.Minutes;

        public static implicit operator TimeSpan(DayTime dt) => dt._t;
        public static implicit operator DayTime(TimeSpan dt) => new DayTime(dt.Hours, dt.Minutes);
    }
}
