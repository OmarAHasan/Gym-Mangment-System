using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {

        IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new();

        ISessionRepository SessionRepository { get; }
        IMemberShipRepository MemberShipRepository { get; }

        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
