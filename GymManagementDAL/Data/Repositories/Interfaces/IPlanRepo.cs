using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.Repositories.Interfaces
{
    public interface IPlanRepo
    {
        Task<IEnumerable<Plan>> GetAllActiveAsync();
        IEnumerable<Plan> GetAll();
        Plan? GetById(int id);
        void Add(Plan plan);
        void Update(Plan plan);
        void Delete(int id);
        bool Exists(int id);
        void Save();
    }
}
