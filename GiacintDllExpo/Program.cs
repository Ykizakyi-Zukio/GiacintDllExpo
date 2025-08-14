using System;
using Mono.Cecil;
using GiacintDllExpo.Lib.Data;
using GiacintDllExpo.Lib.Services;

namespace GiacintDllExpo;

internal class Program
{
    private DLL currentDll;

    private static void Main(string[] args)
    {
        Program app = new Program();

        MainMessage();
        app.Listener();
    }

    internal static void MainMessage()
    {
        Console.WriteLine($"{Color.Info}\r\n");
        Console.Write($"/           /   \r\n         /' .,,,,  ./       \r\n        /';'     ,/        \r\n       / /   ,,//,`'`     Welcome Giacint Dll Expo\r\n      ( ,, '_,  ,,,' ``   Version: {Environment.Version}\r\n      |    /@  ,,, ;\" `   Help command: --help\r\n     /    .   ,''/' `,``   \r\n    /   .     ./, `,, ` ; \r\n ,./  .   ,-,',` ,,/''\\,' \r\n|   /; ./,,'`,,'' |   |   \r\n|     /   ','    /    |   \r\n \\___/'   '     |     |  \r\n  `,,'   |      /     `\\  \r\n        /      |        ~\\  \r\n       '       (          \r\n      :                    \r\n     ; .         \\         \r\n    :   \\         ;       ");
        Console.WriteLine("\r\n\r\n");
    }

    internal void Listener()
    {
        while (true)
        {
            string msg = Debug.Input() ?? string.Empty;
            if (msg == string.Empty)
                continue;

            string[] args = msg.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            try
            {
                switch (args[0])
                {
                    case "--help":
                    case "-h":
                        Console.WriteLine($"{Color.Info}Available commands:");
                        Console.WriteLine($"{Color.Info}  --help, -h: Show this help message");
                        Console.WriteLine($"{Color.Info}  --read, -r <path>: Read a DLL file");
                        Console.WriteLine($"{Color.Info}  --select, -s <path>: Select a DLL file to read");
                        Console.WriteLine($"{Color.Info}  --exit, -e: Exit the application");
                        break;
                    case "--read":
                    case "-r":
                        try
                        {
                            if (currentDll.Equals(null))
                                Debug.Warning("Please select DLL");
                            DllReader.Read(currentDll);
                        }
                        catch (Exception ex)
                        {
                            Debug.Error(ex);
                        }
                        break;
                    case "--select":
                    case "-s":
                        string path = StringHelper.PathFromArgs(args, 1);
                        if (!System.IO.File.Exists(path))
                        {
                            Debug.Warning($"File not found: {path}");
                            continue;
                        }
                        currentDll = DLL.FromFile(path);
                        break;
                    case "--exit":
                    case "-e":
                        Environment.Exit(0);
                        break;
                    default:
                        Debug.Warning($"Unknown command: {args[0]}");
                        break;
                }
            }
            catch (Exception ex)
            {
                if (ex is BadImageFormatException)
                    Debug.Warning("The file is not a valid .NET assembly or is corrupted.");
                else
                    Debug.Error(ex);
            }   
        }
    }
}
