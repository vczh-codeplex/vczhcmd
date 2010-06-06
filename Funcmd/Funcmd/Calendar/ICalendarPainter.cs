using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Funcmd.Calendar
{
    public interface ICalendarPainter
    {
        void DrawDay(Graphics graphics, Rectangle bounds, DateTime day, Font font, string text);

        event CalendarPainterGetProxyHandler OnProxyNeeded;
    }

    public class CalendarPainterGetProxyEventArgs : EventArgs
    {
        public ICalendarPainter Painter { get; set; }
        public DateTime Day { get; private set; }

        public CalendarPainterGetProxyEventArgs(DateTime day)
        {
            Day = day;
        }
    }

    public delegate void CalendarPainterGetProxyHandler(object sender, CalendarDaySelectedEventArgs e);
}
