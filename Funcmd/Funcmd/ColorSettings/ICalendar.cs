using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml.Linq;
using System.Windows.Forms;

namespace Funcmd.ColorSettings
{
    public interface ICalendar
    {
        Size CalendarSize { get; }
        void Draw(Graphics graphics, Point location, Point cursor);
        Cursor GetCursor(Point location, Point cursor);
        void Click(Point location, Point cursor);
    }
}
