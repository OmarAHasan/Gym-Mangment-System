using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.TrainerViewModel;
using GymManagementDAL.Data.Repositories.Interfaces;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.classes
{
    public class TrainerServies : ITrainerServies
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TrainerServies(IUnitOfWork unitOfWork , IMapper mapper) {

                     _unitOfWork = unitOfWork;
            this._mapper = mapper;
        }


        public bool CreateTrainer(CreateTrainerViewModel createtrainer)
        {
            try
            {
                if (IsEmailExist(createtrainer.Email) || IsPhoneExist(createtrainer.Phone))
                    return false;

                // mapping 
                var trainer = _mapper.Map<Trainer>(createtrainer);
                _unitOfWork.GetRepository<Trainer>().Add(trainer);
                return _unitOfWork.SaveChanges() > 0;


            }
            catch (Exception)
            { 
            
                return false;
            
            }
        }

        public IEnumerable<TrainerViewModel> GetAll()
        {
            var trainers = _unitOfWork.GetRepository<Trainer>().GetAll();
            if (trainers is null || !trainers.Any()) return [];
            var trainersviewModel = _mapper.Map<IEnumerable<TrainerViewModel>>(trainers);
            return trainersviewModel;

        }

        public TrainerDetailsViewModel? GetTrainerDetails(int trainerid)
        {
            var trainer  = _unitOfWork.GetRepository<Trainer>().GetById(trainerid);
            if (trainer is null) return null;
            return _mapper.Map<TrainerDetailsViewModel>(trainer);
        }

        public TrainerToUpdateViewModel? GetTrainerToUpdate(int trainerid)
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(trainerid);
            if (trainer is null) return null;
            return _mapper.Map<TrainerToUpdateViewModel>(trainer);
        }
        public bool UpdateTrainer(int trainerid, TrainerToUpdateViewModel updatetrainer)
        {
            try
            {
                var repo = _unitOfWork.GetRepository<Trainer>();
                var isExist = repo.GetAll(x =>
                    (x.Email == updatetrainer.Email || x.Phone == updatetrainer.Phone)
                    && x.Id != trainerid
                ).Any();

                if (isExist)
                    return false;

                var trainer = repo.GetById(trainerid);
                if (trainer is null) return false;
                _mapper.Map(updatetrainer, trainer);
                trainer.UpdatedAt = DateTime.Now;

                repo.Update(trainer);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool RemoveTrainer(int trainerid)
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(trainerid);
            if(trainer is null) return false;

            // check if trainer has active session

            var HasactiveSession = _unitOfWork.GetRepository<Session>().GetAll(X => X.TrainerId == trainerid && X.StartDate > DateTime.Now).Any();
            if (HasactiveSession) return false;
            try
            {

                _unitOfWork.GetRepository<Trainer>().Delete(trainer);
                return _unitOfWork.SaveChanges() > 0;

            }
            catch (Exception) { 
            
              
                return false;

            }
        }




        #region HelperMethods

        private bool IsEmailExist(string Email)
        {

            return _unitOfWork.GetRepository<Trainer>().GetAll(X => X.Email == Email).Any();

        }

        private bool IsPhoneExist(string Phone)
        {

            return _unitOfWork.GetRepository<Trainer>().GetAll(X => X.Phone == Phone).Any();

        }

        #endregion
    }
}
