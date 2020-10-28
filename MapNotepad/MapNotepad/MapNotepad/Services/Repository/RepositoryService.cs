using MapNotepad.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MapNotepad.Services.Repository
{
    public class RepositoryService : IRepositoryService
    {
        private readonly string DATABASE_NAME = "Repository.db";
        public readonly SQLiteAsyncConnection database;
        public RepositoryService()
        {
            database = new SQLiteAsyncConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DATABASE_NAME));
            database.CreateTableAsync<User>();
            database.CreateTableAsync<CustomPin>();
        }
        public async Task DeleteItemAsync<T>(T item)
        {
            await database.DeleteAsync(item);
        }
        public async Task<T> GetItemAsync<T>(int id) where T : class, new()
        {
            return await database.GetAsync<T>(id);
        }
        public async Task<IEnumerable<T>> GetItemsAsync<T>() where T : class, new()
        {
            return await database.Table<T>().ToListAsync();
        }
        public async Task<int> SaveItemAsync<T>(T item)
        {
            return await database.InsertAsync(item);
        }
        public async void UpdateItemAsync<T>(T item)
        {
            await database.UpdateAsync(item);
        }
    }
}
