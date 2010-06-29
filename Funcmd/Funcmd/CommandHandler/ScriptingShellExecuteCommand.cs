using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Diagnostics;
using System.IO;

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
                Executable = Executable,
                Parameter = Parameter
            };
        }

        public override void LoadSetting(XElement element)
        {
            Name = element.Attribute("Name").Value;
            Executable = element.Attribute("Executable").Value;
            Parameter = element.Attribute("Parameter").Value;
        }

        public override void SaveSetting(XElement element)
        {
            element.Add(new XAttribute("Name", Name));
            element.Add(new XAttribute("Executable", Executable));
            element.Add(new XAttribute("Parameter", Parameter));
        }

        public override void ExecuteCommand(ICommandHandlerCallback callback)
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.Arguments = Parameter;
            info.FileName = Executable;
            info.UseShellExecute = true;
            info.Verb = "OPEN";
            try
            {
                info.WorkingDirectory = Path.GetDirectoryName(Executable);
            }
            catch (Exception)
            {
            }
            Process.Start(info);
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
