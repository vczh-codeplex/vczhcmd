using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;

namespace Funcmd.CommandHandler
{
    public interface ICommandHandler
    {
        bool HandleCommand(string command, ref Exception error);
        void LoadSetting(XElement settingRoot);
        void SaveSetting(XElement settingRoot);
    }

    public interface ICommandHandlerCallback
    {
        void DoExit();
        void ShowMessage(string message);
        void ShowError(string message);
        void OpenCodeForm();

        void ApplyCommandView();
        void ApplyMonthView();
    }

    public class CommandHandlerManager
    {
        private List<ICommandHandler> handlers = new List<ICommandHandler>();
        private ICommandHandlerCallback callback;

        public CommandHandlerManager(ICommandHandlerCallback callback)
        {
            this.callback = callback;
        }

        public void AddCommandHandler(ICommandHandler handler)
        {
            handlers.Add(handler);
        }

        public void HandleCommand(string command)
        {
            Exception error = null;
            foreach (ICommandHandler handler in handlers)
            {
                if (handler.HandleCommand(command, ref error))
                {
                    return;
                }
            }
            throw error;
        }

        public void LoadSetting(XElement root)
        {
            foreach (ICommandHandler handler in handlers)
            {
                try
                {
                    XElement element = root
                        .Elements("CommandSetting")
                        .Where(e => e.Attribute("Class").Value == handler.GetType().AssemblyQualifiedName)
                        .FirstOrDefault();
                    if (element != null)
                    {
                        handler.LoadSetting(element);
                    }
                }
                catch (Exception ex)
                {
                    callback.ShowError("为" + handler.GetType().AssemblyQualifiedName + "加载设置的时候发生错误：" + ex.Message);
                }
            }
        }

        public void SaveSetting(XElement root)
        {
            foreach (ICommandHandler handler in handlers)
            {
                try
                {
                    XElement element = new XElement(
                        "CommandSetting",
                        new XAttribute("Class", handler.GetType().AssemblyQualifiedName)
                        );
                    handler.SaveSetting(element);
                    root.Add(element);
                }
                catch (Exception ex)
                {
                    callback.ShowError("为" + handler.GetType().AssemblyQualifiedName + "保存设置的时候发生错误：" + ex.Message);
                }
            }
        }
    }
}
