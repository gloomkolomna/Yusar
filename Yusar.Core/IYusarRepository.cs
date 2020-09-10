using System.Collections.Generic;
using Yusar.Core.Entities;

namespace Yusar.Core
{
    public interface IYusarRepository<T> where T : Entity
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        bool Create(T item);
        bool Update(T item);
        bool Delete(int id);
    }
}
