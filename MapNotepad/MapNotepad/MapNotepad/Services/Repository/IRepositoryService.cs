using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MapNotepad.Services.Repository
{
    public interface IRepositoryService
    {
        Task<IEnumerable<T>> GetItemsAsync<T>() where T : class, new();
        Task<T> GetItemAsync<T>(int id) where T : class, new();
        Task DeleteItemAsync<T>(T item);
        Task<int> SaveItemAsync<T>(T item);
        Task UpdateItemAsync<T>(T item);
    }
}
