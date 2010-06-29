using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Funcmd.CalendarTimer
{
    public partial class EventTimerPlugin : UserControl, ICalendarTimerEditorPlugin
    {
        public EventTimerPlugin()
        {
            InitializeComponent();
        }

        public Control Editor
        {
            get
            {
                return this;
            }
        }

        public void Edit(ICalendarTimer timer)
        {
        }

        public void Save(ICalendarTimer timer)
        {
        }
    }
}
