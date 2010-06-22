using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Funcmd.Scripting;

namespace Funcmd.CommandHandler
{
    public class ScriptingCommandHandler : ICommandHandler
    {
        private ScriptingEnvironment scriptingEnvironment = new Scripting.Scripting().Parse(null);
        private ICommandHandlerCallback callback;

        public ScriptingCommandHandler(ICommandHandlerCallback callback)
        {
            this.callback = callback;
        }

        public bool HandleCommand(string command, ref Exception error)
        {
            try
            {
                ScriptingValue value = scriptingEnvironment.ParseValue(command);
                callback.ShowMessage(value.Value.ToString());
                return true;
            }
            catch (Exception ex)
            {
                error = ex;
                return false;
            }
        }
    }
}
