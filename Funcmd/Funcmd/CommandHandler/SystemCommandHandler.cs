using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            else
            {
                return false;
            }
        }
    }
}
