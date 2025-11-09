using GymManagementDAL.Data.Context;
using GymManagementDAL.Data.Repositories.Interfaces;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;

namespace GymManagementDAL.Data.Repositories.Classes
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly Dictionary<Type, object> _repositories = new();
        private readonly GymDbContext _context;

        public UnitOfWork(GymDbContext context,ISessionRepository sessionRepository, IMemberShipRepository memberShipRepository)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            SessionRepository = new SessionRepository(_context);
            MemberShipRepository = new MemberShipRepository(_context);
        }

        // Entity-specific repositories
        public ISessionRepository SessionRepository { get; }
        public IMemberShipRepository MemberShipRepository { get; }

        // Generic repository
        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            var type = typeof(TEntity);
            if (_repositories.TryGetValue(type, out var repo))
                return (IGenericRepository<TEntity>)repo;

            var newRepo = new GenericRepository<TEntity>(_context);
            _repositories.Add(type, newRepo);
            return newRepo;
        }

        public int SaveChanges() => _context.SaveChanges();

        public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();
    }
}

