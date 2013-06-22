using CmdLib;
using Newtonsoft.Json;

namespace CmdLib
{
    [JsonObject(MemberSerialization.OptIn)]
    public class CmdMsg
    {
        [JsonProperty]
        public string msg { get; set; }

        [JsonProperty]
        public string value{ get; set; }

        [JsonProperty]
        public string from { get; set; }

        [JsonProperty]
        public string user { get; set; }

        [JsonProperty]
        public string pass { get; set; }

        public CmdMsg()
        {
        }

        public string ToJsonString()
        {
            return JsonConvert.SerializeObject(this);
        }
        public static CmdMsg Deserialize(string jsonString)
        {
            return JsonConvert.DeserializeObject<CmdMsg>(jsonString);
        }
    }
}
