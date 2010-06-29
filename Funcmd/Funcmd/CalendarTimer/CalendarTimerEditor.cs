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
    public partial class CalendarTimerEditor : UserControl
    {
        private ICalendarTimerEditorPlugin plugin = null;
        private ICalendarTimer timer = null;

        public CalendarTimerEditor(ICalendarTimerEditorPlugin plugin)
        {
            InitializeComponent();
            this.plugin = plugin;
            plugin.Editor.Dock = DockStyle.Fill;
            panelTimer.Controls.Add(plugin.Editor);
        }

        public void Edit(ICalendarTimer timer)
        {
            this.timer = timer;
            textName.Text = timer.Name;
            textDescription.Text = timer.Description;
            checkUrgent.Checked = timer.Urgent;
            checkEnabled.Checked = timer.Enabled;
            plugin.Edit(timer);
        }

        public void Save()
        {
            timer.Name = textName.Text;
            timer.Description = textDescription.Text;
            timer.Urgent = checkUrgent.Checked;
            timer.Enabled = checkEnabled.Checked;
            plugin.Save(timer);
        }
    }

    public interface ICalendarTimerEditorPlugin
    {
        Control Editor { get; }
        void Edit(ICalendarTimer timer);
        void Save(ICalendarTimer timer);
    }
}
