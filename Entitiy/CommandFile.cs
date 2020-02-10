using System.IO;

namespace DBCommon
{
    public static class CommandFile
    {
        public static string SharedFolder;

        public const string FileExtension = ".cmd";
        
        internal static string GetFileName(ICommandAble cmmd)
        {
            return $"{cmmd.Id} {cmmd.UpdatedDateTime.ToString("yyMMdd hhmmss")} {cmmd.UpdatedUserName}{FileExtension}";
        }

        internal static string GetFilePath(ICommandAble cmmd)
        {
            return Path.Combine(SharedFolder, GetFileName(cmmd));
        }
    }
}
