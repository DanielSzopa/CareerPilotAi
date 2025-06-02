using System.ComponentModel.DataAnnotations;
using CareerPilotAi.Models.CustomValidationAttributes;

namespace CareerPilotAi.Models.JobApplication
{
    public class UserDetailsViewModel
    {
        [Required(ErrorMessage = "User ID is required")]
        public Guid? JobApplicationId { get; set; }

        [Required(ErrorMessage = "Personal details are required")]
        [MaxWords(10000, ErrorMessage = "Personal details cannot exceed 10,000 words")]
        [Display(Name = "Personal Details")]
        public string PersonalDetails { get; set; } = string.Empty;
    }
}
