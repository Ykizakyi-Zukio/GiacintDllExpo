using GiacintDllExpo.Lib.Data;

namespace GiacintDllExpo.Lib.Services
{
    internal class DllReader
    {
        internal static void ReadInfo(DLL dll)
        {
            Console.WriteLine($"{Color.Info}Reading DLL: {dll.Name} ({dll.Version})");
            Console.WriteLine($"{Color.Info}Full Name: {dll.FullName}");
            Console.WriteLine($"{Color.Info}Path: {dll.Path}");
            Console.WriteLine($"{Color.Info}Public Key Token: {dll.PublicKeyToken}");
            Console.WriteLine($"{Color.Info}Certificate: {dll.Cert}");
            if (dll.Modules != null && dll.Modules.Length > 0)
            {
                Console.WriteLine($"{Color.Info}Modules:");
                foreach (var module in dll.Modules)
                {
                    Console.Write($"Module - {module.Name}\r\n" +
                    $" _ Architecture: {module.Architecture}\r\n" +
                    $"_ Runtime {module.Runtime} ({module.RuntimeVersion})");
                }
            }
            else
            {
                Console.WriteLine($"{Color.Warning}No modules found.");
            }
            if (dll.Resources != null && dll.Resources.Length > 0)
            {
                Console.WriteLine($"{Color.Info}Resources:");
                foreach (var resource in dll.Resources)
                {
                    Console.WriteLine($" - {resource.Name} ({resource.ResourceType}");
                }
            }
            else
            {
                Console.WriteLine($"{Color.Warning}No resources found.");
            }
        }
        internal static void ReadTypes(DLL dll)
        {
            foreach (var type in dll.Types)
            {
                Debug.Success($"t ~> {type.FullName}");
                Debug.Info($"   namespace -> {type.Namespace}");
                Debug.Info($"   isPublic -> {type.IsPublic}");

                if (type.Methods.Count > 0)
                    Debug.Info("\r\nMethods")
                foreach (var method in type.Methods)
                {
                    Debug.Info($"m ~> {method.FullName}");
                    Debug.Info($"   isPublic -> {method.IsPublic}");
                    Debug.Info($"   isStatic => {method.IsStatic}\r\n ");
                }
            }
        }
    }
}
