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
    }
}
