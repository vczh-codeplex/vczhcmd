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

        public EventTimer(CalendarTimerProvider provider)
        {
            this.provider = provider;
            this.EventDateTime = DateTime.Today;
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

        public ICalendarTimer CloneTimer()
        {
            return new EventTimer(provider)
            {
                Name = Name,
                Descripting = Descripting,
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
            Descripting = element.Attribute("Descripting").Value;
            Urgent = bool.Parse(element.Attribute("Urgent").Value);
            Enabled = bool.Parse(element.Attribute("Enabled").Value);
            EventDateTime = DateTime.Parse(element.Attribute("EventDateTime").Value, CultureInfo.InvariantCulture);
        }

        public void SaveSetting(XElement element)
        {
            element.Add(new XAttribute("Name", Name));
            element.Add(new XAttribute("Descripting", Descripting));
            element.Add(new XAttribute("Urgent", Urgent.ToString()));
            element.Add(new XAttribute("Enabled", Enabled.ToString()));
            element.Add(new XAttribute("EventDateTime", EventDateTime.ToString(CultureInfo.InvariantCulture)));
        }
    }
}
