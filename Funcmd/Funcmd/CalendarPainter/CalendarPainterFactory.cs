using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Funcmd.CalendarPainter
{
    public abstract class CalendarPainterFactory
    {
        public abstract ICalendarPainter GetNormalPainter();

        public virtual ICalendarPainter GetInfoPainter()
        {
            return GetNormalPainter();
        }

        public virtual ICalendarPainter GetUrgentPainter()
        {
            return GetNormalPainter();
        }
    }
}
