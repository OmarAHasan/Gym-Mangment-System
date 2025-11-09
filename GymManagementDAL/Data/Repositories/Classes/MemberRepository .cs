using GymManagementDAL.Data.Context;
using GymManagementDAL.Data.Repositories.Interfaces;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.Repositories.Classes
{
    public class MemberRepository : IMemberRepository
    {
        private readonly GymDbContext _context;

        public MemberRepository(GymDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Member> GetAll()
        {
            return _context.Members.ToList();
        }

        public Member? GetById(int id)
        {
            return _context.Members.Find(id);
        }

        public void Add(Member member)
        {
            _context.Members.Add(member);
        }

        public void Update(Member member)
        {
            _context.Members.Update(member);
        }

        public void Delete(int id)
        {
            var member = _context.Members.Find(id);
            if (member != null)
                _context.Members.Remove(member);
        }

        public bool Exists(int id)
        {
            return _context.Members.Any(m => m.Id == id);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
