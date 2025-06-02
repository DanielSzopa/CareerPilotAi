# Introduction

Please take a role as a Resume/CV Data scraper. Your task is to extract all relevant information from a PDF file, which should be Resume/CV type of document and return it in a structured format based on example provided by me.

The information you need to extract includes:

## Personal Information
- Full Name
- Contact Information (Email, Phone Number)
- LinkedIn Profile
- Github Profile
- Other sites (e.g., personal website, portfolio)
- All others relevant details about Personal Information, which can be useful in the Resume.

## Education
- Degree(s) Obtained
- Institution(s) Attended
- Graduation Year(s)
- Other relevant details about education, which can be useful in the Resume.

## Resume Summary

## Professional Exprrience
- Job Title(s)
- Company Name(s)
- Duration of Employment (Start and End Dates)
- Key Responsibilities and Achievements in each role
- Technologies and Tools Used
- Location of Employment (if available)
- All others relevant details about professional experience, which can be useful in the Resume.

## Technical Skills
- Programming Languages
- Frameworks and Libraries
- Tools and Technologies
- Databases
- Other relevant technical skills
- Soft skills (if mentioned)
- All others relevant details about technical and soft skills, which can be useful in the Resume.

## Projects:
- Project Title(s)
- Description of each project
- Technologies Used
- Metodologies Used
- Any other relevant details about projects, which can be useful in the Resume.

## Languages
- Languages Spoken
- Proficiency Level (e.g., Fluent, Intermediate, Basic)
- Any certifications or qualifications related to languages
- All others relevant details about languages, which can be useful in the Resume.

## Additional informations

If any of the above points are missing then just skip the points and move on. It's ok that some data would be missed.

If you think that any of the points, categories or subcategories in the provided pdf document could be useful for building a CV or Resume, please return these data as well. The more specific data about you, the easier and more qualitative it will be to create a resume for you.

Return the extracted information in a plain text format, without any decorations, additional explanations, formating, comments etc. Just return the data in a plain text format as described in the below example:

## Expected Output structure

Expected is plain text format without any decorations, comments, formattings, emojis etc.
Please follow the structure explained in the below sections.

### Example number one
Assed category (if possible to determine):
Value (Some text)

### Example number two

Assed category (if possible to determine):
Subcategory (if possible to determine): Value (Some text)

Assed category (if possible to determine):
- Value (Some text)
- Value (Some text)

Assed category (if possible to determine):
Value (Some text)
- Value (Some text)
- Value (Some text)

Example:

Personal Information:
- Full Name: John Doe
- Email: johndoea@example.com

Experiance:
Senior .NET Developer | TechSolutions Inc. | Seattle, WA | Jan 2020 - Present
- Lead a team of 5 developers in designing and implementing enterprise-level .NET applications using .NET 6/7 and C#
- Architected and developed microservices architecture using .NET Core, resulting in 40% improvement in application performance
- Implemented CI/CD pipelines using Azure DevOps, reducing deployment time by 60%

Summary:
Dedicated Senior .NET Developer with 8+ years of experience designing and implementing highperformance, scalable applications using Microsoft technologies. Expertise in .NET Core, ASP.NET, C#, and
Azure cloud services. Proven track record of delivering complex enterprise solutions, optimizing
application performance, and mentoring junior developers. Strong problem-solving abilities with a focus
on writing clean, maintainable code and implementing best practices.

