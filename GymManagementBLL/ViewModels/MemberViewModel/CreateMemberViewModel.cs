using GymManagementDAL.Entities.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.MemberViewModel
{
    public class CreateMemberViewModel
    {
        [Required(ErrorMessage ="ProfilePhoto Is Required")]
        [Display(Name ="Prpfile Photo")]
        public IFormFile PhotoFile { get; set; } = null!;
        // define valdiation
        [Required(ErrorMessage = "Name Is Requierd")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 50 characters")]
        [RegularExpression(@"^[a-zA-Z\s]+$" , ErrorMessage ="Name Can Contain Only Letters And Spaces")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage ="Email Is Required")]
        [EmailAddress(ErrorMessage ="Valid Email Format")] // validation
        [DataType(DataType.EmailAddress)] // ui hint  
        [StringLength(100 , MinimumLength = 5 , ErrorMessage ="Name Must Be Between 100 and 5 Char")]
        public string Email { get; set; } = null!;


        [Required(ErrorMessage ="Phone Is Requierd")]
        [Phone(ErrorMessage ="Valid Phone Format")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage ="PhoneNumber Must Be Valid Egyption Phone Number")]
        public string Phone { get; set; } = null!;

        [Required(ErrorMessage ="DateOfBirth Is Required")]
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; } 

        [Required(ErrorMessage ="Gender Is Required")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "BuildingNumber Is Required")]
        [Range(1 , 9000 , ErrorMessage = "BuildingNumber Must Be Btween 1 And 9000")]
        public int BuildingNumber { get; set; }

        [Required(ErrorMessage ="Street Is Required")]
        [StringLength(30 ,MinimumLength =2 , ErrorMessage ="Street Must Be Between 2 And 30 Char")]
        public string street { get; set; } = null!;


        [Required(ErrorMessage = "City Is Required")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "City Must Be Between 2 And 30 Char")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "City Can Contain Only Letters And Spaces")]
        public string City { get; set; } = null!;

        [Required(ErrorMessage = "Health Record Is Required")]

        public HealthRecordViewModel HealthRecord { get; set; } = null!;

    }


}
