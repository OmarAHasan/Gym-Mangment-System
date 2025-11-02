using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        // create Function To make Repositories

      IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new();

     int SaveChanges();

        public ISessionRepository sessionRepository { get; }

        public IMemberShipRepository MemberShipRepository { get; }
    }
}
