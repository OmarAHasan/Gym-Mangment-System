using GymManagementDAL.Data.Context;
using GymManagementDAL.Data.Repositories.Interfaces;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;

namespace GymManagementDAL.Data.Repositories.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Dictionary<Type, object> _repositories = new();
        private readonly GymDbContext _dbContext;

        public UnitOfWork(GymDbContext dbContext, ISessionRepository sessionRepository, IMemberShipRepository memberShipRepository)
        {
            _dbContext = dbContext;
            this.sessionRepository = sessionRepository;
            MemberShipRepository = memberShipRepository;
        }

        public ISessionRepository sessionRepository { get; }

        

        public IMemberShipRepository MemberShipRepository { get; }

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            var entityType = typeof(TEntity);
            if (_repositories.TryGetValue(entityType, out var repo))
                return (IGenericRepository<TEntity>)repo;

            var newRepo = new GenericRepository<TEntity>(_dbContext);
            _repositories.Add(entityType, newRepo);
            return newRepo;
        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }
    }
}
