using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace GiacintDllExpo.Lib.Services;
internal class TextEditor
{
    private static bool isOpened = false;

    internal static string Edit(string text)
    {
        if (isOpened)
        {
            Debug.Warning("Text editor is already opened, returning original text.");
            return text;
        }

        isOpened = true;
        try
        {
            string tempFile = Path.Combine(Path.GetTempPath(), $"temp_edit_{Guid.NewGuid()}.txt");
            File.WriteAllText(tempFile, text);

            ProcessStartInfo psi;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                psi = new ProcessStartInfo("notepad.exe", $"\"{tempFile}\"");
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                psi = new ProcessStartInfo("xdg-open", $"\"{tempFile}\"");
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                psi = new ProcessStartInfo("open", $"\"{tempFile}\"");
            else
                throw new PlatformNotSupportedException("Unsupported OS");

            psi.UseShellExecute = true;

            using (var proc = Process.Start(psi))
                {
                    proc.WaitForExit();
                }

            string result = File.ReadAllText(tempFile);
            File.Delete(tempFile);

            isOpened = false;
            return result;
        }
        catch (Exception ex)
        {
            Debug.Error(ex);
            return text;
        }
        finally
        {
            isOpened = false;
        }
    }
}
