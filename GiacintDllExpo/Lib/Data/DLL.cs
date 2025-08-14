using System.Reflection;
using Mono.Cecil;

namespace GiacintDllExpo.Lib.Data
{
    internal struct DLL
    {
        public AssemblyDefinition Asm;
        public string Name;
        public string FullName;
        public string Version;
        public string Path;
        public string? Cert;
        public string? PublicKeyToken;

        public ModuleDefinition[]? Modules;
        public Resource[]? Resources;

        public TypeDefinition[]? Types;

        public DLL(AssemblyDefinition asm, string name, string version, string path, string cert)
        {
            Asm = asm;
            Name = name;
            Version = version;
            Path = path;
            Cert = cert;
        }

        internal static DLL FromFile(string path)
        {
            var assembly = AssemblyDefinition.ReadAssembly(path);
            DLL dll = new();

            dll.Asm = assembly;
            dll.Name = assembly.Name.Name;
            dll.FullName = assembly.Name.FullName;
            dll.Version = assembly.Name.Version.ToString();
            dll.Path = path;
            dll.PublicKeyToken = BitConverter.ToString(assembly.Name.PublicKeyToken).Replace("-", "").ToLower();

            dll.Modules = assembly.Modules.ToArray();
            dll.Resources = assembly.MainModule.Resources.ToArray();
            dll.Types = assembly.MainModule.Types.ToArray();

            return dll;
        }
    }
}
