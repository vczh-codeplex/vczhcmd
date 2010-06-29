using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Diagnostics;
using System.IO;

namespace Funcmd.CommandHandler
{
    public class ScriptingFileCommand : ScriptingCommand
    {
        public IList<string> Paths { get; private set; }

        public ScriptingFileCommand(ScriptingObjectEditorProvider provider)
            : base(provider)
        {
            Paths = new List<string>();
        }

        public override void LoadSetting(XElement element)
        {
            Paths = element.Elements("Path").Select(e => e.Value).ToList();
        }

        public override void SaveSetting(XElement element)
        {
            element.Add(Paths.Select(p => new XElement("Path") { Value = p }));
        }

        public override string CommandType
        {
            get
            {
                return typeof(ScriptingFileType).AssemblyQualifiedName;
            }
        }

        public override ScriptingCommand CloneCommand()
        {
            return new ScriptingFileCommand(Provider)
            {
                Name = Name,
                Paths = new List<string>(Paths)
            };
        }

        public override void ExecuteCommand(ICommandHandlerCallback callback)
        {
            foreach (string path in Paths)
            {
                try
                {
                    ProcessStartInfo info = new ProcessStartInfo();
                    info.FileName = path;
                    info.UseShellExecute = true;
                    info.Verb = "OPEN";
                    Process.Start(info);
                }
                catch (Exception ex)
                {
                    callback.ShowError(ex.Message);
                }
            }
        }
    }
}
