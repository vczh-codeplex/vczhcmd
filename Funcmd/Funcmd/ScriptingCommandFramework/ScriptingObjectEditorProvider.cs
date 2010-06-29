using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Funcmd.CommandHandler;

namespace Funcmd.ScriptingCommandFramework
{
    public class ScriptingObjectEditorProvider : IObjectEditorProvider
    {
        private IObjectEditorType[] types;
        private List<IObjectEditorObject> objects = new List<IObjectEditorObject>();

        public ScriptingObjectEditorProvider(ICommandHandlerCallback callback)
        {
            types = new IObjectEditorType[]
            {
                new ScriptingShellExecuteType(this),
                new ScriptingFileType(this, callback)
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
}
