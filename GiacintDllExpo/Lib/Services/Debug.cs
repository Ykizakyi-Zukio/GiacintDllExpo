// from https://github.com/Ykizakyi-Zukio/GiacintTrustEncrypt

using System.Text;
using GiacintDllExpo.Lib.Data;

namespace GiacintDllExpo.Lib.Services;

internal static class Debug
{
    internal static void Error(Exception ex)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.Error.WriteLine($"{Color.Reset}[{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}] {Color.Error}× {ex.ToString()}");
        Console.ForegroundColor = ConsoleColor.White;
    }

    internal static void Warning(string message)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine($"{Color.Reset}[{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}] {Color.Warning}⚠  {message}");
        Console.ForegroundColor = ConsoleColor.White;
    }

    internal static void Success(string message)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine($"{Color.Reset}[{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}] {Color.Success}✓  {message}");
        Console.ForegroundColor = ConsoleColor.White;
    }

    internal static void Info(string message)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine($"{Color.Reset}[{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}] {Color.Info}ⓘ  {message}");
        Console.ForegroundColor = ConsoleColor.White;
    }

    internal static string? Input()
    {
        Console.Write($"{Color.Reset}[{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}] ~ user ->{Color.Info} ");
        return Console.ReadLine();
    }
}
