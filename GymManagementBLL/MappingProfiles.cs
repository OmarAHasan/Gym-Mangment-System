using AutoMapper;
using GymManagementBLL.ViewModels.MemberShipViewModel;
using GymManagementBLL.ViewModels.MemberViewModel;
using GymManagementBLL.ViewModels.PlanViewModel;
using GymManagementBLL.ViewModels.SessionViewModel;
using GymManagementBLL.ViewModels.TrainerViewModel;
using GymManagementDAL.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL
{
    public class MappingProfiles : Profile
    {

        public MappingProfiles() 
        {

            MappingSessions();

            MappingMembers();

            MappingTrainers();

            MappingPlans();

            MappingMemberShips();

        }

        private void MappingSessions() {

            #region Session Mapping
            CreateMap<Session, SessionViewModel>()
        .ForMember(dst => dst.CategoryName, options =>
        {

            options.MapFrom(src => src.Category.CategoryName);
        })
        .ForMember(dst => dst.CategoryName, options =>
        {

            options.MapFrom(src => src.SessionTrainer.Name);

        })
        .ForMember(dst => dst.Availableslots, options => options.Ignore());

            CreateMap<CreateSessionViewModel, Session>();
            CreateMap<Session, SessionUpdateViewModel>().ReverseMap();

            CreateMap<Trainer, TrainerSelectViewModel>();
            CreateMap<Category, CategorySelectViewModel>()
                        .ForMember(dst => dst.Name , options => options.MapFrom(src => src.CategoryName));
            #endregion
        }

        private void MappingMembers()
        {
            #region Member Mapping

            CreateMap<CreateMemberViewModel, Member>().ForMember(dst => dst.Address, options => options.MapFrom(src => new Address
            {

                BulidingNumber = src.BuildingNumber,
                Street = src.street,
                City = src.City,

            }))
            .ForMember(dst => dst.HealthRecord, options => options.MapFrom(src => new HealthRecord
            {

                BloodType = src.HealthRecord.BloodType,
                Weight = src.HealthRecord.Weight,
                Height = src.HealthRecord.Height,
                Note = src.HealthRecord.Note,

            }));

            CreateMap<Member, MemberViewModel>().ForMember(dst => dst.Gender, options => options.MapFrom(src => src.Gender.ToString()))
                                                .ForMember(dst => dst.DateOfBirth, options => options.MapFrom(src => src.DateOfBirth.ToShortDateString()))
                                                .ForMember(dst => dst.Address, options => { options.MapFrom(src => $"{src.Address.BulidingNumber} - {src.Address.Street} - {src.Address.City}"); });

            CreateMap<HealthRecord, HealthRecordViewModel>();
            CreateMap<Member, MemberToUpdateViewModel>()
                .ForMember(dst => dst.BuildingNumber, opt => opt.MapFrom(src => src.Address.BulidingNumber))
                .ForMember(dst => dst.Street, opt => opt.MapFrom(src => src.Address.Street))
                .ForMember(dst => dst.City, opt => opt.MapFrom(src => src.Address.City));


            CreateMap<MemberToUpdateViewModel, Member>()
                .ForPath(dst => dst.Address.BulidingNumber, opt => opt.MapFrom(src => src.BuildingNumber))
                .ForPath(dst => dst.Address.Street, opt => opt.MapFrom(src => src.Street))
                .ForPath(dst => dst.Address.City, opt => opt.MapFrom(src => src.City))
                .ForMember(dst => dst.Name, opt => opt.Ignore())
                .ForMember(dst => dst.Photo, opt => opt.Ignore());





            #endregion
        }

        private void MappingTrainers() {
            #region Trainer Mapping 

            // create trainer => map from trainercreateviewmodel to trainer
            CreateMap<CreateTrainerViewModel, Trainer>().ForMember(dst => dst.Address, options => options.MapFrom(

                src => new Address
                {

                    BulidingNumber = src.BuildingNumber,
                    Street = src.Street,
                    City = src.City,
                }
                ))
                                                        .ForMember(dst => dst.Specialties, options => options.MapFrom(src => src.Specialties));

            // getAll trainers => map from trainer to trainerviewmodel 
            CreateMap<Trainer, TrainerViewModel>()
                     .ForMember(dst => dst.Specialization, options => options.MapFrom(src => src.Specialties.ToString()));

            // get trainer Details => map from trainer to trainerdetailsviewmodel
            CreateMap<Trainer, TrainerDetailsViewModel>().ForMember(dst => dst.Specialization, options => options.MapFrom(src => src.Specialties.ToString()))
                                                         .ForMember(dst => dst.DateOfBirth, options => options.MapFrom(src => src.DateOfBirth.ToShortDateString()))
                                                         .ForMember(dst => dst.Address, options => { options.MapFrom(src => $"{src.Address.BulidingNumber} - {src.Address.Street} - {src.Address.City}"); });


            // get trainer to update => map from trainer to trainertoupdateviewmodel
            CreateMap<Trainer, TrainerToUpdateViewModel>().ForMember(dst => dst.BuildingNumber, options => options.MapFrom(src => src.Address.BulidingNumber))
                                                          .ForMember(dst => dst.street, options => options.MapFrom(src => src.Address.Street))
                                                          .ForMember(dst => dst.city, options => options.MapFrom(src => src.Address.City))
                                                          .ForMember(dst => dst.Specialization, options => options.MapFrom(src => src.Specialties));

            // update trainer => map from trainertoupdateviewmodel to trainer
            CreateMap<TrainerToUpdateViewModel, Trainer>()
                .ForPath(dst => dst.Address.BulidingNumber, opt => opt.MapFrom(src => src.BuildingNumber))
                .ForPath(dst => dst.Address.Street, opt => opt.MapFrom(src => src.street))
                .ForPath(dst => dst.Address.City, opt => opt.MapFrom(src => src.city))
                .ForMember(dst => dst.Specialties, opt => opt.MapFrom(src => src.Specialization))
                .ForMember(dst => dst.Name, opt => opt.Ignore());




            #endregion
        }


        private void MappingPlans() {
            #region Plan Mapping

            // getall plans => map from plan to planviewmodel
            CreateMap<Plan, PlanViewModel>();

            // getplan details to update => map from plan to plantoupdateviwemodel and reverse map in update
            CreateMap<Plan, PlanToUpdateViewModel>().ReverseMap();


            #endregion
        }

        private void MappingMemberShips() { 
        
        
            CreateMap<MemberShip , MemberShipViewModel>()
                      .ForMember(dst => dst.MemberName , options => options.MapFrom(src => src.Memebre.Name))
                      .ForMember(dst => dst.PlanName , options => options.MapFrom(src => src.Plan.Name))
                      .ForMember(dst => dst.StartDate , options => options.MapFrom(src => src.CreatedAt));

            CreateMap<CreateMemberShipViewModel, MemberShip>()
                .ForMember(dst => dst.MemebreId, opt => opt.MapFrom(src => src.MemberId))
                .ForMember(dst => dst.PlanId, opt => opt.MapFrom(src => src.PlanId))
                ; 


            // Mapping for Plan → PlanSelectViewModel
            CreateMap<Plan, PlanSelectViewModel>();

            // Mapping for Member → MemberSelectViewModel
            CreateMap<Member, MemberSelectViewModel>();


        }

    }
}
