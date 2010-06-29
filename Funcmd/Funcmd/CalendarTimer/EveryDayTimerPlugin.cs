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
    public partial class EveryDayTimerPlugin : UserControl, ICalendarTimerEditorPlugin
    {
        public EveryDayTimerPlugin()
        {
            InitializeComponent();
            checkBox1.Tag = DayOfWeek.Sunday;
            checkBox2.Tag = DayOfWeek.Monday;
            checkBox3.Tag = DayOfWeek.Tuesday;
            checkBox4.Tag = DayOfWeek.Wednesday;
            checkBox5.Tag = DayOfWeek.Thursday;
            checkBox6.Tag = DayOfWeek.Friday;
            checkBox7.Tag = DayOfWeek.Saturday;
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
            EveryDayTimer everyDayTimer = (EveryDayTimer)timer;
            dateTimeTime.Value = everyDayTimer.EventTime;
            foreach (CheckBox checkBox in tableTimer.Controls.Cast<Control>().Where(c => c.Tag is DayOfWeek))
            {
                checkBox.Checked = everyDayTimer.ActiveWeekDays.Contains((DayOfWeek)checkBox.Tag);
            }
        }

        public void Save(ICalendarTimer timer)
        {
            EveryDayTimer everyDayTimer = (EveryDayTimer)timer;
            everyDayTimer.EventTime = dateTimeTime.Value;
            everyDayTimer.ActiveWeekDays = tableTimer
                .Controls
                .Cast<Control>()
                .Where(c => c.Tag is DayOfWeek)
                .Cast<CheckBox>()
                .Where(c => c.Checked)
                .Select(c => (DayOfWeek)c.Tag)
                .ToArray();
        }
    }
}
