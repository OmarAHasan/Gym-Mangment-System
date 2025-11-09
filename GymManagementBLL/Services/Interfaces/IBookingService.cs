using GymManagementBLL.Helpers;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    public interface IBookingService
    {
        Task<IEnumerable<Booking>> GetUpcomingAndOngoingForMemberAsync(int memberId);
        Task<OperationResult> CreateBookingAsync(int memberId, int sessionId);
        Task<OperationResult> CancelBookingAsync(int bookingId, int memberId);
        Task<IEnumerable<Member>> GetMembersForUpcomingSessionAsync(int sessionId);
        Task<IEnumerable<Member>> GetMembersForOngoingSessionAsync(int sessionId);
        Task<OperationResult> MarkAttendanceAsync(int bookingId, bool isAttended);
    }
}
