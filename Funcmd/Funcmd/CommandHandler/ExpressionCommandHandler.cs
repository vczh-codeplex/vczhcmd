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
    public class ExpressionCommandHandler : ICommandHandler
    {
        private ScriptingEnvironment scriptingEnvironment = new ScriptingEnvironment();
        private ICommandHandlerCallback callback;

        public ExpressionCommandHandler(ICommandHandlerCallback callback)
        {
            this.callback = callback;
        }

        public event EventHandler SuggestedCommandsChanged;

        public string[] SuggestedCommands
        {
            get
            {
                return new string[] { "" };
            }
        }

        public bool HandleCommand(string command, ref Exception error)
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

        public void LoadSetting(XElement settingRoot)
        {
        }

        public void SaveSetting(XElement settingRoot)
        {
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
