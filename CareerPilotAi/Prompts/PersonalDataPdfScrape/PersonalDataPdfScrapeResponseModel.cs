using System.Text.Json.Serialization;

namespace CareerPilotAi.Prompts.PersonalDataPdfScrape;

public class PersonalDataPdfScrapeResponseModel
{
    public bool IsProvidedDataValid { get; set; }
    public Content? Content { get; set; }
}

public class Content
{
    public Personalinformation? PersonalInformation { get; set; }
    public string? Summary { get; set; }
    public Skill[]? Skills { get; set; }
    public Experience[]? Experience { get; set; }
    public Education[]? Education { get; set; }
    public Project[]? Projects { get; set; }
    public Certification[]? Certifications { get; set; }
    public Language[]? Languages { get; set; }
    public Otherinformation[]? OtherInformation { get; set; }
}

public class Personalinformation
{
    public string? Fullname { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Location { get; set; }
    public Link[]? Links { get; set; }
}

public class Link
{
    public string? Label { get; set; }
    public string? Url { get; set; }
}

public class Skill
{
    public string? Label { get; set; }
    public string? Level { get; set; }
}

public class Experience
{
    public string? Title { get; set; }
    public string? Company { get; set; }
    public string? Location { get; set; }
    public string? StartDate { get; set; }
    public string? EndDate { get; set; }
    public bool Present { get; set; }
    public string[]? ListOfContentPoints { get; set; }
}

public class Education
{
    public string? Degree { get; set; }
    public string? Institution { get; set; }
    public string? Location { get; set; }
    public string? StartDate { get; set; }
    public string? EndDate { get; set; }
    public bool Present { get; set; }
}

public class Project
{
    public string? Title { get; set; }
    public string[]? ListOfContentPoints { get; set; }
    public string[]? Technologies { get; set; }
    public string? Link { get; set; }
}

public class Certification
{
    public string? Title { get; set; }
    public string? Issuer { get; set; }
    public string? Date { get; set; }
    public string? Description { get; set; }
    public string? Link { get; set; }
}

public class Language
{
    [JsonPropertyName("Language")]
    public string? Value { get; set; }
    public string? Proficiency { get; set; }
}

public class Otherinformation
{
    public string? Label { get; set; }
    public string? Title { get; set; }
    public string[]? ListOfContentPoints { get; set; }
}
