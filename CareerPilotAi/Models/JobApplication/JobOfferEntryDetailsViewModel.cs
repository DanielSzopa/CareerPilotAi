using System.ComponentModel.DataAnnotations;
using CareerPilotAi.Models.CustomValidationAttributes;

namespace CareerPilotAi.Models.JobApplication
{
    public class JobOfferEntryDetailsViewModel
    {
        [Url(ErrorMessage = "Please enter a valid URL")]
        public string? Url { get; set; }

        [MaxWords(10000, ErrorMessage = "Text cannot exceed 10,000 words")]
        [Required(ErrorMessage = "Text is required")]
        public string Text { get; set; }

        public Guid? JobApplicationId { get; set; }
    }
} 