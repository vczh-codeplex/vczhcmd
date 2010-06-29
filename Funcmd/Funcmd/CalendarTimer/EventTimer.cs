using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Globalization;

namespace Funcmd.CalendarTimer
{
    public class EventTimer : ICalendarTimer
    {
        private bool happened = false;
        private CalendarTimerProvider provider;
        private DateTime eventDateTime;

        public EventTimer(CalendarTimerProvider provider)
        {
            this.provider = provider;
            this.EventDateTime = DateTime.Today;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public bool Urgent { get; set; }
        public bool Enabled { get; set; }
        public DateTime EventDateTime
        {
            get
            {
                return eventDateTime;
            }
            set
            {
                eventDateTime = value;
                happened = false;
            }
        }

        public bool TurnedOff
        {
            get
            {
                return (long)(DateTime.Now - EventDateTime).TotalSeconds > 0;
            }
        }

        public bool TestAndActive()
        {
            if (!TurnedOff && !happened && (long)(DateTime.Now - EventDateTime).TotalSeconds == 0)
            {
                happened = true;
                return true;
            }
            else
            {
                return false;
            }
        }

        public ICalendarTimer CloneTimer()
        {
            return new EventTimer(provider)
            {
                Name = Name,
                Description = Description,
                Urgent = Urgent,
                Enabled = Enabled,
                EventDateTime = EventDateTime,
            };
        }

        public IObjectEditorType Type
        {
            get
            {
                return provider.Types.Where(t => t.GetType() == typeof(EventTimerType)).First();
            }
        }

        public void LoadSetting(XElement element)
        {
            Name = element.Attribute("Name").Value;
            Description = element.Attribute("Descripting").Value;
            Urgent = bool.Parse(element.Attribute("Urgent").Value);
            Enabled = bool.Parse(element.Attribute("Enabled").Value);
            EventDateTime = DateTime.Parse(element.Attribute("EventDateTime").Value, CultureInfo.InvariantCulture);
        }

        public void SaveSetting(XElement element)
        {
            element.Add(new XAttribute("Name", Name));
            element.Add(new XAttribute("Descripting", Description));
            element.Add(new XAttribute("Urgent", Urgent.ToString()));
            element.Add(new XAttribute("Enabled", Enabled.ToString()));
            element.Add(new XAttribute("EventDateTime", EventDateTime.ToString(CultureInfo.InvariantCulture)));
        }

        public bool ShowMaskOnDate(DateTime date)
        {
            return EventDateTime.Date == date.Date;
        }

        public bool ShowDescriptionOnDate(DateTime date)
        {
            return EventDateTime.Date == date.Date;
        }

        public DateTime GetDescriptionTime()
        {
            return DateTime.Today + (EventDateTime - EventDateTime.Date);
        }
    }
}
