using Mono.Cecil;
using GiacintDllExpo.Lib.Data;

namespace GiacintDllExpo.Lib.Services;
internal static class CecilConverter
{
    internal static AssemblyDto ToDto(AssemblyDefinition asm)
    {
        return new AssemblyDto
        {
            Name = asm.Name?.Name,
            Version = asm.Name?.Version?.ToString(),
            Attributes = asm.CustomAttributes.Select(ToDto).ToList(),
            Modules = asm.Modules.Select(m => new ModuleDto
            {
                Name = m.Name,
                Types = m.Types.Select(ToDto).ToList()
            }).ToList()
        };
    }

    internal static TypeDto ToDto(TypeDefinition t)
    {
        return new TypeDto
        {
            Name = t.Name,
            Namespace = t.Namespace,
            BaseType = t.BaseType?.FullName,
            Interfaces = t.Interfaces.Select(i => i.InterfaceType.FullName).ToList(),

            IsPublic = t.IsPublic,
            IsNotPublic = t.IsNotPublic,
            IsAbstract = t.IsAbstract,
            IsSealed = t.IsSealed,
            IsInterface = t.IsInterface,
            IsEnum = t.IsEnum,
            IsValueType = t.IsValueType,
            IsClass = t.IsClass,

            Attributes = t.CustomAttributes.Select(ToDto).ToList(),
            Fields = t.Fields.Select(ToDto).ToList(),
            Properties = t.Properties.Select(ToDto).ToList(),
            Methods = t.Methods.Select(ToDto).ToList()
        };
    }

    internal static FieldDto ToDto(FieldDefinition f)
    {
        return new FieldDto
        {
            Name = f.Name,
            FieldType = f.FieldType.FullName,
            IsPublic = f.IsPublic,
            IsPrivate = f.IsPrivate,
            IsStatic = f.IsStatic,
            IsInitOnly = f.IsInitOnly,
            IsLiteral = f.IsLiteral,
            Attributes = f.CustomAttributes.Select(ToDto).ToList()
        };
    }

    internal static PropertyDto ToDto(PropertyDefinition p)
    {
        return new PropertyDto
        {
            Name = p.Name,
            PropertyType = p.PropertyType.FullName,
            CanRead = p.GetMethod != null,
            CanWrite = p.SetMethod != null,
            HasPublicGetter = p.GetMethod?.IsPublic ?? false,
            HasPublicSetter = p.SetMethod?.IsPublic ?? false,
            Attributes = p.CustomAttributes.Select(ToDto).ToList()
        };
    }

    internal static MethodDto ToDto(MethodDefinition m)
    {
        return new MethodDto
        {
            Name = m.Name,
            ReturnType = m.ReturnType.FullName,
            IsPublic = m.IsPublic,
            IsPrivate = m.IsPrivate,
            IsStatic = m.IsStatic,
            IsAbstract = m.IsAbstract,
            IsVirtual = m.IsVirtual,
            IsConstructor = m.IsConstructor,
            Attributes = m.CustomAttributes.Select(ToDto).ToList(),
            Parameters = m.Parameters.Select(ToDto).ToList()
        };
    }

    internal static ParameterDto ToDto(ParameterDefinition p)
    {
        return new ParameterDto
        {
            Name = p.Name,
            ParameterType = p.ParameterType.FullName,
            IsOptional = p.IsOptional,
            IsOut = p.IsOut,
            IsIn = p.IsIn,
            Attributes = p.CustomAttributes.Select(ToDto).ToList()
        };
    }

    internal static CustomAttributeDto ToDto(CustomAttribute attr)
    {
        return new CustomAttributeDto
        {
            AttributeType = attr.AttributeType.FullName,
            ConstructorArguments = attr.ConstructorArguments
                .Select(a => a.Value?.ToString() ?? "null")
                .ToList()
        };
    }
}
