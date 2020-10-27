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
        void DeleteItemAsync<T>(T item);
        Task<int> SaveItemAsync<T>(T item);
        void UpdateItemAsync<T>(T item);
    }
}
