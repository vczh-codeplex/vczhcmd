using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace Funcmd.CommandHandler
{
    public class ShellCommandHandler : ICommandHandler
    {
        public bool HandleCommand(string command, ref Exception error)
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.ErrorDialog = false;
            info.FileName = command;
            info.UseShellExecute = true;
            info.Verb = "OPEN";
            try
            {
                info.WorkingDirectory = Path.GetDirectoryName(command);
            }
            catch (Exception)
            {
            }
            try
            {
                Process.Start(info);
                return true;
            }
            catch (Exception ex)
            {
                error = ex;
                return false;
            }
        }
    }
}
