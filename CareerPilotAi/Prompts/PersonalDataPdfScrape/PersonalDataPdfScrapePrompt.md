# Introduction

Please take a role as a Resume/CV Data scraper. Your task is to extract all relevant information from the attached text, which should be Resume/CV type of document and return it in a structured json format based on example provided by me.

The information you need to extract includes:

## Personal Information
- Full Name
- Contact Information (Email, Phone Number)
- LinkedIn Profile
- Github Profile
- Other sites (e.g., personal website, portfolio)

## Education
- Degree(s) Obtained
- Institution(s) Attended
- Graduation Year(s)

## Resume Summary

## Professional Experience
- Job Title(s)
- Company Name(s)
- Duration of Employment (Start and End Dates)
- Key Responsibilities and Achievements in each role
- Technologies and Tools Used (if available)
- Location (City, Country if available)
- All others relevant details about professional experience, which can be useful in the Resume.

## Technical Skills
- Programming Languages
- Frameworks and Libraries
- Tools
- Methodologies (e.g., Agile, Scrum, other project methodologies)
- Design patterns
- Architectures
- Databases
- Other relevant technical skills
- Soft skills (if mentioned)
- Skills Level based on required rules for mapping values
- All others relevant details about technical and soft skills, which can be useful in the Resume.

### Skills level required rules

Allowed output values: "Junior", "Regular", "Expert"

For the skills level:

- Map terms like 'beginner', 'learning', 'novice', 'intern', 'trainee' '1-2 years of experience' to "Junior".
- Map terms like 'proficient', 'comfortable', 'solid understanding', 'intermediate', 'Advanced' '3-5 years of experience' to "Regular".
- Map terms like 'expert', 'advanced', 'mastery', 'lead', 'architect', 'senuior' '5+ years of experience' to "Expert".
- If the proficiency is not mentioned or cannot be confidently determined, you MUST use the value n/a.
- Don't use any other values than the ones mentioned above.

## Projects:
- Project Title(s)
- Description of each project
- Technologies Used
- Metodologies Used
- Any other relevant details about projects, which can be useful in the Resume.

## Languages
- Languages Spoken
- Proficiency Level

### Proficiency rules only for Languages

Allowed output values: "Native", "Fluent", "Advanced", "Intermediate", "Basic" 

For the Languages proficiency:

- Map terms like 'mother tongue', 'native speaker' to "Native".
- Map terms like 'C2', 'fully proficient', 'bilingual proficiency' to "Fluent".
- Map terms like 'C1', 'business level', 'professional working proficiency' to "Advanced".
- Map terms like 'B2', 'conversational', 'limited working proficiency' to "Intermediate".
- Map terms like 'A1', 'A2', 'B1', 'beginner', 'elementary proficiency' to "Basic".
- If the proficiency is not mentioned or cannot be mapped, you MUST use the value n/a.
- Don't use any other values than the ones mentioned above.

## Certifications
- Certification Title
- Issuing Organization
- Date of Issue
- Link to certificate (if available)
- Brief description (if available)

## Additional informations

If any of the above points are missing then just skip the points and move on. It's ok that some data would be missed.
If you think that any of the points, categories or subcategories in the provided content could be useful for building a CV or Resume,
please return these data as well and put that into "OtherInformation" json property in the format described below.


```json

"OtherInformation": [
    {
      "Label": "Hobbies",
      "Title": "Interests",
      "ListOfContentPoints": [
        "Read the book",
        "Play the guitar"
      ]
    },
    {
      "Label": "Achievements",
      "Title": "",
      "ListOfContentPoints": [
        "First place in the C# programming contest"
      ]
    }
  ]

```

Your primary task is to understand the semantic meaning of sections and data points within the resume, even if it's written in a language other than English (e.g., Polish, German, Spanish). You must then map these understood pieces of information to the corresponding English keys in the JSON structure provided. For example, if the resume has a section titled 'Umiejętności' (Polish for Skills), its content should be placed under the 'Skills' key in the output JSON. Similarly, 'Doświadczenie Zawodowe' should map to 'Experience', 'Edukacja' to 'Education', and so on.
The values you extract should remain in their original language as found in the document.

Return the extracted information in json format, without any decorations, additional explanations, formating, comments etc. Just return the data in a json format as described in the below example.

## Expected Output structure

