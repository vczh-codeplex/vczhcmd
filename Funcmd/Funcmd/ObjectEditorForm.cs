using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Funcmd
{
    public partial class ObjectEditorForm : Form
    {
        private IObjectEditorProvider provider;

        public ObjectEditorForm(IObjectEditorProvider provider)
        {
            InitializeComponent();
            this.provider = provider;
            Text = provider.Title;
            columnHeaderName.Text = provider.Header;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }

    public interface IObjectEditorProvider
    {
        string Title { get; }
        string Header { get; }
    }
}
