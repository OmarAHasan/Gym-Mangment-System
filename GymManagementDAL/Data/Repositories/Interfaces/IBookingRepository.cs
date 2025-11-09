using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.Repositories.Interfaces
{
    public interface IBookingRepository
    {
        Task<IEnumerable<Booking>> GetAllAsync();
        Task<Booking?> GetByIdAsync(int id);
        Task<IEnumerable<Booking>> GetBookingsForMemberAsync(int memberId);
        Task<IEnumerable<Booking>> GetBookingsForSessionAsync(int sessionId);

        Task<bool> ExistsAsync(int id);
        Task<bool> IsMemberBookedAsync(int memberId, int sessionId);

        Task AddAsync(Booking booking);
        void Delete(Booking booking);

        Task SaveAsync();
    }
}
