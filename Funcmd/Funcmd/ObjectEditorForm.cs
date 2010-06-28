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

            foreach (IObjectEditorType type in provider.Types)
            {
                ToolStripMenuItem item = new ToolStripMenuItem()
                {
                    Text = type.Name,
                    Tag = type,
                };
                item.Click += new EventHandler(item_Click);
                buttonAdd.DropDownItems.Add(item);
            }
        }

        private void item_Click(object sender, EventArgs e)
        {
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
        IObjectEditorType[] Types { get; }
        IList<IObjectEditorObject> Objects { get; }
    }

    public interface IObjectEditorType
    {
        string Name { get; }
        IObjectEditorObject CreateObject();
        Control EditObject(IObjectEditorObject obj);
    }

    public interface IObjectEditorObject
    {
        string Name { get; }
        IObjectEditorType Type { get; }
    }
}
