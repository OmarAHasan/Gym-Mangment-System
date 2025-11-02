using GymManagementBLL.ViewModels.TrainerViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    public interface ITrainerServies
    {
        // get all trainers
        IEnumerable<TrainerViewModel> GetAll();

        // get Trainer ById
        TrainerDetailsViewModel? GetTrainerDetails(int trainerid);

        // add Trainer
        bool CreateTrainer(CreateTrainerViewModel createtrainer);

        // Get Member To update
        TrainerToUpdateViewModel? GetTrainerToUpdate(int trainerid);

        //update Trainer
        bool UpdateTrainer(int trainerid, TrainerToUpdateViewModel updatetrainer);

        // remove Trainer
        bool RemoveTrainer(int trainerid);


    }
}
