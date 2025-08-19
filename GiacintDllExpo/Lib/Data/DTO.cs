// For Newtonsoft.Json

namespace GiacintDllExpo.Lib.Data;

public class AssemblyDto
{
    public string Name { get; set; }
    public string Version { get; set; }
    public List<CustomAttributeDto> Attributes { get; set; } = new();
    public List<ModuleDto> Modules { get; set; } = new();
}

public class ModuleDto
{
    public string Name { get; set; }
    public List<TypeDto> Types { get; set; } = new();
}

public class TypeDto
{
    public string Name { get; set; }
    public string Namespace { get; set; }
    public string BaseType { get; set; }
    public List<string> Interfaces { get; set; } = new();

    // Флаги
    public bool IsPublic { get; set; }
    public bool IsNotPublic { get; set; }
    public bool IsAbstract { get; set; }
    public bool IsSealed { get; set; }
    public bool IsInterface { get; set; }
    public bool IsEnum { get; set; }
    public bool IsValueType { get; set; }
    public bool IsClass { get; set; }

    public List<CustomAttributeDto> Attributes { get; set; } = new();
    public List<FieldDto> Fields { get; set; } = new();
    public List<PropertyDto> Properties { get; set; } = new();
    public List<MethodDto> Methods { get; set; } = new();
}

public class FieldDto
{
    public string Name { get; set; }
    public string FieldType { get; set; }

    // Флаги
    public bool IsPublic { get; set; }
    public bool IsPrivate { get; set; }
    public bool IsStatic { get; set; }
    public bool IsInitOnly { get; set; } // readonly
    public bool IsLiteral { get; set; }  // const

    public List<CustomAttributeDto> Attributes { get; set; } = new();
}

public class PropertyDto
{
    public string Name { get; set; }
    public string PropertyType { get; set; }

    // Флаги
    public bool CanRead { get; set; }
    public bool CanWrite { get; set; }
    public bool HasPublicGetter { get; set; }
    public bool HasPublicSetter { get; set; }

    public List<CustomAttributeDto> Attributes { get; set; } = new();
}

public class MethodDto
{
    public string Name { get; set; }
    public string ReturnType { get; set; }

    // Флаги
    public bool IsPublic { get; set; }
    public bool IsPrivate { get; set; }
    public bool IsStatic { get; set; }
    public bool IsAbstract { get; set; }
    public bool IsVirtual { get; set; }
    public bool IsConstructor { get; set; }

    public List<ParameterDto> Parameters { get; set; } = new();
    public List<CustomAttributeDto> Attributes { get; set; } = new();
}

public class ParameterDto
{
    public string Name { get; set; }
    public string ParameterType { get; set; }

    // Флаги
    public bool IsOptional { get; set; }
    public bool IsOut { get; set; }
    public bool IsIn { get; set; }

    public List<CustomAttributeDto> Attributes { get; set; } = new();
}

public class CustomAttributeDto
{
    public string AttributeType { get; set; }
    public List<string> ConstructorArguments { get; set; } = new();
}
