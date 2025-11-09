using GymManagementBLL.Helpers;
using GymManagementBLL.Services.Interfaces;
using GymManagementDAL.Data.Context;
using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.classes
{
    public class BookingService : IBookingService
    {
        private readonly GymDbContext _unitOfWork;
        public BookingService(GymDbContext db) { _unitOfWork = db; }

        public async Task<IEnumerable<Booking>> GetUpcomingAndOngoingForMemberAsync(int memberId)
        {
            var now = DateTime.UtcNow;
            return await _unitOfWork.Bookings
                            .Include(b => b.Session)
                            .Where(b => b.MemberId == memberId && b.Session.EndDate > now)
                            .OrderBy(b => b.Session.StartDate)
                            .ToListAsync();
        }

        public async Task<OperationResult> CreateBookingAsync(int memberId, int sessionId)
        {

            var session = await _unitOfWork.Sessions.FindAsync(sessionId);
            if (session == null) return OperationResult.Fail("Session not found.");

            var now = DateTime.UtcNow;

            if (session.StartDate <= now) return OperationResult.Fail("Cannot book past or ongoing sessions.");


            bool hasActive = await _unitOfWork.memberShips.AnyAsync(m => m.MemebreId == memberId && m.EndDate > now);
            if (!hasActive) return OperationResult.Fail("Member does not have an active membership.");

            var currentCount = await _unitOfWork.Bookings.CountAsync(b => b.SessionId == sessionId);
            if (currentCount >= session.Capacity) return OperationResult.Fail("Session capacity is full.");

            bool alreadyBooked = await _unitOfWork.Bookings.AnyAsync(b => b.MemberId == memberId && b.SessionId == sessionId);
            if (alreadyBooked) return OperationResult.Fail("Member already booked this session.");


            var booking = new Booking
            {
                MemberId = memberId,
                SessionId = sessionId,
                CreatedAt = now,
                IsAttended = false // Rule 7
            };

            _unitOfWork.Bookings.Add(booking);
            await _unitOfWork.SaveChangesAsync();
            return OperationResult.Ok();
        }

        public async Task<OperationResult> CancelBookingAsync(int bookingId, int memberId)
        {
            var booking = await _unitOfWork.Bookings
                                   .Include(b => b.Session)
                                   .FirstOrDefaultAsync(b => b.Id == bookingId && b.MemberId == memberId);
            if (booking == null) return OperationResult.Fail("Booking not found.");

            var now = DateTime.UtcNow;

            // Rule 5: can only cancel future sessions
            if (booking.Session.StartDate <= now)
                return OperationResult.Fail("Cannot cancel a booking for a session that has already started.");

            _unitOfWork.Bookings.Remove(booking);
            await _unitOfWork.SaveChangesAsync();
            return OperationResult.Ok();
        }

        public async Task<IEnumerable<Member>> GetMembersForUpcomingSessionAsync(int sessionId)
        {
            var now = DateTime.UtcNow;
            var session = await _unitOfWork.Sessions.FindAsync(sessionId);
            if (session == null) return new List<Member>();

            if (session.StartDate <= now) return new List<Member>(); // not upcoming

            return await _unitOfWork.Bookings
                        .Where(b => b.SessionId == sessionId)
                        .Select(b => b.Member)
                        .ToListAsync();
        }

        public async Task<IEnumerable<Member>> GetMembersForOngoingSessionAsync(int sessionId)
        {
            var now = DateTime.UtcNow;
            var session = await _unitOfWork.Sessions.FindAsync(sessionId);
            if (session == null) return new List<Member>();

            if (!(session.StartDate <= now && session.EndDate > now)) return new List<Member>(); // not ongoing

            return await _unitOfWork.Bookings
                        .Where(b => b.SessionId == sessionId)
                        .Select(b => b.Member)
                        .ToListAsync();
        }

        public async Task<OperationResult> MarkAttendanceAsync(int bookingId, bool isAttended)
        {
            var booking = await _unitOfWork.Bookings
                                   .Include(b => b.Session)
                                   .FirstOrDefaultAsync(b => b.Id == bookingId);
            if (booking == null) return OperationResult.Fail("Booking not found.");

            var now = DateTime.UtcNow;
            // Rule 6: attendance only for ongoing sessions
            if (!(booking.Session.StartDate <= now && booking.Session.EndDate > now))
                return OperationResult.Fail("Can only mark attendance for an ongoing session.");

            booking.IsAttended = isAttended;
            await _unitOfWork.SaveChangesAsync();
            return OperationResult.Ok();
        }
    }
}
