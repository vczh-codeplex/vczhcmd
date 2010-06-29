using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Funcmd.Scripting;
using System.Xml.Linq;
using System.Windows.Forms;

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
            provider = new ScriptingObjectEditorProvider();
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
    }

    public class ScriptingObjectEditorProvider : IObjectEditorProvider
    {
        private IObjectEditorType[] types;
        private List<IObjectEditorObject> objects = new List<IObjectEditorObject>();

        public ScriptingObjectEditorProvider()
        {
            types = new IObjectEditorType[]
            {
                new ScriptingShellExecuteType(this)
            };
        }

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

        public IObjectEditorType[] Types
        {
            get
            {
                return types;
            }
        }

        public IList<IObjectEditorObject> Objects
        {
            get
            {
                return objects;
            }
        }

        public void Load(List<ScriptingCommand> commands)
        {
            objects.Clear();
            objects.AddRange(commands.Select(c => c.CloneCommand()));
        }

        public void Save(List<ScriptingCommand> commands)
        {
            commands.Clear();
            commands.AddRange(objects.Cast<ScriptingCommand>());
            objects.Clear();
        }
    }

    public abstract class ScriptingCommand : IObjectEditorObject
    {
        public ScriptingCommand(ScriptingObjectEditorProvider provider)
        {
            this.Provider = provider;
            this.Name = "";
        }

        public string Name { get; set; }
        public ScriptingObjectEditorProvider Provider { get; private set; }

        public IObjectEditorType Type
        {
            get
            {
                return Provider.Types.Where(t => t.GetType().AssemblyQualifiedName == CommandType).First();
            }
        }

        public abstract void LoadSetting(XElement element);
        public abstract void SaveSetting(XElement element);
        public abstract string CommandType { get; }
        public abstract ScriptingCommand CloneCommand();
        public abstract void ExecuteCommand(ICommandHandlerCallback callback);
    }
}
