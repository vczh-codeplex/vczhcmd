using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Funcmd.Scripting;
using System.Xml.Linq;

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
            if (command == "command")
            {
                using (CommandEditorForm form = new CommandEditorForm())
                {
                    form.ShowDialog();
                }
                return true;
            }
            else
            {
                try
                {
                    ScriptingValue value = scriptingEnvironment.ParseValue(command);
                    callback.ShowMessage(value.ToString());
                    return true;
                }
                catch (Exception ex)
                {
                    error = ex;
                    return false;
                }
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
