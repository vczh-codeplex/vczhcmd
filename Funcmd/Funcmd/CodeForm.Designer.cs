namespace Funcmd
{
    partial class CodeForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CodeForm));
            this.tableCode = new System.Windows.Forms.TableLayoutPanel();
            this.buttonClose = new System.Windows.Forms.Button();
            this.tabCode = new System.Windows.Forms.TabControl();
            this.tabPageEditor = new System.Windows.Forms.TabPage();
            this.textCode = new System.Windows.Forms.TextBox();
            this.tabPageInterpretor = new System.Windows.Forms.TabPage();
            this.tableInterpretor = new System.Windows.Forms.TableLayoutPanel();
            this.textLaunch = new System.Windows.Forms.TextBox();
            this.textOutput = new System.Windows.Forms.TextBox();
            this.buttonRun = new System.Windows.Forms.Button();
            this.tableCode.SuspendLayout();
            this.tabCode.SuspendLayout();
            this.tabPageEditor.SuspendLayout();
            this.tabPageInterpretor.SuspendLayout();
            this.tableInterpretor.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableCode
            // 
            this.tableCode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tableCode.ColumnCount = 2;
            this.tableCode.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableCode.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableCode.Controls.Add(this.buttonClose, 1, 1);
            this.tableCode.Controls.Add(this.tabCode, 0, 0);
            this.tableCode.Location = new System.Drawing.Point(12, 12);
            this.tableCode.Name = "tableCode";
            this.tableCode.RowCount = 2;
            this.tableCode.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableCode.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableCode.Size = new System.Drawing.Size(692, 494);
            this.tableCode.TabIndex = 0;
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(614, 468);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 1;
            this.buttonClose.Text = "关闭";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // tabCode
            // 
            this.tableCode.SetColumnSpan(this.tabCode, 2);
            this.tabCode.Controls.Add(this.tabPageEditor);
            this.tabCode.Controls.Add(this.tabPageInterpretor);
            this.tabCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabCode.Location = new System.Drawing.Point(3, 3);
            this.tabCode.Name = "tabCode";
            this.tabCode.SelectedIndex = 0;
            this.tabCode.Size = new System.Drawing.Size(686, 459);
            this.tabCode.TabIndex = 0;
            this.tabCode.SelectedIndexChanged += new System.EventHandler(this.tabCode_SelectedIndexChanged);
            // 
            // tabPageEditor
            // 
            this.tabPageEditor.Controls.Add(this.textCode);
            this.tabPageEditor.Location = new System.Drawing.Point(4, 22);
            this.tabPageEditor.Name = "tabPageEditor";
            this.tabPageEditor.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageEditor.Size = new System.Drawing.Size(678, 433);
            this.tabPageEditor.TabIndex = 0;
            this.tabPageEditor.Text = "编辑窗口";
            this.tabPageEditor.UseVisualStyleBackColor = true;
            // 
            // textCode
            // 
            this.textCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textCode.Location = new System.Drawing.Point(3, 3);
            this.textCode.Multiline = true;
            this.textCode.Name = "textCode";
            this.textCode.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textCode.Size = new System.Drawing.Size(672, 427);
            this.textCode.TabIndex = 0;
            // 
            // tabPageInterpretor
            // 
            this.tabPageInterpretor.Controls.Add(this.tableInterpretor);
            this.tabPageInterpretor.Location = new System.Drawing.Point(4, 22);
            this.tabPageInterpretor.Name = "tabPageInterpretor";
            this.tabPageInterpretor.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageInterpretor.Size = new System.Drawing.Size(678, 433);
            this.tabPageInterpretor.TabIndex = 1;
            this.tabPageInterpretor.Text = "运行窗口";
            this.tabPageInterpretor.UseVisualStyleBackColor = true;
            // 
            // tableInterpretor
            // 
            this.tableInterpretor.ColumnCount = 2;
            this.tableInterpretor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableInterpretor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableInterpretor.Controls.Add(this.textLaunch, 0, 1);
            this.tableInterpretor.Controls.Add(this.textOutput, 0, 0);
            this.tableInterpretor.Controls.Add(this.buttonRun, 1, 1);
            this.tableInterpretor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableInterpretor.Location = new System.Drawing.Point(3, 3);
            this.tableInterpretor.Name = "tableInterpretor";
            this.tableInterpretor.RowCount = 2;
            this.tableInterpretor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableInterpretor.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableInterpretor.Size = new System.Drawing.Size(672, 427);
            this.tableInterpretor.TabIndex = 0;
            // 
            // textLaunch
            // 
            this.textLaunch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textLaunch.Location = new System.Drawing.Point(3, 401);
            this.textLaunch.Name = "textLaunch";
            this.textLaunch.Size = new System.Drawing.Size(585, 21);
            this.textLaunch.TabIndex = 0;
            // 
            // textOutput
            // 
            this.tableInterpretor.SetColumnSpan(this.textOutput, 2);
            this.textOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textOutput.Location = new System.Drawing.Point(3, 3);
            this.textOutput.Multiline = true;
            this.textOutput.Name = "textOutput";
            this.textOutput.ReadOnly = true;
            this.textOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textOutput.Size = new System.Drawing.Size(666, 392);
            this.textOutput.TabIndex = 2;
            // 
            // buttonRun
            // 
            this.buttonRun.Location = new System.Drawing.Point(594, 401);
            this.buttonRun.Name = "buttonRun";
            this.buttonRun.Size = new System.Drawing.Size(75, 23);
            this.buttonRun.TabIndex = 1;
            this.buttonRun.Text = "运行";
            this.buttonRun.UseVisualStyleBackColor = true;
            this.buttonRun.Click += new System.EventHandler(this.buttonRun_Click);
            // 
            // CodeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(716, 518);
            this.Controls.Add(this.tableCode);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CodeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "代码试验窗口";
            this.tableCode.ResumeLayout(false);
            this.tabCode.ResumeLayout(false);
            this.tabPageEditor.ResumeLayout(false);
            this.tabPageEditor.PerformLayout();
            this.tabPageInterpretor.ResumeLayout(false);
            this.tableInterpretor.ResumeLayout(false);
            this.tableInterpretor.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableCode;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.TabControl tabCode;
        private System.Windows.Forms.TabPage tabPageEditor;
        private System.Windows.Forms.TabPage tabPageInterpretor;
        private System.Windows.Forms.TextBox textCode;
        private System.Windows.Forms.TableLayoutPanel tableInterpretor;
        private System.Windows.Forms.TextBox textLaunch;
        private System.Windows.Forms.TextBox textOutput;
        private System.Windows.Forms.Button buttonRun;
    }
}