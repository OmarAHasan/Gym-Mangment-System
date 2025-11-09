using GymManagementDAL.Data.Context;
using GymManagementDAL.Data.Repositories.Interfaces;
using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.Repositories.Classes
{
    public class PlanRepo : IPlanRepo
    {
        private readonly GymDbContext _context;

        public PlanRepo(GymDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Plan> GetAll()
        {
            return _context.Plans.ToList();
        }

        public Plan? GetById(int id)
        {
            return _context.Plans.Find(id);
        }

        public void Add(Plan plan)
        {
            _context.Plans.Add(plan);
        }

        public void Update(Plan plan)
        {
            _context.Plans.Update(plan);
        }

        public void Delete(int id)
        {
            var plan = _context.Plans.Find(id);
            if (plan != null)
                _context.Plans.Remove(plan);
        }

        public bool Exists(int id)
        {
            return _context.Plans.Any(p => p.Id == id);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
        public async Task<IEnumerable<Plan>> GetAllActiveAsync()
        {
            return await _context.Plans
                .Where(p => p.IsActive == true)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
