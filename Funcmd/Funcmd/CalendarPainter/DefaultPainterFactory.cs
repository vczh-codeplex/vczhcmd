using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Funcmd.CalendarPainter
{
    public class DefaultPainterFactory : CalendarPainterFactory
    {
        private class Painter : ICalendarPainter
        {
            public Brush Background { get; set; }
            public Brush Border { get; set; }
            public Brush Text { get; set; }

            public void DrawDay(Graphics graphics, Rectangle bounds, DateTime day, Font font, string text)
            {
                if (Background != null)
                {
                    graphics.FillRectangle(Background, bounds);
                }
                SizeF size = graphics.MeasureString(text, font);
                graphics.DrawString(text, font, Text, new PointF(bounds.X + (bounds.Width - size.Width) / 2, bounds.Y + (bounds.Height - size.Height) / 2));
                if (day.Date == DateTime.Today)
                {
                    using (Pen pen = new Pen(Border))
                    {
                        graphics.DrawRectangle(pen, bounds);
                    }
                }
            }
        }

        private Painter normalPainter = new Painter()
        {
            Background = null,
            Border = Brushes.Black,
            Text = Brushes.Black
        };

        private Painter selectedPainter = new Painter()
        {
            Background = Brushes.Azure,
            Border = Brushes.Blue,
            Text = Brushes.Blue
        };

        private Painter infoPainter = new Painter()
        {
            Background = Brushes.Olive,
            Border = Brushes.Green,
            Text = Brushes.Green
        };

        private Painter urgentPainter = new Painter()
        {
            Background = Brushes.Pink,
            Border = Brushes.Red,
            Text = Brushes.Red
        };

        public override ICalendarPainter GetNormalPainter()
        {
            return normalPainter;
        }

        public override ICalendarPainter GetSelectedPainter()
        {
            return selectedPainter;
        }

        public override ICalendarPainter GetInfoPainter()
        {
            return infoPainter;
        }

        public override ICalendarPainter GetUrgentPainter()
        {
            return urgentPainter;
        }
    }
}
