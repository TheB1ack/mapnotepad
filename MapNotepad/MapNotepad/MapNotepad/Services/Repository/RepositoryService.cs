﻿using MapNotepad.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MapNotepad.Services.Repository
{
    public class RepositoryService : IRepositoryService
    {

        public RepositoryService()
        {       

        }

        #region -- Public properties --

        private SQLiteAsyncConnection _database;
        public SQLiteAsyncConnection dataBase => _database ??= new SQLiteAsyncConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Constants.DATABASE_NAME));

        #endregion

        #region -- IterfaceName implementation --

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
        public async Task<IEnumerable<T>> GetItemsAsync<T>() where T : IBaseModel, new()
        {
            await dataBase.CreateTableAsync<T>();

            return await dataBase.Table<T>().ToListAsync();
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
