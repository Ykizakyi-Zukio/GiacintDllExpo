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
    }
}
