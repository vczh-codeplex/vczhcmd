using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Funcmd.Calendar
{
    public class MonthCalendar : ICalendar
    {
        public Size CalendarSize
        {
            get { throw new NotImplementedException(); }
        }

        public DateTime CurrentDay
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void Draw(Graphics graphics, Point location, Point cursor)
        {
            throw new NotImplementedException();
        }

        public Cursor GetCursor(Point location, Point cursor)
        {
            throw new NotImplementedException();
        }

        public void Click(Point location, Point cursor)
        {
            throw new NotImplementedException();
        }

        public event CalendarDaySelectedHandler CalendarDaySelected;

        public event EventHandler CurrentDayChanged;
    }
}
