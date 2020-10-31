using MapNotepad.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MapNotepad.Services.Repository
{
    public interface IRepositoryService
    {
        Task<IEnumerable<T>> GetItemsAsync<T>() where T : IBaseModel, new();
        Task<T> GetItemByIdAsync<T>(int id) where T : IBaseModel, new();
        Task DeleteItemAsync<T>(T item) where T : IBaseModel, new();
        Task<int> SaveItemAsync<T>(T item) where T : IBaseModel, new();
        Task UpdateItemAsync<T>(T item) where T : IBaseModel, new();
    }
}
