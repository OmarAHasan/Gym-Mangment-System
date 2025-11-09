using AutoMapper;
using GymManagementBLL.Helpers;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemberShipViewModel;
using GymManagementDAL.Data.Context;
using GymManagementDAL.Data.Repositories.Interfaces;
using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.classes
{
    public class MemberShipServies : IMemberShipServies
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MemberShipServies(IUnitOfWork unitOfWork , IMapper mapper) {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public IEnumerable<MemberShip> GetAll()
        {
            return _unitOfWork.MemberShipRepository.GetAll();
        }
        public bool CreateMemberShip(CreateMemberShipViewModel createMemberShipViewModel)
        {

            try
            {
                var memberRepo = _unitOfWork.GetRepository<Member>();
                var planRepo = _unitOfWork.GetRepository<Plan>();
                var memberShipRepo = _unitOfWork.GetRepository<MemberShip>();

                // check if member exist
                var member = memberRepo.GetById(createMemberShipViewModel.MemberId);
                if (member is null) return false;

                // check if plan exist
                var plan = planRepo.GetById(createMemberShipViewModel.PlanId);
                if (plan is null) return false;

                // check if member has active membership
                bool hasActiveMembership = memberShipRepo.GetAll(X => X.MemebreId == member.Id && X.EndDate > DateTime.Now && X.CreatedAt < DateTime.Now).Any();
                if (hasActiveMembership) return false;

                if (!plan.IsActive) return false;

                var membership = _mapper.Map<MemberShip>(createMemberShipViewModel);
                 membership.EndDate = DateTime.Now.AddDays(plan.DurationDays);
                _unitOfWork.GetRepository<MemberShip>().Add(membership);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Cant Create MemberShip {ex}");
                return false;
            }



        }

        public IEnumerable<MemberShipViewModel> Getall()
        {
            var memberShips = _unitOfWork.MemberShipRepository.GetMemberShipsWithLodedData();

            if (memberShips is null || !memberShips.Any()) return [];

            var membershipviewmodel = _mapper.Map<IEnumerable<MemberShipViewModel>>(memberShips);
            return membershipviewmodel;


        }

        public IEnumerable<MemberSelectViewModel> GetMembersFroSelect()
        {
            var members = _unitOfWork.GetRepository<Member>().GetAll();
            return _mapper.Map<IEnumerable<MemberSelectViewModel>>(members);
        }

        public IEnumerable<PlanSelectViewModel> GetPlansForSelect()
        {
            var plans = _unitOfWork.GetRepository<Plan>().GetAll();
            return _mapper.Map<IEnumerable<PlanSelectViewModel>>(plans);
        }




        public bool RemoveMemberShip(int memberid ,int planid)
        {

            var activeMembership = _unitOfWork.MemberShipRepository.GetAll(X => X.MemebreId == memberid && X.PlanId == planid 
                                                                           && X.EndDate > DateTime.Now).FirstOrDefault();
            if (activeMembership is null) return false;

            try
            {
                _unitOfWork.GetRepository<MemberShip>().Delete(activeMembership);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Cant Remove MemberShip {ex}");
                return false;
            }


        }
        //private readonly GymDbContext _unitOfWork;
        //public MemberShipServies(GymDbContext db) { _unitOfWork = db; }

        public async Task<IEnumerable<MemberShip>> GetAllAsync()
        {
            return await _unitOfWork.MemberShipRepository.GetAllAsync() ?? Enumerable.Empty<MemberShip>();
        }

        public async Task<MemberShip?> GetByIdAsync(int id)
        {
            return await _unitOfWork.MemberShipRepository.GetByIdAsync(id);
        }

        public async Task<bool> MemberHasActiveMembershipAsync(int memberId)
        {
            var allMemberships = await _unitOfWork.MemberShipRepository.GetAllAsync();
            return allMemberships.Any(m => m.MemebreId == memberId && m.EndDate > DateTime.UtcNow);
        }

        public async Task<OperationResult> CreateAsync(int memberId, int planId, DateTime startDate)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(memberId);
            if (member == null) return OperationResult.Fail("Member not found.");

            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(planId);
            if (plan == null) return OperationResult.Fail("Plan not found.");
            if (!plan.IsActive) return OperationResult.Fail("Plan is not active.");

            if (await MemberHasActiveMembershipAsync(memberId))
                return OperationResult.Fail("Member already has an active membership.");

            var membership = new MemberShip
            {
                MemebreId = memberId,
                PlanId = planId,
                CreatedAt = startDate,
                EndDate = startDate.AddDays(plan.DurationDays)
            };

            await _unitOfWork.MemberShipRepository.AddAsync(membership);
            await _unitOfWork.SaveChangesAsync();

            return OperationResult.Ok();
        }

        public async Task<OperationResult> CancelAsync(int membershipId)
        {
            var membership = await _unitOfWork.MemberShipRepository.GetByIdAsync(membershipId);
            if (membership == null) return OperationResult.Fail("Membership not found.");

            if (membership.EndDate <= DateTime.UtcNow)
                return OperationResult.Fail("Membership is not active and cannot be cancelled.");

            await _unitOfWork.MemberShipRepository.DeleteAsync(membership);
            await _unitOfWork.SaveChangesAsync();

            return OperationResult.Ok();
        }
    }
}
