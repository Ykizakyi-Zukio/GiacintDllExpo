using Newtonsoft.Json;

namespace GiacintDllExpo.Lib.Data
{
    internal static class AppInfo
    {
        internal readonly static JsonSerializerSettings Settings = new()
        {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore,
            //ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore
        };
    }
}
