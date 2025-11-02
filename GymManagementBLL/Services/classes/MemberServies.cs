using AutoMapper;
using GymManagementBLL.Services.AttachmentServies;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemberViewModel;
using GymManagementDAL.Data.Repositories.Interfaces;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.classes
{
   public class MemberServies : IMemberServies
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAttachmentServies _attachmentServies;

        public MemberServies(IUnitOfWork unitOfWork , IMapper mapper , IAttachmentServies attachmentServies)
        {
            _unitOfWork = unitOfWork;
            this._mapper = mapper;
            this._attachmentServies = attachmentServies;
        }

        public bool CreateMember(CreateMemberViewModel createMemberViewModel)
        {
            try
            {
                if (IsEmailExist(createMemberViewModel.Email) || IsPhoneExist(createMemberViewModel.Phone)) 
                   return false;

                var uploadfile = _attachmentServies.Upload("Members", createMemberViewModel.PhotoFile);
                if(String.IsNullOrEmpty(uploadfile)) return false;


                var member = _mapper.Map<Member>(createMemberViewModel);
                member.Photo = uploadfile;
                 _unitOfWork.GetRepository<Member>().Add(member);
                 var iscreated = _unitOfWork.SaveChanges() > 0;
                if (!iscreated) { 
                
                    _attachmentServies.Delete("Members", uploadfile);
                    return false;
                }

                return iscreated;
            }
            catch (Exception)
            {

                return false;

            }
        }

        public IEnumerable<MemberViewModel> GetAll()
        {
           var members = _unitOfWork.GetRepository<Member>().GetAll();
            if (members is null || !members.Any()) return [];

            var memberviewmodel = _mapper.Map<IEnumerable<MemberViewModel>>(members);

            return memberviewmodel;

        }

        public MemberViewModel? GetMemberDetails(int id)
        {
            var memberdetails = _unitOfWork.GetRepository<Member>().GetById(id);
            if (memberdetails is null) return null;
            var memberDetailsViewModel = _mapper.Map<MemberViewModel>(memberdetails);

            // active membership name
            var Activemembership = _unitOfWork.GetRepository<MemberShip>().GetAll(X => X.MemebreId == memberdetails.Id && X.Status == "Active")
                                                        .FirstOrDefault();
            if (Activemembership is not null) { 
                memberDetailsViewModel.MembershipStartDate = Activemembership.CreatedAt.ToLongDateString(); 
                memberDetailsViewModel.MembershipEndDate = Activemembership.EndDate.ToLongDateString();

                // get plan name
                var plan = _unitOfWork.GetRepository<Plan>().GetById(Activemembership.PlanId);
                memberDetailsViewModel.PlanName = plan?.Name;
            }

            return memberDetailsViewModel;


        }

        public HealthRecordViewModel? GetMemberHealthRecord(int memberid)
        {
            var memberHealthRecord = _unitOfWork.GetRepository<HealthRecord>().GetById(memberid);
            if (memberHealthRecord is null) return null;
            return _mapper.Map<HealthRecordViewModel>(memberHealthRecord);
        }

        public MemberToUpdateViewModel? MemberToUpdate(int id)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(id);
            if (member is null) return null;
            return _mapper.Map<MemberToUpdateViewModel>(member);
        }

        public bool RemoveMember(int memberid)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(memberid);
            if(member is null) return false;

            // membersessionActive

            var IsactiveMemberSession = _unitOfWork.GetRepository<MemberSession>().GetAll(X => X.MemberId == memberid &&
                                                                 X.Session.StartDate > DateTime.Now).Any();
            if (IsactiveMemberSession) return false;

            var memberships = _unitOfWork.GetRepository<MemberShip>().GetAll(X => X.MemebreId == memberid);
            try {

                if (memberships.Any()) {

                    foreach (var membership in memberships) {

                        _unitOfWork.GetRepository<MemberShip>().Delete(membership);
                    
                    }

                }
                        _unitOfWork.GetRepository<Member>().Delete(member);
                        var isdeleted = _unitOfWork.SaveChanges() > 0;
                        if (!isdeleted)
                        {
                            _attachmentServies.Delete("Members", member.Photo);
                            return false;
                        }

                        return isdeleted;
            } catch 
            { 
             return false;
            }

        }

        public bool UpdateMemberDetails(int id, MemberToUpdateViewModel memberDetails)
        {
            var _memberRepository = _unitOfWork.GetRepository<Member>();
            try
            {

                var isemailexist = _unitOfWork.GetRepository<Member>().
                                    GetAll(X => X.Email == memberDetails.Email && X.Id != id);

                var isphoneexist = _unitOfWork.GetRepository<Member>().
                                     GetAll(X => X.Phone == memberDetails.Phone && X.Id != id);

                if (isemailexist.Any() ||  isphoneexist.Any()) return false;
                var member = _memberRepository.GetById(id);
                if (member is null) return false;
                _mapper.Map(memberDetails , member);
                 _memberRepository.Update(member);
                return _unitOfWork.SaveChanges() > 0;

            }
            catch {

                return false;
            
            }
        }


        #region Helper Methods
        private bool IsEmailExist(string Email)
        {

            return _unitOfWork.GetRepository<Member>().GetAll(X => X.Email == Email).Any();

        }

        private bool IsPhoneExist(string Phone)
        {

            return _unitOfWork.GetRepository<Member>().GetAll(X => X.Phone == Phone).Any();

        } 
        #endregion
    }
}
