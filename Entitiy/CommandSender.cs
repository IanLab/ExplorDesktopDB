using System;
using System.IO;

namespace DBCommon
{
    public class CommandSender
    {
        private readonly ISerializer _ser;

        public CommandSender(ISerializer ser)
        {
            _ser = ser ?? throw new ArgumentNullException(nameof(ser));
        }

        public void Send(ICommandAble cmmd)
        {
            var cmmdStr =  _ser.Ser(cmmd);
            var filePath = CommandFile.GetFilePath(cmmd);
            File.WriteAllText(filePath, cmmdStr);
        }
    }
}
