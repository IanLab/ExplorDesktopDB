using DBCommon;
using Newtonsoft.Json;

namespace NetCorDBTests
{
    public class JsonSer : ISerializer
    {
        private readonly static JsonSerializerSettings _setting = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

        public ICommandAble DesSer(string cmmdStr)
        {
            var cmmd = JsonConvert.DeserializeObject(cmmdStr, _setting) as ICommandAble;
            return cmmd;
        }

        public string Ser(ICommandAble cmmd)
        {
            return JsonConvert.SerializeObject(cmmd, _setting);
        }
    }
}
