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

        void Draw(Graphics graphics, Point location, Point cursor);
        Cursor GetCursor(Point location, Point cursor);
        void Click(Point location, Point cursor);

        event CalendarDaySelectedHandler CalendarDaySelected;
        event EventHandler CurrentDayChanged;
    }

    public class CalendarDaySelectedEventArgs
    {
        public DateTime Day { get; set; }
    }

    public delegate void CalendarDaySelectedHandler(object sender,CalendarDaySelectedEventArgs e);
}
