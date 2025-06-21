using System.ComponentModel.DataAnnotations;
using CareerPilotAi.Models.CustomValidationAttributes;

namespace CareerPilotAi.Models.JobApplication
{
    public class PersonalInformationViewModel
    {
        [Required(ErrorMessage = "Personal information is required")]
        public PersonalInformationDetailsViewModel PersonalInformation { get; set; } = new();
        
        [MaxWords(1000, ErrorMessage = "Summary cannot exceed 1,000 words")]
        public string? Summary { get; set; }
        
        [MinimumCount(3, ErrorMessage = "At least 3 skills are required")]
        public List<SkillViewModel> Skills { get; set; } = new();
        
        public List<ExperienceViewModel> Experience { get; set; } = new();
        
        [MinimumCount(1, ErrorMessage = "At least one education entry is required")]
        public List<EducationViewModel> Education { get; set; } = new();
        
        public List<ProjectViewModel> Projects { get; set; } = new();
        public List<CertificationViewModel> Certifications { get; set; } = new();
        public List<LanguageViewModel> Languages { get; set; } = new();
        public List<OtherInformationViewModel> OtherInformation { get; set; } = new();
        
        public Guid? JobApplicationId { get; set; }
    }

    public class PersonalInformationDetailsViewModel
    {
        [Required(ErrorMessage = "Full name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Full name must be between 2 and 100 characters")]
        public string Fullname { get; set; } = "";
        
        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [StringLength(254, ErrorMessage = "Email address cannot exceed 254 characters")]
        public string Email { get; set; } = "";
        
        [Phone(ErrorMessage = "Invalid phone number format")]
        public string? Phone { get; set; }
        
        [StringLength(200, ErrorMessage = "Location cannot exceed 200 characters")]
        public string? Location { get; set; }
        
        public List<LinkViewModel> Links { get; set; } = new();
    }

    public class LinkViewModel
    {
        [Required(ErrorMessage = "Label is required")]
        [StringLength(50, ErrorMessage = "Label cannot exceed 50 characters")]
        public string Label { get; set; } = "";
        
        [Required(ErrorMessage = "URL is required")]
        public string Url { get; set; } = "";
    }

    public class SkillViewModel
    {
        [Required(ErrorMessage = "Skill name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Skill name must be between 2 and 100 characters")]
        public string Label { get; set; } = "";
        
        [StringLength(50, ErrorMessage = "Level cannot exceed 50 characters")]
        [CustomValidationAttributes.AllowedValues("Junior", "Regular", "Expert", ErrorMessage = "Skill level must be Junior, Regular, or Expert.")]
        public string? Level { get; set; }
    }

    public class ExperienceViewModel
    {
        [Required(ErrorMessage = "Job title is required")]
        [StringLength(200, ErrorMessage = "Job title cannot exceed 200 characters")]
        public string Title { get; set; } = "";
        
        [Required(ErrorMessage = "Company name is required")]
        [StringLength(200, ErrorMessage = "Company name cannot exceed 200 characters")]
        public string Company { get; set; } = "";
        
        [StringLength(200, ErrorMessage = "Location cannot exceed 200 characters")]
        public string? Location { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }
        
        public bool Present { get; set; }
        
        public List<string> ListOfContentPoints { get; set; } = new();
    }

    public class EducationViewModel
    {
        [Required(ErrorMessage = "Degree is required")]
        [StringLength(200, ErrorMessage = "Degree cannot exceed 200 characters")]
        public string Degree { get; set; } = "";
        
        [Required(ErrorMessage = "Institution is required")]
        [StringLength(200, ErrorMessage = "Institution cannot exceed 200 characters")]
        public string Institution { get; set; } = "";
        
        [StringLength(200, ErrorMessage = "Location cannot exceed 200 characters")]
        public string? Location { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }
        
        public bool Present { get; set; }
    }

    public class ProjectViewModel
    {
        [Required(ErrorMessage = "Project title is required")]
        [StringLength(200, ErrorMessage = "Project title cannot exceed 200 characters")]
        public string Title { get; set; } = "";
        
        public List<string> ListOfContentPoints { get; set; } = new();
        public List<string> Technologies { get; set; } = new();
        
        [Url(ErrorMessage = "Invalid URL format")]
        public string? Link { get; set; }
    }

    public class CertificationViewModel
    {
        [Required(ErrorMessage = "Certification title is required")]
        [StringLength(200, ErrorMessage = "Certification title cannot exceed 200 characters")]
        public string Title { get; set; } = "";
        
        [Required(ErrorMessage = "Issuer is required")]
        [StringLength(200, ErrorMessage = "Issuer cannot exceed 200 characters")]
        public string Issuer { get; set; } = "";
        
        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }
        
        [StringLength(1000, ErrorMessage = "Description cannot exceed 1,000 characters")]
        public string? Description { get; set; }
        
        [Url(ErrorMessage = "Invalid URL format")]
        public string? Link { get; set; }
    }

    public class LanguageViewModel
    {
        [Required(ErrorMessage = "Language is required")]
        [StringLength(100, ErrorMessage = "Language cannot exceed 100 characters")]
        public string Language { get; set; } = "";
        
        [Required(ErrorMessage = "Proficiency level is required")]
        [StringLength(50, ErrorMessage = "Proficiency cannot exceed 50 characters")]
        [CustomValidationAttributes.AllowedValues("Native", "Fluent", "Advanced", "Intermediate", "Basic", ErrorMessage = "Language proficiency must be Native, Fluent, Advanced, Intermediate, or Basic.")]
        public string Proficiency { get; set; } = "";
    }

    public class OtherInformationViewModel
    {
        [Required(ErrorMessage = "Label is required")]
        [StringLength(100, ErrorMessage = "Label cannot exceed 100 characters")]
        public string Label { get; set; } = "";
        
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
        public string? Title { get; set; }
        
        public List<string> ListOfContentPoints { get; set; } = new();
    }
} 