Set **IsProvidedDataValid** to `true` if the input text content appears to be a resume or cv. Set to `false` if the input text does not resemble a resume/CV (e.g., it's a news article, a random story, or gibberish).
If `false`, the Content field should be null.
Warning: Don't create other properties which are not described in the json structure below. Use OtherInformation property to store any additional information which is not covered by the properties below.

```json

{
  "IsProvidedDataValid": true,
  "Content": {
    "PersonalInformation": {
      "Fullname": " Jon Bocker",
      "Email": "jonbocker@email.com",
      "Phone": "+48 535 535 535",
      "Location": "Warsaw, Poland",
      "Links": [
        {
          "Label": "LinkedIn",
          "Url": "https://www.linkedin.com/in/jonbocker/"
        },
        {
          "Label": "GitHub",
          "Url": "github.com/jonbocker"
        },
        {
          "Label": "Portfolio",
          "Url": "www.jonbocker.com"
        }
      ]
    },
    "Summary": "Experienced Software Engineer specializing in React.js, building dynamic and user-friendly web applications. Proficient in JavaScript, HTML, CSS, and modern front-end frameworks, with strong teamwork and communication skills. Committed to continuous learning and applying best practices for efficient project delivery.",
    "Skills": [
      {
        "Label": "React.js",
        "Level": "Expert"
      },
      {
        "Label": "C#",
        "Level": "Junior"
      },
      {
        "Label": "SQL Server",
        "Level": "Regular"
      },
      {
        "Label": "Scrum",
        "Level": "n/a"
      },
      {
        "Label": "Vertical slices Architecture",
        "Level": "n/a"
      },
      {
        "Label": "Visual Studio Code",
        "Level": "n/a"
      },
      {
        "Label": "Problem solving",
        "Level": "n/a"
      },
      {
        "Label": "Leadership",
        "Level": "n/a"
      },
      {
        "Label": "Punctuality",
        "Level": "n/a"
      }
    ],
    "Experience": [
      {
        "Title": "Software Engineer",
        "Company": "Tech Solutions Ltd.",
        "Location": "Warsaw, Poland",
        "StartDate": "2020-01-01",
        "EndDate": "2023-01-01",
        "Present": true,
        "ListOfContentPoints": [
          "Developed and maintained web applications using React.js, C#, and SQL Server.",
          "Collaborated with cross-functional teams to design and implement new features.",
          "Optimized application performance and ensured code quality through unit testing and code reviews."
        ]
      },
      {
        "Title": "Junior Developer",
        "Company": "Web Innovations Inc.",
        "Location": "Warsaw, Poland",
        "StartDate": "2018-01-01",
        "EndDate": "2019-12-31",
        "Present": false,
        "ListOfContentPoints": [
          "Assisted in the development of web applications using HTML, CSS, and JavaScript.",
          "Contributed to the design and implementation of user interfaces."
        ]
      }
    ],
    "Education": [
      {
        "Degree": "Bachelor of Science in Computer Science",
        "Institution": "Warsaw University of Technology",
        "Location": "Warsaw, Poland",
        "StartDate": "2014-09-01",
        "EndDate": "2018-06-30",
        "Present": false
      }
    ],
    "Projects": [
      {
        "Title": "E-commerce Platform",
        "ListOfContentPoints": [
          "Developed a full-stack e-commerce platform using React.js and Node.js",
          "Integrating payment gateways and user authentication."
        ],
        "Technologies": [ "React.js", "Node.js", "MongoDB" ],
        "Link": ""
      },
      {
        "Title": "Portfolio Website",
        "ListOfContentPoints": [
          "Create portfolio to show my skills in the front end technologies"
        ],
        "Technologies": [],
        "Link": "https://portfolio.com"
      }
    ],
    "Certifications": [
      {
        "Title": "Certified React Developer",
        "Issuer": "React Training Institute",
        "Date": "2021-05-01",
        "Description": "Certification in React.js development, covering advanced concepts and best practices.",
        "Link": "https://reacttraininginstitute.com/certifications/react-developer"
      },
      {
        "Title": "Certified C# Developer",
        "Issuer": "C# Training Institute",
        "Date": "2025-02-03",
        "Description": "",
        "Link": ""
      }
    ],
    "Languages": [
      {
        "Language": "English",
        "Proficiency": "Fluent"
      },
      {
        "Language": "Polish",
        "Proficiency": "Native"
      },
      {
        "Language": "Germany",
        "Proficiency": "n/a"
      }
    ],
    "OtherInformation": [
      {
        "Label": "Hobbies",
        "Title": "Interests",
        "ListOfContentPoints": [
          "Read the book",
          "Play the guitar"
        ]
      },
      {
        "Label": "Hobbies",
        "Title": "",
        "ListOfContentPoints": [
          "Read the book"
        ]
      }
    ]
  }
}

```

