using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Funcmd.CalendarTimer
{
    public class EveryDayTimerType : IObjectEditorType
    {
        private CalendarTimerProvider provider;

        public EveryDayTimerType(CalendarTimerProvider provider)
        {
            this.provider = provider;
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
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
