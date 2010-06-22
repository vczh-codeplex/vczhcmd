using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Funcmd.CommandHandler
{
    public interface ISystemCommandHandlerCallback
    {
        void DoExit();
    }

    public class SystemCommandHandler : ICommandHandler
    {
        private ISystemCommandHandlerCallback callback;

        public SystemCommandHandler(ISystemCommandHandlerCallback callback)
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
