using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Funcmd.CalendarTimer
{
    public class EventTimerType : IObjectEditorType
    {
        private CalendarTimerProvider provider;
        private CalendarTimerEditor editor;

        public EventTimerType(CalendarTimerProvider provider)
        {
            this.provider = provider;
            this.editor = new CalendarTimerEditor(new EventTimerPlugin());
        }

        public string Name
        {
            get
            {
                return "普通事件";
            }
        }

        public IObjectEditorObject CreateObject()
        {
            return new EventTimer(provider);
        }

        public System.Windows.Forms.Control EditObject(IObjectEditorObject obj)
        {
            editor.Edit((ICalendarTimer)obj);
            return editor;
        }

        public void Save()
        {
            editor.Save();
        }
    }
}
