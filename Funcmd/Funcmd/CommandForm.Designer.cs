namespace Funcmd
{
    partial class CommandForm
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
            this.labelCaption = new System.Windows.Forms.Label();
            this.textBoxCommand = new System.Windows.Forms.TextBox();
            this.panelBackground = new System.Windows.Forms.Panel();
            this.tableInfo = new System.Windows.Forms.TableLayoutPanel();
            this.panelCalendar = new System.Windows.Forms.Panel();
            this.tableCommand = new System.Windows.Forms.TableLayoutPanel();
            this.panelBackground.SuspendLayout();
            this.tableInfo.SuspendLayout();
            this.tableCommand.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelCaption
            // 
            this.labelCaption.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.labelCaption.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCaption.Location = new System.Drawing.Point(3, 0);
            this.labelCaption.Name = "labelCaption";
            this.labelCaption.Size = new System.Drawing.Size(154, 100);
            this.labelCaption.TabIndex = 0;
            this.labelCaption.Text = "Label";
            this.labelCaption.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelCaption.MouseDown += new System.Windows.Forms.MouseEventHandler(this.labelCaption_MouseDown);
            this.labelCaption.MouseMove += new System.Windows.Forms.MouseEventHandler(this.labelCaption_MouseMove);
            this.labelCaption.MouseUp += new System.Windows.Forms.MouseEventHandler(this.labelCaption_MouseUp);
            // 
            // textBoxCommand
            // 
            this.textBoxCommand.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxCommand.Location = new System.Drawing.Point(3, 39);
            this.textBoxCommand.Name = "textBoxCommand";
            this.textBoxCommand.Size = new System.Drawing.Size(194, 21);
            this.textBoxCommand.TabIndex = 1;
            // 
            // panelBackground
            // 
            this.panelBackground.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelBackground.Controls.Add(this.tableInfo);
            this.panelBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBackground.Location = new System.Drawing.Point(0, 0);
            this.panelBackground.Name = "panelBackground";
            this.panelBackground.Size = new System.Drawing.Size(462, 102);
            this.panelBackground.TabIndex = 2;
            // 
            // tableInfo
            // 
            this.tableInfo.ColumnCount = 3;
            this.tableInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            this.tableInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableInfo.Controls.Add(this.panelCalendar, 2, 0);
            this.tableInfo.Controls.Add(this.labelCaption, 0, 0);
            this.tableInfo.Controls.Add(this.tableCommand, 1, 0);
            this.tableInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableInfo.Location = new System.Drawing.Point(0, 0);
            this.tableInfo.Name = "tableInfo";
            this.tableInfo.RowCount = 1;
            this.tableInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableInfo.Size = new System.Drawing.Size(460, 100);
            this.tableInfo.TabIndex = 3;
            // 
            // panelCalendar
            // 
            this.panelCalendar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCalendar.Location = new System.Drawing.Point(360, 0);
            this.panelCalendar.Margin = new System.Windows.Forms.Padding(0);
            this.panelCalendar.Name = "panelCalendar";
            this.panelCalendar.Size = new System.Drawing.Size(100, 100);
            this.panelCalendar.TabIndex = 3;
            this.panelCalendar.Paint += new System.Windows.Forms.PaintEventHandler(this.panelCalendar_Paint);
            this.panelCalendar.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panelCalendar_MouseClick);
            this.panelCalendar.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelCalendar_MouseMove);
            // 
            // tableCommand
            // 
            this.tableCommand.ColumnCount = 1;
            this.tableCommand.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableCommand.Controls.Add(this.textBoxCommand, 0, 1);
            this.tableCommand.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableCommand.Location = new System.Drawing.Point(160, 0);
            this.tableCommand.Margin = new System.Windows.Forms.Padding(0);
            this.tableCommand.Name = "tableCommand";
            this.tableCommand.RowCount = 3;
            this.tableCommand.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableCommand.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableCommand.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableCommand.Size = new System.Drawing.Size(200, 100);
            this.tableCommand.TabIndex = 1;
            // 
            // CommandForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(462, 102);
            this.Controls.Add(this.panelBackground);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CommandForm";
            this.Text = "Functional Command";
            this.TopMost = true;
            this.Shown += new System.EventHandler(this.CommandForm_Shown);
            this.panelBackground.ResumeLayout(false);
            this.tableInfo.ResumeLayout(false);
            this.tableCommand.ResumeLayout(false);
            this.tableCommand.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelCaption;
        private System.Windows.Forms.TextBox textBoxCommand;
        private System.Windows.Forms.Panel panelBackground;
        private System.Windows.Forms.TableLayoutPanel tableInfo;
        private System.Windows.Forms.TableLayoutPanel tableCommand;
        private System.Windows.Forms.Panel panelCalendar;
    }
}

