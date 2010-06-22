using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Funcmd.CommandHandler
{
    public interface ICommandHandler
    {
        bool HandleCommand(string command, ref Exception error);
    }

    public class CommandHandlerManager
    {
        private List<ICommandHandler> handlers = new List<ICommandHandler>();

        public CommandHandlerManager()
        {
        }

        public void AddCommandHandler(ICommandHandler handler)
        {
            handlers.Add(handler);
        }

        public void HandleCommand(string command)
        {
            Exception error = null;
            foreach (ICommandHandler handler in handlers)
            {
                if (handler.HandleCommand(command, ref error))
                {
                    return;
                }
            }
            throw error;
        }
    }
}
