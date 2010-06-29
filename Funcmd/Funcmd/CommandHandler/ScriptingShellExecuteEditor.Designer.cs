namespace Funcmd.CommandHandler
{
    partial class ScriptingShellExecuteEditor
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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textName = new System.Windows.Forms.TextBox();
            this.textExecutable = new System.Windows.Forms.TextBox();
            this.textParameter = new System.Windows.Forms.TextBox();
            this.tableEditor.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableEditor
            // 
            this.tableEditor.ColumnCount = 2;
            this.tableEditor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableEditor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 51.65975F));
            this.tableEditor.Controls.Add(this.label1, 0, 0);
            this.tableEditor.Controls.Add(this.label2, 0, 1);
            this.tableEditor.Controls.Add(this.label3, 0, 2);
            this.tableEditor.Controls.Add(this.textName, 1, 0);
            this.tableEditor.Controls.Add(this.textExecutable, 1, 1);
            this.tableEditor.Controls.Add(this.textParameter, 1, 2);
            this.tableEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableEditor.Location = new System.Drawing.Point(0, 0);
            this.tableEditor.Name = "tableEditor";
            this.tableEditor.RowCount = 3;
            this.tableEditor.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableEditor.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableEditor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 134F));
            this.tableEditor.Size = new System.Drawing.Size(482, 332);
            this.tableEditor.TabIndex = 0;
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
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 35);
            this.label2.Margin = new System.Windows.Forms.Padding(3, 8, 3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "程序：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 62);
            this.label3.Margin = new System.Windows.Forms.Padding(3, 8, 3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "参数：";
            // 
            // textName
            // 
            this.textName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textName.Location = new System.Drawing.Point(74, 3);
            this.textName.Name = "textName";
            this.textName.Size = new System.Drawing.Size(405, 21);
            this.textName.TabIndex = 3;
            // 
            // textExecutable
            // 
            this.textExecutable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textExecutable.Location = new System.Drawing.Point(74, 30);
            this.textExecutable.Name = "textExecutable";
            this.textExecutable.Size = new System.Drawing.Size(405, 21);
            this.textExecutable.TabIndex = 4;
            // 
            // textParameter
            // 
            this.textParameter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textParameter.Location = new System.Drawing.Point(74, 57);
            this.textParameter.Multiline = true;
            this.textParameter.Name = "textParameter";
            this.textParameter.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textParameter.Size = new System.Drawing.Size(405, 272);
            this.textParameter.TabIndex = 5;
            // 
            // ScriptingShellExecuteEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableEditor);
            this.Name = "ScriptingShellExecuteEditor";
            this.Size = new System.Drawing.Size(482, 332);
            this.tableEditor.ResumeLayout(false);
            this.tableEditor.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableEditor;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textName;
        private System.Windows.Forms.TextBox textExecutable;
        private System.Windows.Forms.TextBox textParameter;
    }
}
