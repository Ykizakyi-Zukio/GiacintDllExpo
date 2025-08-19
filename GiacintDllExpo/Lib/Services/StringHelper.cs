namespace GiacintDllExpo.Lib.Services
{
    internal class StringHelper
    {
        internal static string PathFromArgs(string[] args, byte index)
        {
            if (args.Length <= index)
            {
                throw new ArgumentException($"Argument at index {index} is missing.");
            }

            string path = "";
            for (byte i = index; i < args.Length; i++)
            {
                path = path + ' ' + args[i];
            }

            return path.Trim('\"', ' ');
        }

        internal static string AddTabs(string str, int tabCount = 1)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            string tabs = new string('\t', tabCount);
            return str.Replace("\r\n", $"\r\n{tabs}").Replace("\n", $"\n{tabs}").Insert(0, tabs);
        }
    }
}
