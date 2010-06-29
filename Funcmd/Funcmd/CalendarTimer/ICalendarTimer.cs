using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Funcmd.CalendarTimer
{
    public interface ICalendarTimer : IObjectEditorObject
    {
        string Descripting { get; set; }
        bool Urgent { get; set; }
        bool Enabled { get; set; }
        bool TurnedOff { get; }
        bool IsActive();
    }

    public class CalendarTimerProvider : IObjectEditorProvider
    {
        private IObjectEditorType[] types;

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
            get { throw new NotImplementedException(); }
        }

        public IList<IObjectEditorObject> Objects
        {
            get { throw new NotImplementedException(); }
        }
    }
}
