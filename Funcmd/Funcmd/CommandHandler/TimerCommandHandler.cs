using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Funcmd.CalendarTimer;
using System.Xml.Linq;
using System.Windows.Forms;

namespace Funcmd.CommandHandler
{
    public class TimerCommandHandler : ICommandHandler
    {
        private ICommandHandlerCallback callback;
        private List<ICalendarTimer> timers = new List<ICalendarTimer>();
        private CalendarTimerProvider provider;

        public TimerCommandHandler(ICommandHandlerCallback callback)
        {
            this.callback = callback;
            this.provider = new CalendarTimerProvider();
        }

        public event EventHandler SuggestedCommandsChanged;
        public event EventHandler TimersChanged;

        public string[] SuggestedCommands
        {
            get
            {
                return new string[] { "event" };
            }
        }

        public IEnumerable<ICalendarTimer> Timers
        {
            get
            {
                return timers;
            }
        }

        public bool HandleCommand(string command, ref Exception error)
        {
            if (command == "event")
            {
                provider.Load(timers);
                using (ObjectEditorForm form = new ObjectEditorForm(provider, callback))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        provider.Save(timers);
                        callback.SaveSettings();
                        InvokeTimersChanged();
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public void LoadSetting(XElement settingRoot)
        {
            timers.Clear();
            foreach (XElement element in settingRoot.Elements("Timer"))
            {
                IObjectEditorType type = provider.Types.Where(t => t.GetType().AssemblyQualifiedName == element.Attribute("Class").Value).FirstOrDefault();
                if (type != null)
                {
                    ICalendarTimer timer = (ICalendarTimer)type.CreateObject();
                    timer.LoadSetting(element);
                    timers.Add(timer);
                }
            }
        }

        public void SaveSetting(XElement settingRoot)
        {
            foreach (ICalendarTimer timer in timers)
            {
                XElement element = new XElement("Timer");
                element.Add(new XAttribute("Class", timer.Type.GetType().AssemblyQualifiedName));
                timer.SaveSetting(element);
                settingRoot.Add(element);
            }
        }

        private void InvokeSuggestedCommandChanged()
        {
            if (SuggestedCommandsChanged != null)
            {
                SuggestedCommandsChanged(this, new EventArgs());
            }
        }

        private void InvokeTimersChanged()
        {
            if (TimersChanged != null)
            {
                TimersChanged(this, new EventArgs());
            }
        }
    }
}
