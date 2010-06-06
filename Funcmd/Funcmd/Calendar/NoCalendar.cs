using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Funcmd.Calendar
{
    public class NoCalendar : ICalendar
    {
        public event CalendarDaySelectedHandler CalendarDayEntered;
        public event CalendarDaySelectedHandler CalendarDaySelected;
        public event EventHandler CurrentDayChanged;

        public Size CalendarSize
        {
            get
            {
                return new Size(1, 24);
            }
        }

        public string Caption
        {
            get
            {
                return "陈梓瀚(vczh)";
            }
        }

        public DateTime CurrentDay { get; set; }

        public CalendarPainter.ICalendarPainter Painter { get; set; }

        public void Draw(Graphics graphics, Point location, Point cursor)
        {
        }

        public Cursor GetCursor(Point location, Point cursor)
        {
            return Cursors.Default;
        }

        public void MouseMove(Point location, Point cursor)
        {
        }

        public void MouseClick(Point location, Point cursor)
        {
        }

        private void SupressWarning()
        {
            CalendarDayEntered(this, new CalendarDaySelectedEventArgs(DateTime.Today));
            CalendarDaySelected(this, new CalendarDaySelectedEventArgs(DateTime.Today));
            CurrentDayChanged(this, new EventArgs());
        }
    }
}
