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
    public partial class ScriptingFileEditor : UserControl
    {
        ScriptingFileCommand command = null;

        public ScriptingFileEditor()
        {
            InitializeComponent();
        }

        public void Edit(ScriptingFileCommand command)
        {
            this.command = command;
            textName.Text = command.Name;
        }

        public void Save()
        {
            command.Name = textName.Text;
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
        }

        private void menuItemFile_Click(object sender, EventArgs e)
        {
        }

        private void menuItemDirectory_Click(object sender, EventArgs e)
        {
        }

        private void menuItemUrl_Click(object sender, EventArgs e)
        {
        }

        private void buttonProperties_Click(object sender, EventArgs e)
        {
        }
    }
}
