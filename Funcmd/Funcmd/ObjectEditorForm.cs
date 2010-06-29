using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Funcmd.CommandHandler;

namespace Funcmd
{
    public partial class ObjectEditorForm : Form
    {
        private IObjectEditorProvider provider;
        private ICommandHandlerCallback callback;
        private IObjectEditorObject lastObject = null;
        private ListViewItem lastItem = null;

        public ObjectEditorForm(IObjectEditorProvider provider, ICommandHandlerCallback callback)
        {
            InitializeComponent();
            this.provider = provider;
            this.callback = callback;
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

            foreach (IObjectEditorObject obj in provider.Objects.OrderBy(o => o.Name))
            {
                ListViewItem item = new ListViewItem(obj.Name);
                item.Tag = obj;
                listViewCommands.Items.Add(item);
            }
        }

        private void item_Click(object sender, EventArgs e)
        {
            IObjectEditorType type = (sender as ToolStripMenuItem).Tag as IObjectEditorType;
            IObjectEditorObject obj = type.CreateObject();
            obj.Name = "未命名";
            provider.Objects.Add(obj);

            ListViewItem item = new ListViewItem(obj.Name);
            item.Tag = obj;
            listViewCommands.Items.Add(item);
            item.Selected = true;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            listViewCommands.SelectedIndices.Clear();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void listViewCommands_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lastObject != null)
            {
                lastObject.Type.Save();
                lastItem.Text = lastObject.Name;
                panelEditor.Controls.Clear();
                lastObject = null;
                lastItem = null;
            }
            if (listViewCommands.SelectedIndices.Count > 0)
            {
                lastItem = listViewCommands.SelectedItems[0];
                lastObject = (IObjectEditorObject)lastItem.Tag;
                Control editor = lastObject.Type.EditObject(lastObject);
                editor.Dock = DockStyle.Fill;
                panelEditor.Controls.Add(editor);
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (listViewCommands.SelectedItems.Count == 0)
            {
                callback.ShowError("必须选中命令后才能删除。");
            }
            else
            {
                provider.Objects.Remove(lastObject);
                listViewCommands.Items.Remove(lastItem);
                lastItem = null;
                lastObject = null;
            }
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
        void Save();
    }

    public interface IObjectEditorObject
    {
        string Name { get; set; }
        IObjectEditorType Type { get; }
    }
}
