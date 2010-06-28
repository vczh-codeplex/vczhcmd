using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Funcmd.CommandHandler
{
    public partial class ScriptingShellExecuteEditor : UserControl
    {
        private ScriptingShellExecuteCommand command;

        public ScriptingShellExecuteEditor()
        {
            InitializeComponent();
            this.command = null;
        }

        public void Edit(ScriptingShellExecuteCommand command)
        {
            this.command = command;
            textName.Text = command.Name;
            textExecutable.Text = command.Executable;
            textParameter.Text = command.Parameter;
        }

        public void Save()
        {
            command.Name = textName.Text;
            command.Executable = textExecutable.Text;
            command.Parameter = textParameter.Text;
        }
    }
}
