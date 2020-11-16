using MapNotepad.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MapNotepad.Services.Repository
{
    public class RepositoryService : IRepositoryService
    {
        private readonly string _path;

        public RepositoryService()
        {
            _path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Constants.DATABASE_NAME);
        }

        private SQLiteAsyncConnection _database;
        public SQLiteAsyncConnection dataBase => _database ??= new SQLiteAsyncConnection(_path);

        #region -- IRepositoryService implementation --

        public async Task DeleteItemAsync<T>(T item) where T : IBaseModel, new()
        {
            await dataBase.CreateTableAsync<T>();

            await dataBase.DeleteAsync(item);
        }

        public async Task<T> GetItemByIdAsync<T>(int id) where T : IBaseModel, new()
        {
            await dataBase.CreateTableAsync<T>();

            return await dataBase.GetAsync<T>(id);
        }

        public async Task<IEnumerable<T>> GetItemsAsync<T>(Expression<Func<T, bool>> predicate) where T : IBaseModel, new()
        {
            await dataBase.CreateTableAsync<T>();

            return await dataBase.Table<T>().Where(predicate).ToListAsync();
        }

        public async Task<int> SaveItemAsync<T>(T item) where T : IBaseModel, new()
        {
            await dataBase.CreateTableAsync<T>();

            return await dataBase.InsertAsync(item);
        }

        public async Task UpdateItemAsync<T>(T item) where T : IBaseModel, new()
        {
            await dataBase.CreateTableAsync<User>();

            await dataBase.UpdateAsync(item);
        }

        #endregion

    }
}
