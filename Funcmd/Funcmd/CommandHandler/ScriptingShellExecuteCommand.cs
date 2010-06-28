using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Funcmd.CommandHandler
{
    public class ScriptingShellExecuteCommand : ScriptingCommand
    {
        public ScriptingShellExecuteCommand(ScriptingObjectEditorProvider provider)
            : base(provider)
        {
            Parameter = "";
        }

        public string Parameter { get; set; }

        public override string CommandType
        {
            get
            {
                return typeof(ScriptingShellExecuteType).AssemblyQualifiedName;
            }
        }

        public override ScriptingCommand CloneCommand()
        {
            return new ScriptingShellExecuteCommand(Provider)
            {
                Name = Name,
                Parameter = Parameter
            };
        }
    }

    public class ScriptingShellExecuteType : IObjectEditorType
    {
        private ScriptingObjectEditorProvider provider;

        public ScriptingShellExecuteType(ScriptingObjectEditorProvider provider)
        {
            this.provider = provider;
        }

        public string Name
        {
            get
            {
                return "命令行";
            }
        }

        public IObjectEditorObject CreateObject()
        {
            return new ScriptingShellExecuteCommand(provider);
        }

        public System.Windows.Forms.Control EditObject(IObjectEditorObject obj)
        {
            throw new NotImplementedException();
        }
    }
}
