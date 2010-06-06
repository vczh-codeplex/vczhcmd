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
        private const int ButtonSize = 14;
        private const int ButtonSpaceWidth = 2;
        private const int ButtonSpaceHeight = 5;
        private const int YearBackward = 0;
        private const int MonthBackward = 1;
        private const int MonthForward = 2;
        private const int YearForward = 3;
        private const int DayStart = 4;

        private DateTime currentDay = DateTime.MinValue;
        private int lastButton = -1;
        private Font font = new Font("宋体", 10, GraphicsUnit.Pixel);

        public event CalendarDaySelectedHandler CalendarDayEntered;
        public event CalendarDaySelectedHandler CalendarDaySelected;
        public event EventHandler CurrentDayChanged;

        public MonthCalendar()
        {
            currentDay = DateTime.MinValue;
        }

        public Size CalendarSize
        {
            get
            {
                return new Size(ButtonSize * 35 + ButtonSpaceWidth * 36, ButtonSize + ButtonSpaceHeight * 2);
            }
        }

        public string Caption
        {
            get
            {
                return currentDay.Year.ToString() + "年" + currentDay.Month.ToString() + "月";
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
                DateTime day = new DateTime(value.Year, value.Month, 1);
                if (currentDay != day)
                {
                    currentDay = day;
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
            graphics.FillRectangle(Brushes.White, new Rectangle(location, CalendarSize));
            int days = DateTime.DaysInMonth(currentDay.Year, currentDay.Month);
            for (int i = 0; i < days + DayStart; i++)
            {
                Painter.DrawDay(graphics, GetButtonBounds(location, i), GetButtonDay(i), font, GetButtonText(i), lastButton == i);
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
                        CurrentDay = new DateTime(currentDay.Year, currentDay.Month - 1, 1);
                        break;
                    case MonthForward:
                        CurrentDay = new DateTime(currentDay.Year, currentDay.Month + 1, 1);
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
                case YearForward: return ">>";
                default: return (index - DayStart + 1).ToString();
            }
        }

        private Rectangle GetButtonBounds(Point location, int index)
        {
            return new Rectangle(new Point(location.X + index * (ButtonSize + ButtonSpaceWidth) + ButtonSpaceWidth, location.Y + ButtonSpaceHeight), new Size(ButtonSize, ButtonSize));
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
