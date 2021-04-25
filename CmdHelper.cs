using System.Diagnostics;

namespace VultrManager
{
    public static class CmdHelper
    {
        public static string Execute(string command)
        {
            command = command.Trim().Trim('&') + "&exit";
            using Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.UseShellExecute = false;
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.RedirectStandardError = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.Start();

            cmd.StandardInput.WriteLine(command);
            cmd.StandardInput.AutoFlush = true;

            string output = cmd.StandardOutput.ReadToEnd();
            cmd.WaitForExit();
            return output;
        }
    }
}
