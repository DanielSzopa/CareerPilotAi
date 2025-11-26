`I would like to create tech-stack.md file which will contain all informations about used technologies, packages, systems, databases, servers, everything what can be useful for understanding the project, creating new features, fixing bugs and maintaining the project.

In order to create this file, you need to execute firstly a few commands to collect all context and crucial informations.

Please follow these steps:

# Step 1
Find solution directory.
Run command:
```pwsh
Get-ChildItem -Recurse -Filter "*.sln" | Select-Object -ExpandProperty Directory | Select-Object -ExpandProperty FullName
```

If these commands return more than one result, please ask user to select the correct one.

# Step 2
Navigate to solution directory.

# Step 3
Execute command:
```pwsh
tree /f /a
```
This command will return a tree of the solution directory with all files and directories.
Please select files which can store crucial informations about used technologies, packages, systems, databases, servers, everything what can be useful for understanding the project, creating new features, fixing bugs and maintaining the project.
My suggestion:
- Docs
- Documentation files
- .csproj files
- .json files
- .md files
- Readme files

Read content of these selected files.

# Step 4
For sln file execute command:
```pwsh
dotnet list package
```

# Step 5
Execute pwsh `Analyze-UsingStatements.ps1` script, but remember to adjust the sln directory path.

Output of pwsh script, should return all used usings which can show you what is used, what is the most popular, based on that you can select some files and read them to understand more context of used packages or technology.

<output_structure>
{
    "Usings": [
        {
            "Name": "System.Console",
            "UsageQuantity": 1,
            "FilePaths": [
                "C:\\Users\\Daniel\\source\\repos\\CareerPilotAi\\CareerPilotAi\\Controllers\\AuthController.cs",
            ]
        }
    ],
    "GlobalUsings": [
        {
            "Name": "System.Console",
            "UsageQuantity": 1,
            "FilePath": "C:\\Users\\Daniel\\source\\repos\\CareerPilotAi\\CareerPilotAi\\GlobalUsingsSample.cs"
        }
    ]
}
</output_structure>

# Step 6

It will return a list of packages used in the solution.

Based on collected informations from above steps please create tech-stack.md file.