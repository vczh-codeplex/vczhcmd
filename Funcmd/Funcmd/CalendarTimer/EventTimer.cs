using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Funcmd.CalendarTimer
{
    public class EventTimer : ICalendarTimer
    {
        private bool happened = false;
        private CalendarTimerProvider provider;

        public EventTimer(CalendarTimerProvider provider)
        {
            this.provider = provider;
        }

        public string Name { get; set; }
        public string Descripting { get; set; }
        public bool Urgent { get; set; }
        public bool Enabled { get; set; }
        public DateTime EventDateTime { get; set; }

        public bool TurnedOff
        {
            get
            {
                return happened || EventDateTime > DateTime.Now;
            }
        }

        public bool IsActive()
        {
            return !happened && EventDateTime == DateTime.Now;
        }

        public IObjectEditorType Type
        {
            get
            {
                return provider.Types.Where(t => t.GetType() == typeof(EventTimerType)).First();
            }
        }
    }
}
