using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using Funcmd.CalendarPainter;

namespace Funcmd.Calendar
{
    public class MonthCalendar : ICalendar
    {
        private const int ButtonSize = 16;
        private const int ButtonSpace = 2;
        private const int YearBackward = 0;
        private const int MonthBackward = 1;
        private const int MonthForward = 2;
        private const int YearForward = 3;
        private const int DayStart = 4;

        private DateTime currentDay = default(DateTime);
        private int lastButton = -1;
        private Font font = new Font("微软雅黑", 12, GraphicsUnit.Pixel);

        public event CalendarDaySelectedHandler CalendarDayEntered;
        public event CalendarDaySelectedHandler CalendarDaySelected;
        public event EventHandler CurrentDayChanged;

        public MonthCalendar()
        {
            currentDay = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        }

        public Size CalendarSize
        {
            get
            {
                return new Size(ButtonSize * 35 + ButtonSpace * 36, ButtonSize + ButtonSpace * 2);
            }
        }

        public DateTime CurrentDay
        {
            get
            {
                return currentDay;
            }
            set
            {
                if (currentDay != value)
                {
                    currentDay = value;
                    if (CurrentDayChanged != null)
                    {
                        CurrentDayChanged(this, new EventArgs());
                    }
                }
            }
        }

        public ICalendarPainter Painter { get; set; }

        public void Draw(Graphics graphics, Point location, Point cursor)
        {
            int days = DateTime.DaysInMonth(currentDay.Year, currentDay.Month);
            for (int i = 0; i < days + DayStart; i++)
            {
                Painter.DrawDay(graphics, GetButtonBounds(location, i), GetButtonDay(i), font, GetButtonText(i));
            }
        }

        public Cursor GetCursor(Point location, Point cursor)
        {
            int days = DateTime.DaysInMonth(currentDay.Year, currentDay.Month);
            int button = GetButton(location, cursor);
            return button == -1 || button >= days + DayStart ? Cursors.Default : Cursors.Hand;
        }

        public void MouseMove(Point location, Point cursor)
        {
            int days = DateTime.DaysInMonth(currentDay.Year, currentDay.Month);
            int button = GetButton(location, cursor);
            if (lastButton != button)
            {
                lastButton = button;
                if (CalendarDayEntered != null)
                {
                    CalendarDayEntered(this, new CalendarDaySelectedEventArgs(GetButtonDay(button)));
                }
            }
        }

        public void MouseClick(Point location, Point cursor)
        {
            int days = DateTime.DaysInMonth(currentDay.Year, currentDay.Month);
            int button = GetButton(location, cursor);
            if (button != -1 && button < days + DayStart)
            {
                switch (button)
                {
                    case YearBackward:
                        CurrentDay = new DateTime(currentDay.Year - 1, currentDay.Month, 1);
                        break;
                    case MonthBackward:
                        CurrentDay = new DateTime(currentDay.Year, currentDay.Month, 1 - 1);
                        break;
                    case MonthForward:
                        CurrentDay = new DateTime(currentDay.Year, currentDay.Month, 1 + 1);
                        break;
                    case YearForward:
                        CurrentDay = new DateTime(currentDay.Year + 1, currentDay.Month, 1);
                        break;
                    default:
                        if (CalendarDaySelected != null)
                        {
                            CalendarDaySelected(this, new CalendarDaySelectedEventArgs(GetButtonDay(button)));
                        }
                        break;
                }
            }
        }

        private string GetButtonText(int index)
        {
            switch (index)
            {
                case YearBackward: return "<<";
                case MonthBackward: return "<";
                case MonthForward: return ">";
                case YearForward: return ">";
                default: return (index - DayStart + 1).ToString();
            }
        }

        private Rectangle GetButtonBounds(Point location, int index)
        {
            return new Rectangle(new Point(location.X + index * (ButtonSize + ButtonSpace) + ButtonSpace, location.Y + ButtonSpace), new Size(ButtonSize, ButtonSize));
        }

        private int GetButton(Point location, Point cursor)
        {
            for (int i = 0; i < 35; i++)
            {
                if (GetButtonBounds(location, i).Contains(cursor))
                {
                    return i;
                }
            }
            return -1;
        }

        private DateTime GetButtonDay(int button)
        {
            int days = DateTime.DaysInMonth(currentDay.Year, currentDay.Month);
            if (button >= DayStart && button < days + DayStart)
            {
                return new DateTime(currentDay.Year, currentDay.Month, currentDay.Day + button - DayStart);
            }
            else
            {
                return DateTime.MinValue;
            }
        }
    }
}
