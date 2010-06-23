using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Funcmd.Scripting;
using Funcmd.CommandHandler;

namespace Funcmd
{
    public partial class CodeForm : Form
    {
        private ScriptingEnvironment env = null;
        private ICommandHandlerCallback callback;

        public CodeForm(ICommandHandlerCallback callback)
        {
            this.callback = callback;
            InitializeComponent();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonRun_Click(object sender, EventArgs e)
        {
        }

        private void tabCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabCode.SelectedTab == tabPageInterpretor)
            {
                try
                {
                    env = new Scripting.Scripting().Parse(textCode.Text);
                }
                catch (ScriptingException ex)
                {
                    if (ex.Start != -1)
                    {
                        textCode.Select(ex.Start, ex.Length);
                    }
                    callback.ShowError(ex.Message);
                    tabCode.SelectedTab = tabPageEditor;
                }
                catch (Exception ex)
                {
                    callback.ShowError(ex.Message);
                    tabCode.SelectedTab = tabPageEditor;
                }
            }
        }
    }
}
