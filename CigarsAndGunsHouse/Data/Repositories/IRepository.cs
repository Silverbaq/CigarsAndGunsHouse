using System.Collections.Generic;

namespace CigarsAndGunsHouse.Data.Repositories
{
    public interface IRepository<T>
    {
        void Add(T value);

        T GetById(string id);

        List<T> GetAll();

        T Update(T value);

        T Remove(T value);
    }
}