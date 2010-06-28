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
        private ScriptingObjectEditorProvider provider;

        public ScriptingCommandHandler(ICommandHandlerCallback callback)
        {
            this.callback = callback;
            provider = new ScriptingObjectEditorProvider();
        }

        public bool HandleCommand(string command, ref Exception error)
        {
            if (command == "command")
            {
                using (ObjectEditorForm form = new ObjectEditorForm(provider))
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

    public class ScriptingObjectEditorProvider : IObjectEditorProvider
    {
        public string Title
        {
            get
            {
                return "命令编辑器";
            }
        }

        public string Header
        {
            get
            {
                return "命令名称";
            }
        }
    }
}
