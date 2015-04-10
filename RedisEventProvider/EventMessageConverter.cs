using Newtonsoft.Json;

namespace EPiServer.Redis.Events
{
    public class JsonConverter
    {
        public static  string ToJson<T>(T instance)
        {
            return JsonConvert.SerializeObject(instance);
        }

        public static  T FromJson<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

    }
}
