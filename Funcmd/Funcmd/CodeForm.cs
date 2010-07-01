using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Funcmd.Scripting;
using Funcmd.CommandHandler;
using System.Threading;

namespace Funcmd
{
    public partial class CodeForm : Form
    {
        private ScriptingEnvironment env = null;
        private ICommandHandlerCallback callback;

        public CodeForm(ICommandHandlerCallback callback)
        {
            this.callback = callback;
            InitializeComponent();
        }

        private void Run(ScriptingValue value)
        {
            string result = "";
            try
            {
                result = value.ToString();
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            Invoke(new MethodInvoker(() =>
            {
                textOutput.Text += result + "\r\n";
                textOutput.Select(textOutput.Text.Length, 0);
                textOutput.ScrollToCaret();
            }));
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonRun_Click(object sender, EventArgs e)
        {
            string text = textLaunch.Text;
            try
            {
                ScriptingValue value = env.ParseValue(text);
                textLaunch.Text = "";
                textLaunch.Select();
                textCode.Enabled = false;
                textLaunch.Enabled = false;
                buttonRun.Enabled = false;
                buttonClear.Enabled = false;

                Thread interpretorThread = new Thread(new ParameterizedThreadStart(o => Run((ScriptingValue)o)));
                interpretorThread.Start(value);

                Thread waitingThread = new Thread(() =>
                {
                    bool terminated = false;
                    terminated = interpretorThread.Join(10000);
                    if (!terminated)
                    {
                        interpretorThread.Abort();
                    }
                    this.Invoke(new MethodInvoker(() =>
                    {
                        if (!terminated)
                        {
                            callback.ShowError("10秒超时，停止脚本执行。");
                        }
                        textCode.Enabled = true;
                        textLaunch.Enabled = true;
                        buttonRun.Enabled = true;
                        buttonClear.Enabled = true;
                    }));
                });
                waitingThread.Start();
            }
            catch (Exception ex)
            {
                callback.ShowError(ex.Message);
                textLaunch.SelectAll();
                textLaunch.Select();
            }
        }

        private void tabCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabCode.SelectedTab == tabPageInterpretor)
            {
                try
                {
                    env = new ScriptingEnvironment(textCode.Text, new ScriptingEnvironment());
                    textLaunch.Select();
                }
                catch (ScriptingException ex)
                {
                    if (ex.Start != -1)
                    {
                        textCode.Select(ex.Start, ex.Length);
                        textCode.ScrollToCaret();
                    }
                    tabCode.SelectedTab = tabPageEditor;
                    textCode.Select();
                    callback.ShowError(ex.Message);
                }
                catch (Exception ex)
                {
                    callback.ShowError(ex.Message);
                    tabCode.SelectedTab = tabPageEditor;
                }
            }
        }

        private void textLaunch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                e.Handled = true;
                buttonRun_Click(buttonRun, new EventArgs());
            }
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            textOutput.Clear();
        }
    }
}
