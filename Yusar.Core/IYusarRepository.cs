using System.Collections.Generic;
using System.Threading.Tasks;
using Yusar.Core.Entities;

namespace Yusar.Core
{
    public interface IYusarRepository<T> where T : Entity
    {
        IEnumerable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();
        T GetById(int id);
        Task<T> GetByIdAsync(int id);
        T Create(T item);
        Task<T> CreateAsync(T item);
        bool Update(T item);
        Task<bool> UpdateAsync(T item);
        bool Delete(int id);
        Task<bool> DeleteAsync(int id);
    }
}
