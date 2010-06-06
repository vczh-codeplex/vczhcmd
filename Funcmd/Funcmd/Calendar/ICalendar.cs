using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml.Linq;
using System.Windows.Forms;

namespace Funcmd.Calendar
{
    public interface ICalendar
    {
        Size CalendarSize { get; }
        DateTime CurrentDay { get; set; }
        ICalendarPainter Painter { get; set; }

        void Draw(Graphics graphics, Point location, Point cursor);
        Cursor GetCursor(Point location, Point cursor);
        void MouseMove(Point location, Point cursor);
        void MouseClick(Point location, Point cursor);

        event CalendarDaySelectedHandler CalendarDayEntered;
        event CalendarDaySelectedHandler CalendarDaySelected;
        event EventHandler CurrentDayChanged;
    }

    public class CalendarDaySelectedEventArgs
    {
        public DateTime Day { get; private set; }

        public CalendarDaySelectedEventArgs(DateTime day)
        {
            Day = day;
        }
    }

    public delegate void CalendarDaySelectedHandler(object sender,CalendarDaySelectedEventArgs e);
}
