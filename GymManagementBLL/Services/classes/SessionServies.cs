using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.SessionViewModel;
using GymManagementDAL.Data.Repositories.Interfaces;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.classes
{
    public class SessionServies : ISessionServies

    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionServies(IUnitOfWork unitOfWork , IMapper mapper) 
        {
            _unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        public bool CeateSession(CreateSessionViewModel createSession)
        {
            try {

                // check if trainer exist
                if (!IsTrainerExist(createSession.TrainerId)) return false;
                // check if category exist
                if (!IsCategoryExsit(createSession.CategoryId)) return false;
                // checl if startdate < enddate
                if (!IsDateTimeValid(createSession.StartDate, createSession.EndDate)) return false;

                if (createSession.Capacity > 25 || createSession.Capacity < 0) return false;

                // munal mapping

                var createsession = _mapper.Map<Session>(createSession);
                _unitOfWork.GetRepository<Session>().Add(createsession);
                _unitOfWork.SaveChanges();
                return true;


            }catch(Exception ex)
            {
                Console.WriteLine($"Create Session Failed : {ex}");
                return false;
            
            }
        }

        public IEnumerable<SessionViewModel> GetAll()
        {
            // we wanna to load data from navigation property so we can not use get all of genericrepo ..
            var sessions = _unitOfWork.sessionRepository.GetSessionsWithCategoryAndTrainer();
            if (!sessions.Any()) return [];



                var mappedsessions = _mapper.Map<IEnumerable<Session>, IEnumerable<SessionViewModel>>(sessions);
            foreach (var session in mappedsessions) {

                session.Availableslots = session.Capacity - _unitOfWork.sessionRepository.GetCountOfBooking(session.Id);

            }

            return mappedsessions;
        }

        public SessionViewModel? GetSessionById(int sessionid)
        {
            var session = _unitOfWork.sessionRepository.GetSessionDetailsWithTrainerAndCategory(sessionid);
            if (session is null) return null;



            var mappedsession = _mapper.Map<Session, SessionViewModel>(session);
            mappedsession.Availableslots = mappedsession.Capacity - _unitOfWork.sessionRepository.GetCountOfBooking(mappedsession.Id);
            return mappedsession;
        }

        public SessionUpdateViewModel? GetSessionToUpdate(int sessionid)
        {
            var session = _unitOfWork.GetRepository<Session>().GetById(sessionid);
            // check if session completed and ongoing and has active booking => no update

            if (!CheckIfSessionIsAvailableForIpdate(session!)) return null;

            return _mapper.Map<SessionUpdateViewModel>(session);
        }

        public bool UpdateSession(int sessionid, SessionUpdateViewModel sessionupdate)
        {
            try
            {

                var session = _unitOfWork.GetRepository<Session>().GetById(sessionid);
                if (!CheckIfSessionIsAvailableForIpdate(session!)) return false;
                if (!IsTrainerExist(session!.TrainerId)) return false;
                if (!IsDateTimeValid(session.StartDate, session.EndDate)) return false;

                _mapper.Map(sessionupdate, session);
                session.UpdatedAt = DateTime.Now;
                _unitOfWork.GetRepository<Session>().Update(session);
                return _unitOfWork.SaveChanges() > 0;

            }
            catch (Exception ex) {

                Console.WriteLine($"Updated Session Failed : {ex}");
                return false;
            }
        }


        public bool RemoveSession(int sessionid)
        {
            try
            {

                var session = _unitOfWork.GetRepository<Session>().GetById(sessionid);
                if (session is null) return false;

                // check if session completed and has no active session
                if (CheckIfSessionIsAvailableForDelete(session!)) {

                    _unitOfWork.GetRepository<Session>().Delete(session!);
                    return _unitOfWork.SaveChanges() > 0;
                };
                return false;



            }
            catch (Exception ex) {
                Console.WriteLine($"Session Delete Failed : {ex} ");
                return false;
            
            }
        }

        public IEnumerable<TrainerSelectViewModel> GetAllTrainersForSelect()
        {
            var trainers = _unitOfWork.GetRepository<Trainer>().GetAll();
            return _mapper.Map<IEnumerable<TrainerSelectViewModel>>(trainers);
        }

        public IEnumerable<CategorySelectViewModel> GetAllCategoriesForSelect()
        {
           var categories = _unitOfWork.GetRepository<Category>().GetAll();
            return _mapper.Map<IEnumerable<CategorySelectViewModel>>(categories);
        }


        #region HelperMethods

        private bool IsTrainerExist(int trainerid) {

            return _unitOfWork.GetRepository<Trainer>().GetById(trainerid) is not null;
        
        }

        private bool IsCategoryExsit(int categoryid) {

            return _unitOfWork.GetRepository<Category>().GetById(categoryid) is not null;
        }

        private bool IsDateTimeValid(DateTime startdate, DateTime enddate) {

            return startdate < enddate;        
        }

        private bool CheckIfSessionIsAvailableForIpdate(Session session) {

            // Only future sessions with no bookings
            return session.StartDate > DateTime.Now &&
                   _unitOfWork.sessionRepository.GetCountOfBooking(session.Id) == 0;

        }


        private bool CheckIfSessionIsAvailableForDelete(Session session) {

            // check if session is completed 
            var sessioncompleted = session.EndDate < DateTime.Now;

            // if session not has active booking
            var hasnoactivebooking = _unitOfWork.sessionRepository.GetCountOfBooking(session.Id) <= 0;

            return sessioncompleted && hasnoactivebooking;

        
        }


        #endregion
    }
}
