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
Execute pwsh script, but remember to adjust the sln path.
```pwsh
# Script to collect and count all using statements in C# files and output as JSON

param(
    [string]$SolutionPath = "C:\Users\Daniel\source\repos\CareerPilotAi"
)

# Get all .cs files recursively
$csFiles = Get-ChildItem -Path $SolutionPath -Filter "*.cs" -Recurse -ErrorAction SilentlyContinue

if (-not $csFiles) {
    Write-Error "No .cs files found in $SolutionPath"
    exit
}

Write-Host "Found $($csFiles.Count) .cs files. Scanning for using statements..." -ForegroundColor Cyan

# Hash tables to store using and global using
$usingDetails = @{}  # For regular usings: {name: [filePaths]}
$globalUsingDetails = @{}  # For global usings: {name: count}

foreach ($file in $csFiles) {
    try {
        $content = Get-Content -Path $file.FullName -ErrorAction Stop
        
        # Find all using statements (including global using and using static)
        foreach ($line in $content) {
            # Match: using [global] [static] namespace [= namespace];
            if ($line -match '^\s*using\s+(.+?);?\s*$') {
                $statement = $Matches[1].Trim()
                
                # Check if it's a global using
                if ($statement -match '^global\s+') {
                    # Remove 'global' prefix and get the actual namespace
                    $usingName = $statement -replace '^global\s+', ''
                    
                    if (-not $globalUsingDetails.ContainsKey($usingName)) {
                        $globalUsingDetails[$usingName] = 0
                    }
                    $globalUsingDetails[$usingName]++
                }
                else {
                    # Regular using
                    if (-not $usingDetails.ContainsKey($statement)) {
                        $usingDetails[$statement] = @()
                    }
                    $usingDetails[$statement] += $file.FullName
                }
            }
        }
    }
    catch {
        Write-Warning "Error reading file $($file.FullName): $_"
    }
}

# Build JSON output
$usings = @()
foreach ($usingName in ($usingDetails.Keys | Sort-Object)) {
    $filePaths = $usingDetails[$usingName] | Select-Object -Unique
    $usings += @{
        Name = $usingName
        UsageQuantity = $filePaths.Count
        FilePaths = @($filePaths)
    }
}

# Sort by usage quantity descending
$usings = $usings | Sort-Object -Property UsageQuantity -Descending

$globalUsings = @()
foreach ($globalUsingName in ($globalUsingDetails.Keys | Sort-Object)) {
    $globalUsings += @{
        Name = $globalUsingName
        UsageQuantity = $globalUsingDetails[$globalUsingName]
    }
}

# Sort by usage quantity descending
$globalUsings = $globalUsings | Sort-Object -Property UsageQuantity -Descending

# Create final JSON object
$jsonOutput = @{
    Usings = $usings
    GlobalUsings = $globalUsings
}

# Convert to JSON and display
$jsonString = $jsonOutput | ConvertTo-Json -Depth 10
Write-Host "`n" -ForegroundColor Green
Write-Host $jsonString -ForegroundColor Green

```

Output of pwsh script, should return all used usings which can show you what is used, what is the most popular, based on that you can select some files and read them to understand more context of used packages or technology.

# Step 6

It will return a list of packages used in the solution.

Based on collected informations from above steps please create tech-stack.md file.