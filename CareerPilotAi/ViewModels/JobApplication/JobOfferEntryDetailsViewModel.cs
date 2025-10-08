using System.ComponentModel.DataAnnotations;
using CareerPilotAi.Application.CustomValidationAttributes;

namespace CareerPilotAi.ViewModels.JobApplication
{
    public class JobOfferEntryDetailsViewModel
    {
        [MaxWords(5000, ErrorMessage = "Job application description cannot exceed 5,000 words")]
        [Required(ErrorMessage = "Job application description is required")]
        public string JobDescription { get; set; } = string.Empty;

        [Required(ErrorMessage = "Title is required")]
        [MaxLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
        [MinLength(1, ErrorMessage = "Title must be at least 1 character long")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Company name is required")]
        [MaxLength(200, ErrorMessage = "Company name cannot exceed 200 characters")]
        public string Company { get; set; } = string.Empty;

        [Url(ErrorMessage = "Invalid URL format")]
        [MaxLength(2000, ErrorMessage = "URL cannot exceed 2000 characters")]
        public string? URL { get; set; }

        public Guid? JobApplicationId { get; set; }
    }
}