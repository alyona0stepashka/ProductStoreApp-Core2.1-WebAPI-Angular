using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using App.BLL.Interfaces;

namespace App.BLL.Services
{
    public class SessionHelper : ISessionHelper
    {
        public T GetObjectFromJson<T>(ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

        public void SetObjectAsJson(ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }
    }
}