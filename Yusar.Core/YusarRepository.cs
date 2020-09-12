using LiteDB;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Yusar.Core.Entities;
using System.Threading.Tasks;

namespace Yusar.Core
{
    public class YusarRepository<T> : IYusarRepository<T> where T : Entity
    {
        public string DbFile => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\yusar.db";

        public T Create(T item)
        {
            using (var db = new LiteDatabase(DbFile))
            {
                var items = db.GetCollection<T>(typeof(T).Name);
                var result = items.Insert(item);

                if (typeof(T).Name.Equals(nameof(SimpleString)))
                    items.EnsureIndex(x => (x as SimpleString).Str);

                var retItem = (T)item;
                return retItem;
            }
        }

        public async Task<T> CreateAsync(T item)
        {
            return await Task.Run(() => Create(item));
        }

        public bool Delete(int id)
        {
            using (var db = new LiteDatabase(DbFile))
            {
                var items = db.GetCollection<T>(typeof(T).Name);
                var value = new BsonValue(id);
                return items.Delete(value);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await Task.Run(() => Delete(id));
        }

        public IEnumerable<T> GetAll()
        {
            var resultItems = new List<T>();
            using (var db = new LiteDatabase(DbFile))
            {
                var items = db.GetCollection<T>(typeof(T).Name);
                resultItems = items.FindAll().ToList();
            }
            return resultItems;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await Task.Run(GetAll);
        }

        public T GetById(int id)
        {
            using (var db = new LiteDatabase(DbFile))
            {
                var items = db.GetCollection<T>(typeof(T).Name);
                var value = new BsonValue(id);
                return items.FindById(value);
            }
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await Task.Run(() => GetById(id));
        }

        public bool Update(T item)
        {
            using (var db = new LiteDatabase(DbFile))
            {
                var items = db.GetCollection<T>(typeof(T).Name);
                var result = items.Update(item);
                if (typeof(T).Name.Equals(nameof(SimpleString)) && result)
                    items.EnsureIndex(x => (x as SimpleString).Str);

                return result;
            }
        }

        public async Task<bool> UpdateAsync(T item)
        {
            return await Task.Run(() => Update(item));
        }
    }
}
