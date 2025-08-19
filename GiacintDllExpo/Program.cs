using System;
using Mono.Cecil;
using GiacintDllExpo.Lib.Data;
using GiacintDllExpo.Lib.Services;
using Newtonsoft.Json;

namespace GiacintDllExpo;

internal class Program
{
    private DLL currentDll;
    private TypeDefinition[]? currentTypes;
    private MethodDefinition? currentMethod;

    private static void Main(string[] args)
    {
        Program app = new Program();
        MainMessage();
        try
        {
            app.Listener().GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            Debug.Error(ex);
        }
    }

    internal static void MainMessage()
    {
        Console.WriteLine($"{Color.Info}\r\n");
        Console.Write($"/           /   \r\n         /' .,,,,  ./       \r\n        /';'     ,/        \r\n       / /   ,,//,`'`     Welcome Giacint Dll Expo\r\n      ( ,, '_,  ,,,' ``   Version: {Environment.Version}\r\n      |    /@  ,,, ;\" `   Help command: --help\r\n     /    .   ,''/' `,``  Github: https://github.com/Ykizakyi-Zukio/GiacintDllExpo\r\n    /   .     ./, `,, ` ; System: {Environment.OSVersion}\r\n ,./  .   ,-,',` ,,/''\\,' Privilaged: {Environment.IsPrivilegedProcess}\r\n|   /; ./,,'`,,'' |   |   Working user: {Environment.UserName}\r\n|     /   ','    /    |   \r\n \\___/'   '     |     |  \r\n  `,,'   |      /     `\\  \r\n        /      |        ~\\  \r\n       '       (          \r\n      :                    \r\n     ; .         \\         \r\n    :   \\         ;       ");
        Console.WriteLine("\r\n\r\n");
    }

    internal async Task Listener()
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
                    case "--info":
                    case "-i":
                        try
                        {
                            if (currentDll.Equals(null))
                                Debug.Warning("Please select DLL");

                            if (args.Length <2)
                                DllReader.ReadInfo(currentDll);
                            else if (args[1] == "-js")
                            {
                                string json = JsonConvert.SerializeObject(currentDll, AppInfo.Settings);

                                if (args.Length < 4)
                                    Console.WriteLine(json);
                                else if (args[2] == "-AS" && args[3] == "file")
                                    await File.WriteAllTextAsync($"{Environment.CurrentDirectory}\\{currentDll.FullName}.json", json).ContinueWith(t =>
                                    {
                                        if (t.IsCompletedSuccessfully)
                                            Debug.Success($"File saved as {Environment.CurrentDirectory}\\{currentDll.FullName}.json");
                                        else
                                            Debug.Error(t.Exception);
                                    });
                            }
                            else if (args.Length == 3 && args[1] == "-INSTRS" && args[2] == "file")
                            {
                                _ = Task.Run(async () => {
                                    string str = await DllReader.ReadInstructions(currentDll.Asm);
                                    if (str != null)
                                    {
                                        await File.WriteAllTextAsync($"{Environment.CurrentDirectory}\\{currentDll.FullName}_instructions.json", str);
                                        Debug.Success($"Instructions writed from: {Environment.CurrentDirectory}\\{currentDll.FullName}_instructions.json");
                                    }
                                    else
                                        Debug.Warning("No instructions found");
                                });
                            }
                            else
                            {
                                Debug.Warning("Invalid syntax");
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.Error(ex);
                        }
                        break;
                    case "--select":
                    case "-s":
                        string path = StringHelper.PathFromArgs(args, 1);
                        if (!File.Exists(path))
                        {
                            Debug.Warning($"File not found: {path}");
                            continue;
                        }
                        currentDll = DLL.FromFile(path);
                        break;
                    case "--read":
                    case "-r":
                        if (currentDll.Equals(null))
                            Debug.Warning("Please select DLL");
                        DllReader.ReadTypes(currentDll);
                        break;
                    case "--typeselect":
                    case "-ts":
                        if (args.Length < 3)
                            Debug.Warning("Minimal args count is 2!");

                        if (currentDll.Equals(null))
                            Debug.Warning("Please select DLL");

                        if (args[1] == "-name")
                        {
                            currentTypes = new TypeDefinition[1];
                            currentTypes[0] = currentDll.Asm.MainModule.Types.Where(s => s.FullName == args[2]).First();
                            if (currentTypes[0] == null)
                                Debug.Warning("Type not finded");
                        }
                        else if (args[1] == "-ns")
                        {
                            currentTypes = currentDll.Asm.MainModule.Types.Where(s => s.Namespace == args[2]).ToArray();
                            if (currentTypes.Length == 0)
                                Debug.Warning("Type not finded");
                            Debug.Info($"Finded {currentTypes.Length} types");
                        }
                        else
                        {
                            Debug.Warning("Invalid syntax");
                        }
                            break;
                    case "-methodselect":
                    case "-ms":
                        if (args.Length < 3)
                            Debug.Warning("Minimal args count is 2!");

                        if (currentTypes == null || currentTypes.Length == 0)
                        {
                            Debug.Warning("Please select type");
                            continue;
                        }
                        if (args[1] == "-name")
                        {
                            currentMethod = currentTypes.First().Methods.Where(s => s.FullName == args[2]).FirstOrDefault();
                            if (currentMethod == null)
                                Debug.Warning("Method not finded");
                        }
                        else if (args[1] == "-type")
                        {
                            currentMethod = currentTypes.First().Methods.Where(s => s.ReturnType.ToString() == args[2]).FirstOrDefault();
                            if (currentMethod == null)
                                Debug.Warning("Method not finded");
                        }
                        else
                        {
                            Debug.Warning("Invalid syntax");
                        }
                        break;
                    case "--this":
                    case "-this":
                        if (args.Length <2)
                        {
                            Debug.Warning("Minimal args count is 1!");
                            continue;
                        }

                        if (args[1] == "-t")
                        {
                            if (args.Length < 3)
                            {
                                Debug.Warning("Minimal args count is 2!");
                                continue;
                            }

                            if (args[2] == "-first")
                            {
                                if (currentTypes == null || currentTypes.Length == 0)
                                {
                                    Debug.Warning("Please select type");
                                    continue;
                                }

                                DllReader.ReadType(CecilConverter.ToDto(currentTypes.First()));
                            }
                            else if (args[2] == "-any")
                            {
                                for (int i = 0; i < currentTypes.Length; i++)
                                {
                                    Debug.Info($"Type {i + 1} of {currentTypes.Length}");
                                    DllReader.ReadType(CecilConverter.ToDto(currentTypes[i]));
                                }
                            }
                        }
                        else if (args[1] == "-m")
                        {
                            if (args.Length < 3)
                            {
                                Debug.Warning("Minimal args count is 2!");
                                continue;
                            }

                            if (args[2] == "-first")
                            {
                                if (currentTypes == null || currentTypes.Length == 0)
                                {
                                    Debug.Warning("Please select type");
                                    continue;
                                }
                                if (currentTypes.First().Methods.Count == 0)
                                {
                                    Debug.Warning("Type has no methods");
                                    continue;
                                }
                                currentMethod = currentTypes.First().Methods.First();
                                DllReader.ReadMethod(CecilConverter.ToDto(currentMethod));
                            }
                            else if (args[2] == "-any")
                            {
                                if (currentTypes == null || currentTypes.Length == 0)
                                {
                                    Debug.Warning("Please select type");
                                    continue;
                                }
                                foreach (var method in currentTypes.First().Methods)
                                {
                                    DllReader.ReadMethod(CecilConverter.ToDto(method));
                                }
                            }
                        }
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
