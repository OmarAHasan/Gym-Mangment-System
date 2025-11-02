using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.PlanViewModel;
using GymManagementDAL.Data.Repositories.Interfaces;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.classes
{



    public class PlanServies : IPlanServies
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        // we need object from IUnitOfWork

        public PlanServies(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            this._mapper = mapper;
        }
        public IEnumerable<PlanViewModel> GetAll()
        {
            var plans = _unitOfWork.GetRepository<Plan>().GetAll();
            if (plans is null || !plans.Any()) return [];
            return _mapper.Map<IEnumerable<PlanViewModel>>(plans);

        }

        public PlanViewModel? GetById(int id)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(id);
            if (plan is null) return null;
            return _mapper.Map<PlanViewModel>(plan);
        }

        public PlanToUpdateViewModel? GetPlanToUpdate(int planid)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(planid);
            if (plan is null || plan.IsActive == false || HasActiveMemberShip(planid))
                return null;
            return _mapper.Map<PlanToUpdateViewModel>(plan);        }
        public bool UpdatePlan(int planid, PlanToUpdateViewModel UpdatePlan)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(planid);
            if (plan is null || HasActiveMemberShip(planid)) return false;
            try
            {
                _mapper.Map(UpdatePlan , plan);
                plan.UpdatedAt = DateTime.Now;

                _unitOfWork.GetRepository<Plan>().Update(plan);
                return  _unitOfWork.SaveChanges() > 0;

            }
            catch {

                return false;
            
            }
        }


        public bool ToggelStatus(int planid)
        {
            var repo = _unitOfWork.GetRepository<Plan>();
            var plan = repo.GetById(planid);
            if(plan is null || HasActiveMemberShip(planid)) return false;

            plan.IsActive = plan.IsActive == true ? false : true;

            try {
            
                repo.Update(plan);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch {
                return false;


            }
        }




        #region Helper Methods
        private bool HasActiveMemberShip(int planid)
        {

            var avtivememberShip = _unitOfWork.GetRepository<MemberShip>().GetAll(X => X.PlanId == planid && X.Status == "Active").Any();
            return avtivememberShip;
        } 
        #endregion
    }
}
