namespace Funcmd.CalendarTimer
{
    partial class EventTimerPlugin
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
            this.tableTimer = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dateTimeDate = new System.Windows.Forms.DateTimePicker();
            this.dateTimeTime = new System.Windows.Forms.DateTimePicker();
            this.tableTimer.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableTimer
            // 
            this.tableTimer.ColumnCount = 2;
            this.tableTimer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableTimer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 83.8362F));
            this.tableTimer.Controls.Add(this.label1, 0, 0);
            this.tableTimer.Controls.Add(this.label2, 0, 1);
            this.tableTimer.Controls.Add(this.dateTimeDate, 1, 0);
            this.tableTimer.Controls.Add(this.dateTimeTime, 1, 1);
            this.tableTimer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableTimer.Location = new System.Drawing.Point(0, 0);
            this.tableTimer.Name = "tableTimer";
            this.tableTimer.RowCount = 3;
            this.tableTimer.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableTimer.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableTimer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableTimer.Size = new System.Drawing.Size(464, 174);
            this.tableTimer.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 27);
            this.label1.TabIndex = 0;
            this.label1.Text = "日期：";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 27);
            this.label2.TabIndex = 1;
            this.label2.Text = "时间：";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dateTimeDate
            // 
            this.dateTimeDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dateTimeDate.Location = new System.Drawing.Point(50, 3);
            this.dateTimeDate.Name = "dateTimeDate";
            this.dateTimeDate.Size = new System.Drawing.Size(411, 21);
            this.dateTimeDate.TabIndex = 2;
            // 
            // dateTimeTime
            // 
            this.dateTimeTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dateTimeTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateTimeTime.Location = new System.Drawing.Point(50, 30);
            this.dateTimeTime.Name = "dateTimeTime";
            this.dateTimeTime.Size = new System.Drawing.Size(411, 21);
            this.dateTimeTime.TabIndex = 3;
            // 
            // EventTimerPlugin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableTimer);
            this.Name = "EventTimerPlugin";
            this.Size = new System.Drawing.Size(464, 174);
            this.tableTimer.ResumeLayout(false);
            this.tableTimer.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableTimer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dateTimeDate;
        private System.Windows.Forms.DateTimePicker dateTimeTime;
    }
}
