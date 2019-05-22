using Microsoft.AspNetCore.Http;

namespace App.BLL.Interfaces
{
    public interface ISessionHelper
    {
        T GetObjectFromJson<T>(ISession session, string key);
        void SetObjectAsJson(ISession session, string key, object value);
    }
}
