using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Funcmd.Calendar;
using Funcmd.CalendarPainter;
using MonthCalendar = Funcmd.Calendar.MonthCalendar;
using System.Globalization;
using System.Diagnostics;
using System.IO;
using Funcmd.CommandHandler;
using System.Xml.Linq;

namespace Funcmd
{
    public partial class CommandForm
        : Form
        , ICommandHandlerCallback
    {
        private Point lastCursor;
        private Size originalWindowSize;
        private Size originalPanelSize;

        private ICalendar calendar;
        private CalendarPainterFactory factory;
        private SelectorCalendarPainter painter;
        private DateTime focusDay = DateTime.MaxValue;
        private Bitmap calendarBuffer = null;
        private Graphics calendarGraphics = null;

        private CommandHandlerManager commandHandlerManager = null;
        private ICommandHandlerCallback systemCallback = null;
        private string settingPath = null;

        public CommandForm()
        {
            InitializeComponent();
            originalWindowSize = this.Size;
            originalPanelSize = panelCalendar.Size;

            painter = new SelectorCalendarPainter();
            painter.PainterNeeded += new CalendarPainterNeededHandler(painter_PainterNeeded);

            systemCallback = this;
            commandHandlerManager = new CommandHandlerManager(systemCallback);
            commandHandlerManager.AddCommandHandler(new SystemCommandHandler(systemCallback));
            commandHandlerManager.AddCommandHandler(new ShellCommandHandler());
            commandHandlerManager.AddCommandHandler(new ScriptingCommandHandler(systemCallback));

            settingPath = Application.ExecutablePath + ".Settings.xml";
            try
            {
                if (File.Exists(settingPath))
                {
                    XDocument document = XDocument.Load(settingPath);
                    commandHandlerManager.LoadSetting(document.Root);
                }
            }
            catch (Exception ex)
            {
                systemCallback.ShowError(ex.Message);
            }
        }

        private void SetDisplay(ICalendar calendar, CalendarPainterFactory factory)
        {
            this.calendar = calendar;
            this.factory = factory;
            this.calendar.Painter = painter;
            this.Size = originalWindowSize + (this.calendar.CalendarSize - originalPanelSize);
            this.Top = Screen.PrimaryScreen.WorkingArea.Top;
            this.Left = Screen.PrimaryScreen.WorkingArea.Right - 200 - this.Width;

            this.calendar.CalendarDayEntered += new CalendarDaySelectedHandler(calendar_CalendarDayEntered);
            this.calendar.CalendarDaySelected += new CalendarDaySelectedHandler(calendar_CalendarDaySelected);
            this.calendar.CurrentDayChanged += new EventHandler(calendar_CurrentDayChanged);

            if (calendarBuffer != null)
            {
                calendarGraphics.Dispose();
                calendarBuffer.Dispose();
            }
            calendarBuffer = new Bitmap(this.calendar.CalendarSize.Width, this.calendar.CalendarSize.Height);
            calendarGraphics = Graphics.FromImage(calendarBuffer);
            focusDay = DateTime.MaxValue;
            labelCaption.Text = this.calendar.Caption;

            this.calendar.CurrentDay = DateTime.Today;
            this.calendar.MouseMove(new Point(0, 0), panelCalendar.PointToClient(Control.MousePosition));
            DrawCalendar();
        }

        private void DrawCalendar()
        {
            calendar.Draw(calendarGraphics, new Point(0, 0), panelCalendar.PointToClient(Control.MousePosition));
            panelCalendar.Refresh();
        }

        #region ISystemCommandHandlerCallback Members

        void ICommandHandlerCallback.DoExit()
        {
            Close();
        }

        void ICommandHandlerCallback.ShowMessage(string message)
        {
            MessageBox.Show(message, "Functional Command", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        void ICommandHandlerCallback.ShowError(string message)
        {
            MessageBox.Show(message, "Functional Command", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        void ICommandHandlerCallback.OpenCodeForm()
        {
            new CodeForm(systemCallback).Show();
        }

        void ICommandHandlerCallback.ApplyCommandView()
        {
            SetDisplay(new NoCalendar(), new DefaultPainterFactory());
        }

        void ICommandHandlerCallback.ApplyMonthView()
        {
            SetDisplay(new MonthCalendar(), new DefaultPainterFactory());
        }

        #endregion

        private void calendar_CalendarDayEntered(object sender, CalendarDaySelectedEventArgs e)
        {
            focusDay = e.Day;
            DrawCalendar();

            string text = e.Day.ToLongDateString();
            switch (e.Day.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    text += " 星期天";
                    break;
                case DayOfWeek.Monday:
                    text += " 星期一";
                    break;
                case DayOfWeek.Tuesday:
                    text += " 星期二";
                    break;
                case DayOfWeek.Wednesday:
                    text += " 星期三";
                    break;
                case DayOfWeek.Thursday:
                    text += " 星期四";
                    break;
                case DayOfWeek.Friday:
                    text += " 星期五";
                    break;
                case DayOfWeek.Saturday:
                    text += " 星期六";
                    break;
            }
            toolTipInfo.RemoveAll();
            toolTipInfo.SetToolTip(panelCalendar, text);
        }

        private void calendar_CalendarDaySelected(object sender, CalendarDaySelectedEventArgs e)
        {
        }

        private void calendar_CurrentDayChanged(object sender, EventArgs e)
        {
            labelCaption.Text = calendar.Caption;
            DrawCalendar();
        }

        private void painter_PainterNeeded(object sender, CalendarPainterNeededEventArgs e)
        {
            e.Painter = factory.GetNormalPainter();
        }

        private void labelCaption_MouseDown(object sender, MouseEventArgs e)
        {
            lastCursor = Control.MousePosition;
            labelCaption.Capture = true;
        }

        private void labelCaption_MouseMove(object sender, MouseEventArgs e)
        {
            if (labelCaption.Capture)
            {
                int x = Control.MousePosition.X - lastCursor.X;
                int y = Control.MousePosition.Y - lastCursor.Y;
                Point pos = new Point(this.Left + x, Screen.PrimaryScreen.WorkingArea.Top);
                if (pos.X < Screen.PrimaryScreen.WorkingArea.Left)
                {
                    pos.X = Screen.PrimaryScreen.WorkingArea.Left;
                }
                else if (pos.X + this.Width > Screen.PrimaryScreen.WorkingArea.Right)
                {
                    pos.X = Screen.PrimaryScreen.WorkingArea.Right - this.Width;
                }
                this.Location = pos;
                lastCursor = Control.MousePosition;
            }
        }

        private void labelCaption_MouseUp(object sender, MouseEventArgs e)
        {
            labelCaption.Capture = false;
        }

        private void CommandForm_Shown(object sender, EventArgs e)
        {
            SetDisplay(new MonthCalendar(), new DefaultPainterFactory());
        }

        private void panelCalendar_MouseMove(object sender, MouseEventArgs e)
        {
            this.calendar.MouseMove(new Point(0, 0), panelCalendar.PointToClient(Control.MousePosition));
        }

        private void panelCalendar_MouseClick(object sender, MouseEventArgs e)
        {
            this.calendar.MouseClick(new Point(0, 0), panelCalendar.PointToClient(Control.MousePosition));
        }

        private void panelCalendar_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(calendarBuffer, new Point(0, 0));
        }

        private void menuItemNotifyIconExit_Click(object sender, EventArgs e)
        {
            systemCallback.DoExit();
        }

        private void menuItemNotifyIconMonthCalendar_Click(object sender, EventArgs e)
        {
            systemCallback.ApplyMonthView();
        }

        private void menuItemNotifyIconNoCalendar_Click(object sender, EventArgs e)
        {
            systemCallback.ApplyCommandView();
        }

        private void textBoxCommand_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                e.Handled = true;
                string command = textBoxCommand.Text;
                if (command != "")
                {
                    textBoxCommand.Text = "";
                    try
                    {
                        commandHandlerManager.HandleCommand(command);
                    }
                    catch (Exception ex)
                    {
                        systemCallback.ShowError(ex.Message);
                    }
                }
            }
        }

        private void CommandForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                XDocument document = new XDocument();
                document.Add(new XElement("Settings"));
                commandHandlerManager.SaveSetting(document.Root);
                document.Save(settingPath);
            }
            catch (Exception ex)
            {
                systemCallback.ShowError(ex.Message);
            }
        }
    }
}
