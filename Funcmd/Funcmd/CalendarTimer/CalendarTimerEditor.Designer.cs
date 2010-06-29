namespace Funcmd.CalendarTimer
{
    partial class CalendarTimerEditor
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
            this.checkUrgent = new System.Windows.Forms.CheckBox();
            this.checkEnabled = new System.Windows.Forms.CheckBox();
            this.textName = new System.Windows.Forms.TextBox();
            this.textDescription = new System.Windows.Forms.TextBox();
            this.panelTimer = new System.Windows.Forms.Panel();
            this.tableEditor.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableEditor
            // 
            this.tableEditor.ColumnCount = 2;
            this.tableEditor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableEditor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 81.06509F));
            this.tableEditor.Controls.Add(this.label1, 0, 0);
            this.tableEditor.Controls.Add(this.label2, 0, 1);
            this.tableEditor.Controls.Add(this.checkUrgent, 0, 2);
            this.tableEditor.Controls.Add(this.checkEnabled, 0, 3);
            this.tableEditor.Controls.Add(this.textName, 1, 0);
            this.tableEditor.Controls.Add(this.textDescription, 1, 1);
            this.tableEditor.Controls.Add(this.panelTimer, 0, 4);
            this.tableEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableEditor.Location = new System.Drawing.Point(0, 0);
            this.tableEditor.Name = "tableEditor";
            this.tableEditor.RowCount = 5;
            this.tableEditor.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableEditor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 134F));
            this.tableEditor.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableEditor.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableEditor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 17F));
            this.tableEditor.Size = new System.Drawing.Size(507, 461);
            this.tableEditor.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 8);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 8, 3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "名称：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 35);
            this.label2.Margin = new System.Windows.Forms.Padding(3, 8, 3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "描述：";
            // 
            // checkUrgent
            // 
            this.checkUrgent.AutoSize = true;
            this.tableEditor.SetColumnSpan(this.checkUrgent, 2);
            this.checkUrgent.Location = new System.Drawing.Point(3, 164);
            this.checkUrgent.Name = "checkUrgent";
            this.checkUrgent.Size = new System.Drawing.Size(48, 16);
            this.checkUrgent.TabIndex = 2;
            this.checkUrgent.Text = "紧急";
            this.checkUrgent.UseVisualStyleBackColor = true;
            // 
            // checkEnabled
            // 
            this.checkEnabled.AutoSize = true;
            this.tableEditor.SetColumnSpan(this.checkEnabled, 2);
            this.checkEnabled.Location = new System.Drawing.Point(3, 186);
            this.checkEnabled.Name = "checkEnabled";
            this.checkEnabled.Size = new System.Drawing.Size(72, 16);
            this.checkEnabled.TabIndex = 3;
            this.checkEnabled.Text = "使用闹钟";
            this.checkEnabled.UseVisualStyleBackColor = true;
            // 
            // textName
            // 
            this.textName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textName.Location = new System.Drawing.Point(50, 3);
            this.textName.Name = "textName";
            this.textName.Size = new System.Drawing.Size(454, 21);
            this.textName.TabIndex = 4;
            // 
            // textDescription
            // 
            this.textDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textDescription.Location = new System.Drawing.Point(50, 30);
            this.textDescription.Multiline = true;
            this.textDescription.Name = "textDescription";
            this.textDescription.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textDescription.Size = new System.Drawing.Size(454, 128);
            this.textDescription.TabIndex = 5;
            // 
            // panelTimer
            // 
            this.tableEditor.SetColumnSpan(this.panelTimer, 2);
            this.panelTimer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTimer.Location = new System.Drawing.Point(0, 205);
            this.panelTimer.Margin = new System.Windows.Forms.Padding(0);
            this.panelTimer.Name = "panelTimer";
            this.panelTimer.Size = new System.Drawing.Size(507, 256);
            this.panelTimer.TabIndex = 6;
            // 
            // CalendarTimerEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableEditor);
            this.Name = "CalendarTimerEditor";
            this.Size = new System.Drawing.Size(507, 461);
            this.tableEditor.ResumeLayout(false);
            this.tableEditor.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableEditor;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkUrgent;
        private System.Windows.Forms.CheckBox checkEnabled;
        private System.Windows.Forms.TextBox textName;
        private System.Windows.Forms.TextBox textDescription;
        private System.Windows.Forms.Panel panelTimer;
    }
}
