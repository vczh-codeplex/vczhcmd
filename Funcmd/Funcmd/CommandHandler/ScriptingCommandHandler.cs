using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Funcmd.Scripting;
using System.Xml.Linq;
using System.Windows.Forms;
using Funcmd.ScriptingCommandFramework;

namespace Funcmd.CommandHandler
{
    public class ScriptingCommandHandler : ICommandHandler
    {
        private ScriptingEnvironment scriptingEnvironment = new Scripting.Scripting().Parse(null);
        private ICommandHandlerCallback callback;
        private ScriptingObjectEditorProvider provider;
        private List<ScriptingCommand> commands = new List<ScriptingCommand>();

        public ScriptingCommandHandler(ICommandHandlerCallback callback)
        {
            this.callback = callback;
            provider = new ScriptingObjectEditorProvider(callback);
        }

        public event EventHandler SuggestedCommandsChanged;

        public string[] SuggestedCommands
        {
            get
            {
                return new string[] { "command" }.Concat(commands.Select(c => c.Name)).ToArray();
            }
        }

        public bool HandleCommand(string command, ref Exception error)
        {
            if (command == "command")
            {
                provider.Load(commands);
                using (ObjectEditorForm form = new ObjectEditorForm(provider, callback))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        provider.Save(commands);
                        callback.SaveSettings();
                        InvokeSuggestedCommandChanged();
                    }
                }
                return true;
            }
            else
            {
                try
                {
                    ScriptingCommand scriptingCommand = commands.Where(c => c.Name == command).FirstOrDefault();
                    if (scriptingCommand != null)
                    {
                        scriptingCommand.ExecuteCommand(callback);
                        return true;
                    }
                    else
                    {
                        ScriptingValue value = scriptingEnvironment.ParseValue(command);
                        callback.ShowMessage(value.ToString());
                        return true;
                    }
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
            commands.Clear();
            foreach (XElement element in settingRoot.Elements("ScriptingCommand"))
            {
                IObjectEditorType type = provider.Types.Where(t => t.GetType().AssemblyQualifiedName == element.Attribute("Class").Value).FirstOrDefault();
                if (type != null)
                {
                    ScriptingCommand command = (ScriptingCommand)type.CreateObject();
                    command.LoadSetting(element);
                    commands.Add(command);
                }
            }
        }

        public void SaveSetting(XElement settingRoot)
        {
            foreach (ScriptingCommand command in commands)
            {
                XElement element = new XElement("ScriptingCommand");
                element.Add(new XAttribute("Class", command.Type.GetType().AssemblyQualifiedName));
                command.SaveSetting(element);
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
    }
}
