using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Funcmd.CommandHandler
{
    public class ScriptingShellExecuteCommand : ScriptingCommand
    {
        public ScriptingShellExecuteCommand(ScriptingObjectEditorProvider provider)
            : base(provider)
        {
            Executable = "";
            Parameter = "";
        }

        public string Executable { get; set; }
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
        private ScriptingShellExecuteEditor editor;

        public ScriptingShellExecuteType(ScriptingObjectEditorProvider provider)
        {
            this.provider = provider;
            this.editor = new ScriptingShellExecuteEditor();
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

        public Control EditObject(IObjectEditorObject obj)
        {
            editor.Edit((ScriptingShellExecuteCommand)obj);
            return editor;
        }

        public void Save()
        {
            editor.Save();
        }
    }
}
