using System.Text;
using System.Text.Json;
using Newtonsoft.Json;

namespace Tests.WebApp.Helpers
{
    public static class JsonHelper
    {
        private static readonly JsonSerializerOptions JsonSerializerOptions =
            new JsonSerializerOptions(JsonSerializerDefaults.Web);

        public static TValue? DeserializeWithWebDefaults<TValue>(string json)
        {
            return System.Text.Json.JsonSerializer.Deserialize<TValue>(json, JsonSerializerOptions);
        }
        public static StringContent GetStringContent(object obj)
        {
            return new StringContent(JsonConvert.SerializeObject(obj), Encoding.Default, "application/json");
        }
    }
}
