using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Funcmd.CalendarTimer
{
    public class EveryDayTimer : ICalendarTimer
    {
        private DateTime lastHappenDateTime = DateTime.MinValue;
        private CalendarTimerProvider provider;

        public EveryDayTimer(CalendarTimerProvider provider)
        {
            this.provider = provider;
            this.ActiveWeekDays = new DayOfWeek[]
            {
                DayOfWeek.Monday,
                DayOfWeek.Tuesday,
                DayOfWeek.Wednesday,
                DayOfWeek.Thursday,
                DayOfWeek.Friday
            };
        }

        public string Name { get; set; }
        public string Descripting { get; set; }
        public bool Urgent { get; set; }
        public bool Enabled { get; set; }
        public DateTime EventTime { get; set; }
        public DayOfWeek[] ActiveWeekDays { get; set; }

        public bool TurnedOff
        {
            get
            {
                return false;
            }
        }

        public bool IsActive()
        {
            if (EventTime.TimeOfDay == DateTime.Now.TimeOfDay)
            {
                if (lastHappenDateTime != DateTime.Now)
                {
                    lastHappenDateTime = DateTime.Now;
                    return true;
                }
            }
            return false;
        }

        public IObjectEditorType Type
        {
            get
            {
                return provider.Types.Where(t => t.GetType() == typeof(EveryDayTimerType)).First();
            }
        }
    }
}
