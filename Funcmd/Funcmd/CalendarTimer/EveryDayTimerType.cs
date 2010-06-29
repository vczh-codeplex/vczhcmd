using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Funcmd.CalendarTimer
{
    public class EveryDayTimerType : IObjectEditorType
    {
        private CalendarTimerProvider provider;
        private CalendarTimerEditor editor;

        public EveryDayTimerType(CalendarTimerProvider provider)
        {
            this.provider = provider;
            this.editor = new CalendarTimerEditor(new EveryDayTimerPlugin());
        }

        public string Name
        {
            get
            {
                return "定时事件";
            }
        }

        public IObjectEditorObject CreateObject()
        {
            return new EveryDayTimer(provider);
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
