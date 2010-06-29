using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Funcmd.CalendarTimer
{
    public partial class CalendarTimerAlarmForm : Form
    {
        private class TimerItem
        {
            public DateTime Time { get; set; }
            public ICalendarTimer Timer { get; set; }
        }

        public CalendarTimerAlarmForm()
        {
            InitializeComponent();
        }

        public void AddTimer(DateTime time, ICalendarTimer timer)
        {
            ListViewItem item = new ListViewItem();
            item.Tag = new TimerItem()
            {
                Time = time,
                Timer = timer
            };
            item.Text = time.ToString("HH:mm:ss");
            item.SubItems.Add(timer.Name);
            item.SubItems.Add(timer.Description);
            item.SubItems.Add("");
            UpdateItem(item);
            listViewTimers.Items.Add(item);
        }

        private void UpdateItem(ListViewItem item)
        {
            DateTime happen = (item.Tag as TimerItem).Time;
            DateTime now = DateTime.Now;
            DateTime over = new DateTime(DateTime.Today.Ticks + (now - happen).Ticks);
            item.SubItems[3].Text = over.ToString("HH:mm:ss");
        }

        private void timerUpdate_Tick(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listViewTimers.Items)
            {
                UpdateItem(item);
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
