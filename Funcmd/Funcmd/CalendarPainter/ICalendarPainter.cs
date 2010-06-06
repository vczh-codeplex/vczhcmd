using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Funcmd.CalendarPainter
{
    public interface ICalendarPainter
    {
        void DrawDay(Graphics graphics, Rectangle bounds, DateTime day, Font font, string text);
    }
}
