using System.Threading.Tasks;

namespace MapNotepad.Services.REST
{
    public interface IRestService
    {
        Task<T> GetAsync<T>(string url);
    }
}
