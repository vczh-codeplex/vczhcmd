using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Funcmd.CommandHandler;

namespace Funcmd.ScriptingCommandFramework
{
    public partial class ScriptingFileEditor : UserControl
    {
        private ScriptingFileCommand command = null;
        private ICommandHandlerCallback callback;

        public ScriptingFileEditor(ICommandHandlerCallback callback)
        {
            InitializeComponent();
            this.callback = callback;
        }

        public void Edit(ScriptingFileCommand command)
        {
            this.command = command;
            textName.Text = command.Name;
            listViewPaths.Items.Clear();
            listViewPaths.Items.AddRange(command.Paths.Select(p => new ListViewItem(p)).ToArray());
        }

        public void Save()
        {
            command.Name = textName.Text;
            command.Paths = listViewPaths.Items.Cast<ListViewItem>().Select(i => i.Text).ToList();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (listViewPaths.SelectedIndices.Count == 0)
            {
                callback.ShowError("必须选中路径后才能删除。");
            }
            else
            {
                int[] indices = listViewPaths.SelectedIndices.Cast<int>().OrderByDescending(i => i).ToArray();
                foreach (int index in indices)
                {
                    listViewPaths.Items.RemoveAt(index);
                }
            }
        }

        private void menuItemFile_Click(object sender, EventArgs e)
        {
            if (dialogOpen.ShowDialog() == DialogResult.OK)
            {
                ListViewItem item = new ListViewItem(dialogOpen.FileName);
                listViewPaths.Items.Add(item);
                listViewPaths.SelectedIndices.Clear();
                listViewPaths.SelectedIndices.Add(item.Index);
            }
        }

        private void menuItemDirectory_Click(object sender, EventArgs e)
        {
            if (dialogFolder.ShowDialog() == DialogResult.OK)
            {
                ListViewItem item = new ListViewItem(dialogFolder.SelectedPath);
                listViewPaths.Items.Add(item);
                listViewPaths.SelectedIndices.Clear();
                listViewPaths.SelectedIndices.Add(item.Index);
            }
        }

        private void menuItemUrl_Click(object sender, EventArgs e)
        {
            ListViewItem item = new ListViewItem("请指定路径");
            listViewPaths.Items.Add(item);
            listViewPaths.SelectedIndices.Clear();
            listViewPaths.SelectedIndices.Add(item.Index);
            buttonProperties_Click(buttonProperties, new EventArgs());
        }

        private void buttonProperties_Click(object sender, EventArgs e)
        {
            if (listViewPaths.SelectedIndices.Count == 0)
            {
                callback.ShowError("必须选中一个路径后才能编辑。");
            }
            else if (listViewPaths.SelectedIndices.Count == 1)
            {
                using (ScriptingFileItemPropertyForm form = new ScriptingFileItemPropertyForm())
                {
                    form.Path = listViewPaths.SelectedItems[0].Text;
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        listViewPaths.SelectedItems[0].Text = form.Path;
                    }
                }
            }
            else
            {
                callback.ShowError("一次只能编辑一个路径。");
            }
        }
    }
}
