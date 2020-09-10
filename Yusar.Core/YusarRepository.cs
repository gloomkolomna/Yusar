using LiteDB;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Yusar.Core.Entities;

namespace Yusar.Core
{
    public class YusarRepository<T> : IYusarRepository<T> where T : Entity
    {
        public string DbFile => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\yusar.db";

        public bool Create(T item)
        {
            using (var db = new LiteDatabase(DbFile))
            {
                var items = db.GetCollection<T>(typeof(T).Name);
                var result = items.Insert(item);

                if (typeof(T).Name.Equals(nameof(SimpleString)))
                    items.EnsureIndex(x => (x as SimpleString).Str);
                return true;               
            }
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

        public T GetById(int id)
        {
            using (var db = new LiteDatabase(DbFile))
            {
                var items = db.GetCollection<T>(typeof(T).Name);
                var value = new BsonValue(id);
                return items.FindById(value);
            }
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
    }
}
