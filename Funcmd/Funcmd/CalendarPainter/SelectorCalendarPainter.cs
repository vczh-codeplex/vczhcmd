using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Funcmd.CalendarPainter
{
    public class SelectorCalendarPainter : ICalendarPainter
    {
        public void DrawDay(Graphics graphics, Rectangle bounds, DateTime day, Font font, string text)
        {
            CalendarPainterNeededEventArgs e = new CalendarPainterNeededEventArgs(day);
            PainterNeeded(this, e);
            e.Painter.DrawDay(graphics, bounds, day, font, text);
        }

        public event CalendarPainterNeededHandler PainterNeeded;
    }

    public class CalendarPainterNeededEventArgs : EventArgs
    {
        public DateTime Day { get; private set; }
        public ICalendarPainter Painter { get; set; }

        public CalendarPainterNeededEventArgs(DateTime day)
        {
            Day = day;
        }
    }

    public delegate void CalendarPainterNeededHandler(object sender, CalendarPainterNeededEventArgs e);
}
