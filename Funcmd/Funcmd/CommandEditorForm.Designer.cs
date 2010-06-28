namespace Funcmd
{
    partial class CommandEditorForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableEditor = new System.Windows.Forms.TableLayoutPanel();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.splitEditor = new System.Windows.Forms.SplitContainer();
            this.panelEditor = new System.Windows.Forms.Panel();
            this.toolStripCommands = new System.Windows.Forms.ToolStrip();
            this.listViewCommands = new System.Windows.Forms.ListView();
            this.tableEditor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitEditor)).BeginInit();
            this.splitEditor.Panel1.SuspendLayout();
            this.splitEditor.Panel2.SuspendLayout();
            this.splitEditor.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableEditor
            // 
            this.tableEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tableEditor.ColumnCount = 3;
            this.tableEditor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableEditor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableEditor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableEditor.Controls.Add(this.buttonOK, 1, 1);
            this.tableEditor.Controls.Add(this.buttonCancel, 2, 1);
            this.tableEditor.Controls.Add(this.splitEditor, 0, 0);
            this.tableEditor.Location = new System.Drawing.Point(12, 12);
            this.tableEditor.Name = "tableEditor";
            this.tableEditor.RowCount = 2;
            this.tableEditor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableEditor.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableEditor.Size = new System.Drawing.Size(608, 434);
            this.tableEditor.TabIndex = 0;
            // 
            // buttonOK
            // 
            this.buttonOK.AutoSize = true;
            this.buttonOK.Location = new System.Drawing.Point(449, 408);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "确定";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.AutoSize = true;
            this.buttonCancel.Location = new System.Drawing.Point(530, 408);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "取消";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // splitEditor
            // 
            this.tableEditor.SetColumnSpan(this.splitEditor, 3);
            this.splitEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitEditor.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitEditor.Location = new System.Drawing.Point(3, 3);
            this.splitEditor.Name = "splitEditor";
            // 
            // splitEditor.Panel1
            // 
            this.splitEditor.Panel1.Controls.Add(this.listViewCommands);
            this.splitEditor.Panel1.Controls.Add(this.toolStripCommands);
            // 
            // splitEditor.Panel2
            // 
            this.splitEditor.Panel2.Controls.Add(this.panelEditor);
            this.splitEditor.Size = new System.Drawing.Size(602, 399);
            this.splitEditor.SplitterDistance = 229;
            this.splitEditor.TabIndex = 2;
            // 
            // panelEditor
            // 
            this.panelEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEditor.Location = new System.Drawing.Point(0, 0);
            this.panelEditor.Name = "panelEditor";
            this.panelEditor.Size = new System.Drawing.Size(369, 399);
            this.panelEditor.TabIndex = 0;
            // 
            // toolStripCommands
            // 
            this.toolStripCommands.Location = new System.Drawing.Point(0, 0);
            this.toolStripCommands.Name = "toolStripCommands";
            this.toolStripCommands.Size = new System.Drawing.Size(229, 25);
            this.toolStripCommands.TabIndex = 0;
            this.toolStripCommands.Text = "toolStrip1";
            // 
            // listViewCommands
            // 
            this.listViewCommands.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewCommands.Location = new System.Drawing.Point(0, 25);
            this.listViewCommands.Name = "listViewCommands";
            this.listViewCommands.Size = new System.Drawing.Size(229, 374);
            this.listViewCommands.TabIndex = 1;
            this.listViewCommands.UseCompatibleStateImageBehavior = false;
            // 
            // CommandEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 458);
            this.Controls.Add(this.tableEditor);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CommandEditorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "命令编辑器";
            this.tableEditor.ResumeLayout(false);
            this.tableEditor.PerformLayout();
            this.splitEditor.Panel1.ResumeLayout(false);
            this.splitEditor.Panel1.PerformLayout();
            this.splitEditor.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitEditor)).EndInit();
            this.splitEditor.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableEditor;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.SplitContainer splitEditor;
        private System.Windows.Forms.Panel panelEditor;
        private System.Windows.Forms.ListView listViewCommands;
        private System.Windows.Forms.ToolStrip toolStripCommands;
    }
}