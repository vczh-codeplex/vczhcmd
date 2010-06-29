using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Globalization;

namespace Funcmd.CalendarTimer
{
    public class EveryDayTimer : ICalendarTimer
    {
        private DateTime lastHappenDateTime = DateTime.MinValue;
        private CalendarTimerProvider provider;

        public EveryDayTimer(CalendarTimerProvider provider)
        {
            this.provider = provider;
            this.EventTime = DateTime.Now;
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

        public ICalendarTimer CloneTimer()
        {
            return new EveryDayTimer(provider)
            {
                Name = Name,
                Descripting = Descripting,
                Urgent = Urgent,
                Enabled = Enabled,
                EventTime = EventTime,
                ActiveWeekDays = ActiveWeekDays
            };
        }

        public IObjectEditorType Type
        {
            get
            {
                return provider.Types.Where(t => t.GetType() == typeof(EveryDayTimerType)).First();
            }
        }

        public void LoadSetting(XElement element)
        {
            Name = element.Attribute("Name").Value;
            Descripting = element.Attribute("Descripting").Value;
            Urgent = bool.Parse(element.Attribute("Urgent").Value);
            Enabled = bool.Parse(element.Attribute("Enabled").Value);
            EventTime = DateTime.Parse(element.Attribute("EventTime").Value, CultureInfo.InvariantCulture);

            string[] weekDays = element
                .Elements("ActiveWeekDay")
                .Select(e => e.Attribute("Name").Value)
                .ToArray();
            ActiveWeekDays = Enum.GetValues(typeof(DayOfWeek))
                .Cast<DayOfWeek>()
                .Where(d => weekDays.Contains(d.ToString()))
                .ToArray();
        }

        public void SaveSetting(XElement element)
        {
            element.Add(new XAttribute("Name", Name));
            element.Add(new XAttribute("Descripting", Descripting));
            element.Add(new XAttribute("Urgent", Urgent.ToString()));
            element.Add(new XAttribute("Enabled", Enabled.ToString()));
            element.Add(new XAttribute("EventTime", EventTime.ToString(CultureInfo.InvariantCulture)));

            foreach (DayOfWeek weekDay in ActiveWeekDays)
            {
                element.Add(new XElement("ActiveWeekDay", new XAttribute("Name", weekDay.ToString())));
            }
        }

        public bool ShowMaskOnDate(DateTime date)
        {
            return false;
        }

        public bool ShowDescriptionOnDate(DateTime date)
        {
            return ActiveWeekDays.Contains(date.DayOfWeek);
        }

        public DateTime GetDescriptionTime()
        {
            return DateTime.Now + (EventTime - EventTime.Date);
        }
    }
}
