using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Funcmd.CalendarTimer
{
    public class EventTimerType : IObjectEditorType
    {
        private CalendarTimerProvider provider;

        public EventTimerType(CalendarTimerProvider provider)
        {
            this.provider = provider;
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
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
