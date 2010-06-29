using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Funcmd.CommandHandler
{
    public class SystemCommandHandler : ICommandHandler
    {
        private ICommandHandlerCallback callback;

        public SystemCommandHandler(ICommandHandlerCallback callback)
        {
            this.callback = callback;
        }

        public event EventHandler SuggestedCommandsChanged;

        public string[] SuggestedCommands
        {
            get
            {
                return new string[] { "exit", "code", "command view", "month view" };
            }
        }

        public bool HandleCommand(string command, ref Exception error)
        {
            if (command == "exit")
            {
                callback.DoExit();
                return true;
            }
            else if (command == "code")
            {
                callback.OpenCodeForm();
                return true;
            }
            else if (command == "command view")
            {
                callback.ApplyCommandView();
                return true;
            }
            else if (command == "month view")
            {
                callback.ApplyMonthView();
                return true;
            }
            else
            {
                return false;
            }
        }

        public void LoadSetting(XElement settingRoot)
        {
        }

        public void SaveSetting(XElement settingRoot)
        {
        }

        private void InvokeSuggestedCommandChanged()
        {
            if (SuggestedCommandsChanged != null)
            {
                SuggestedCommandsChanged(this, new EventArgs());
            }
        }
    }
}
