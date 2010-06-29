using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Funcmd.CommandHandler;
using System.Xml.Linq;

namespace Funcmd.ScriptingCommandFramework
{
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
