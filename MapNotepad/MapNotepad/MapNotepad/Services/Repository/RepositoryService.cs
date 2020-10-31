using MapNotepad.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MapNotepad.Services.Repository
{
    public class RepositoryService : IRepositoryService
    {
        public readonly SQLiteAsyncConnection database;
        public RepositoryService()
        {
            database = new SQLiteAsyncConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Constants.DATABASE_NAME));          
        }
        public async Task DeleteItemAsync<T>(T item) where T : IBaseModel, new()
        {
            await database.CreateTableAsync<T>();

            await database.DeleteAsync(item);
        }
        public async Task<T> GetItemByIdAsync<T>(int id) where T : IBaseModel, new()
        {
            await database.CreateTableAsync<T>();

            return await database.GetAsync<T>(id);
        }
        public async Task<IEnumerable<T>> GetItemsAsync<T>() where T : IBaseModel, new()
        {
            await database.CreateTableAsync<T>();

            return await database.Table<T>().ToListAsync();
        }
        public async Task<int> SaveItemAsync<T>(T item) where T : IBaseModel, new()
        {
            await database.CreateTableAsync<T>();

            return await database.InsertAsync(item);
        }
        public async Task UpdateItemAsync<T>(T item) where T : IBaseModel, new()
        {
            await database.CreateTableAsync<User>();

            await database.UpdateAsync(item);
        }
    }
}
