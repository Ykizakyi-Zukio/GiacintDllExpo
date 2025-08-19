using GiacintDllExpo.Lib.Data;
using Mono.Cecil;
using Newtonsoft.Json;
using System.Text;

namespace GiacintDllExpo.Lib.Services
{
    internal class DllReader
    {
        internal static void ReadInfo(DLL dll)
        {
            Console.WriteLine($"\r\n{Color.Info}Reading DLL: {dll.Name ?? "NULL"} ({dll.Version ?? "NULL"})");
            Console.WriteLine($"{Color.Info}Full Name: {dll.FullName ?? "NULL"}");
            Console.WriteLine($"{Color.Info}Path: {dll.Path ?? "NULL"}");
            Console.WriteLine($"{Color.Info}Public Key Token: {dll.PublicKeyToken ?? "NULL"}");
            Console.WriteLine($"\r\n{Color.Info}Certificate: {dll.Cert ?? "NULL"}");
            if (dll.Asm.MainModule.Types != null && dll.Asm.MainModule.Types.Count > 0)
            {
                Console.WriteLine($"{Color.Info}Modules:");
                foreach (var module in dll.Asm.Modules)
                {
                    Console.Write($"\r\nModule ~> {module.Name ?? "NULL"}\r\n" +
                    $"  Architecture -> {module.Architecture}\r\n" +
                    $"  Runtime -> {module.Runtime} ({module.RuntimeVersion ?? "NULL"})");
                }
            }
            else
            {
                Console.WriteLine($"{Color.Warning}No modules found.");
            }
            if (dll.Asm.MainModule.Resources != null && dll.Asm.MainModule.Resources.Count > 0)
            {
                Console.WriteLine($"\r\n{Color.Info}Resources:");
                foreach (var resource in dll.Asm.MainModule.Resources)
                {
                    Console.WriteLine($"\r\n - {resource.Name} ({resource.ResourceType}");
                }
            }
            else
            {
                Console.WriteLine($"{Color.Warning}\r\n\r\nNo resources found.");
            }
        }
        internal static void ReadTypes(DLL dll)
        {
            foreach (var type in dll.Asm.MainModule.Types)
            {
                Debug.Success($"t ~> {type.FullName}");
                Debug.Info($"   namespace -> {type.Namespace}");
                Debug.Info($"   isPublic -> {type.IsPublic}");

                if (type.Methods.Count > 0)
                    Debug.Info("\r\nMethods");
                foreach (var method in type.Methods)
                {
                    Debug.Info($"m ~> {method.FullName}");
                    Debug.Info($"   isPublic -> {method.IsPublic}");
                    Debug.Info($"   isStatic => {method.IsStatic}\r\n ");
                }
            }
        }
        internal static void ReadType(TypeDto type)
        {

            Debug.Info(JsonConvert.SerializeObject(type, Formatting.Indented).Trim('{', '}'));
        }

        internal static void ReadMethod(MethodDto method)
        {
            Debug.Info(JsonConvert.SerializeObject(method, Formatting.Indented).Trim('{', '}'));
        }

        internal async static Task<string?> ReadInstructions(MethodDefinition method)
        {
            if (method.Body == null)
                return null;

            var builder = new StringBuilder();
            Task<string> tsk = Task.Run(() =>
            {
                foreach (var instruction in method.Body.Instructions)
                {
                    builder.AppendLine($"{instruction.OpCode} {instruction.Operand}");
                }

                return builder.ToString();
            });


            return await tsk;
        }
        internal async static Task<string?> ReadInstructions(TypeDefinition type)
        {
            try
            {
                var result = new StringBuilder();
                result.Append($"\r\n<; TYPE: INSTRS  ({type.FullName}) ;>\r\n \t<; INSTRS ;>\r\n");
                for (int i = 0; i < type.Methods.Count; i++)
                {
                    var method = type.Methods[i];
                    if (method.Body != null)
                    {
                        result.Append($"\r\n\t<; METHOD: INSTRS  ({method.FullName}) ;>\r\n \t<; INSTRS ;>\r\n");
                        result.Append(StringHelper.AddTabs(await ReadInstructions(method)));
                        result.Append("\r\n\t<; END INSTRS ;>\r\n");
                    }
                }
                result.Append("\r\n\t<; END INSTRS ;>\r\n");

                return result.ToString();
            }
            catch (Exception ex)
            {
                Debug.Error(ex);
                return null;
            }
        }
        internal async static Task<string?> ReadInstructions(AssemblyDefinition asm)
        {
            try
            {
                var result = new StringBuilder();
                foreach (var type in asm.MainModule.Types)
                {
                    result.Append($"\r\n<; TYPE: INSTRS  ({type.FullName}) ;>\r\n \t<; INSTRS ;>");
                    result.Append(StringHelper.AddTabs(await ReadInstructions(type)));
                    result.Append("\r\n<; END INSTRS ;>\r\n");
                }
                return result.ToString();
            }
            catch (Exception ex)
            {
                Debug.Error(ex);
                return null;
            }
        }
    }
}
