namespace Funcmd.CommandHandler
{
    partial class ScriptingFileEditor
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableEditor = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textName = new System.Windows.Forms.TextBox();
            this.panelPaths = new System.Windows.Forms.Panel();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeaderName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.buttonAdd = new System.Windows.Forms.ToolStripDropDownButton();
            this.menuItemFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemDirectory = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemUrl = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonDelete = new System.Windows.Forms.ToolStripButton();
            this.buttonProperties = new System.Windows.Forms.ToolStripButton();
            this.tableEditor.SuspendLayout();
            this.panelPaths.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableEditor
            // 
            this.tableEditor.ColumnCount = 2;
            this.tableEditor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableEditor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableEditor.Controls.Add(this.label1, 0, 0);
            this.tableEditor.Controls.Add(this.label3, 0, 1);
            this.tableEditor.Controls.Add(this.textName, 1, 0);
            this.tableEditor.Controls.Add(this.panelPaths, 1, 1);
            this.tableEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableEditor.Location = new System.Drawing.Point(0, 0);
            this.tableEditor.Name = "tableEditor";
            this.tableEditor.RowCount = 2;
            this.tableEditor.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableEditor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 134F));
            this.tableEditor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableEditor.Size = new System.Drawing.Size(453, 397);
            this.tableEditor.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 8);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 8, 3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "命令名称：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 35);
            this.label3.Margin = new System.Windows.Forms.Padding(3, 8, 3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "路径列表：";
            // 
            // textName
            // 
            this.textName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textName.Location = new System.Drawing.Point(74, 3);
            this.textName.Name = "textName";
            this.textName.Size = new System.Drawing.Size(376, 21);
            this.textName.TabIndex = 3;
            // 
            // panelPaths
            // 
            this.panelPaths.Controls.Add(this.listView1);
            this.panelPaths.Controls.Add(this.toolStrip1);
            this.panelPaths.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPaths.Location = new System.Drawing.Point(74, 30);
            this.panelPaths.Name = "panelPaths";
            this.panelPaths.Size = new System.Drawing.Size(376, 364);
            this.panelPaths.TabIndex = 4;
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderName});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.Location = new System.Drawing.Point(0, 25);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(376, 339);
            this.listView1.TabIndex = 1;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderName
            // 
            this.columnHeaderName.Text = "路径";
            this.columnHeaderName.Width = 364;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.buttonAdd,
            this.buttonProperties,
            this.buttonDelete});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(376, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // buttonAdd
            // 
            this.buttonAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.buttonAdd.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemFile,
            this.menuItemDirectory,
            this.menuItemUrl});
            this.buttonAdd.Image = global::Funcmd.ImageResource.AddCommand;
            this.buttonAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(29, 22);
            // 
            // menuItemFile
            // 
            this.menuItemFile.Name = "menuItemFile";
            this.menuItemFile.Size = new System.Drawing.Size(148, 22);
            this.menuItemFile.Text = "文件...";
            this.menuItemFile.Click += new System.EventHandler(this.menuItemFile_Click);
            // 
            // menuItemDirectory
            // 
            this.menuItemDirectory.Name = "menuItemDirectory";
            this.menuItemDirectory.Size = new System.Drawing.Size(148, 22);
            this.menuItemDirectory.Text = "文件夹...";
            this.menuItemDirectory.Click += new System.EventHandler(this.menuItemDirectory_Click);
            // 
            // menuItemUrl
            // 
            this.menuItemUrl.Name = "menuItemUrl";
            this.menuItemUrl.Size = new System.Drawing.Size(148, 22);
            this.menuItemUrl.Text = "网址以及其他";
            this.menuItemUrl.Click += new System.EventHandler(this.menuItemUrl_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.buttonDelete.Image = global::Funcmd.ImageResource.DeleteCommand;
            this.buttonDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(23, 22);
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonProperties
            // 
            this.buttonProperties.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.buttonProperties.Image = global::Funcmd.ImageResource.Properties;
            this.buttonProperties.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonProperties.Name = "buttonProperties";
            this.buttonProperties.Size = new System.Drawing.Size(23, 22);
            this.buttonProperties.Click += new System.EventHandler(this.buttonProperties_Click);
            // 
            // ScriptingFileEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableEditor);
            this.Name = "ScriptingFileEditor";
            this.Size = new System.Drawing.Size(453, 397);
            this.tableEditor.ResumeLayout(false);
            this.tableEditor.PerformLayout();
            this.panelPaths.ResumeLayout(false);
            this.panelPaths.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableEditor;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textName;
        private System.Windows.Forms.Panel panelPaths;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton buttonDelete;
        private System.Windows.Forms.ToolStripDropDownButton buttonAdd;
        private System.Windows.Forms.ToolStripMenuItem menuItemFile;
        private System.Windows.Forms.ToolStripMenuItem menuItemDirectory;
        private System.Windows.Forms.ToolStripMenuItem menuItemUrl;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeaderName;
        private System.Windows.Forms.ToolStripButton buttonProperties;
    }
}
