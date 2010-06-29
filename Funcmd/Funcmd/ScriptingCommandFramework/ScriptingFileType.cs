using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Funcmd.CommandHandler;

namespace Funcmd.ScriptingCommandFramework
{
    public class ScriptingFileType : IObjectEditorType
    {
        private ScriptingObjectEditorProvider provider;
        private ScriptingFileEditor editor = null;

        public ScriptingFileType(ScriptingObjectEditorProvider provider, ICommandHandlerCallback callback)
        {
            this.provider = provider;
            this.editor = new ScriptingFileEditor(callback);
        }

        public string Name
        {
            get
            {
                return "快捷方式";
            }
        }

        public IObjectEditorObject CreateObject()
        {
            return new ScriptingFileCommand(provider);
        }

        public System.Windows.Forms.Control EditObject(IObjectEditorObject obj)
        {
            editor.Edit((ScriptingFileCommand)obj);
            return editor;
        }

        public void Save()
        {
            editor.Save();
        }
    }
}
