using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity , new()
    {
        IEnumerable<TEntity> GetAll(Func<TEntity , bool>? Condtion = null);

        TEntity? GetById(int id); // can be null

       void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);


    }
}
