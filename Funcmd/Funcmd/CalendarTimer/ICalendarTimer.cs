using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Funcmd.CalendarTimer
{
    public interface ICalendarTimer : IObjectEditorObject
    {
        string Description { get; set; }
        bool Urgent { get; set; }
        bool Enabled { get; set; }
        bool TurnedOff { get; }
        bool TestAndActive();
        ICalendarTimer CloneTimer();
        void LoadSetting(XElement element);
        void SaveSetting(XElement element);

        bool ShowMaskOnDate(DateTime date);
        bool ShowDescriptionOnDate(DateTime date);
        DateTime GetDescriptionTime();
    }

    public class CalendarTimerProvider : IObjectEditorProvider
    {
        private IObjectEditorType[] types;
        private List<IObjectEditorObject> objects = new List<IObjectEditorObject>();

        public CalendarTimerProvider()
        {
            types = new IObjectEditorType[]
            {
                new EventTimerType(this),
                new EveryDayTimerType(this)
            };
        }

        public string Title
        {
            get
            {
                return "事件编辑器";
            }
        }

        public string Header
        {
            get
            {
                return "事件名称";
            }
        }

        public IObjectEditorType[] Types
        {
            get
            {
                return types;
            }
        }

        public IList<IObjectEditorObject> Objects
        {
            get
            {
                return objects;
            }
        }

        public void Load(List<ICalendarTimer> commands)
        {
            objects.Clear();
            objects.AddRange(commands.Select(c => c.CloneTimer()));
        }

        public void Save(List<ICalendarTimer> commands)
        {
            commands.Clear();
            commands.AddRange(objects.Cast<ICalendarTimer>());
            objects.Clear();
        }
    }
}
