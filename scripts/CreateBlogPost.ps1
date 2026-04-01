Param()

Add-Type -AssemblyName Microsoft.VisualBasic

function Read-Input([string]$prompt, [string]$title = "Create blog post") {
    return [Microsoft.VisualBasic.Interaction]::InputBox($prompt, $title, "")
}

function Ensure-Dir([string]$path) {
    if (-not (Test-Path $path)) {
        New-Item -ItemType Directory -Path $path -Force | Out-Null
    }
}

# START
$solutionDir = (Resolve-Path .).Path
if ($env:SolutionDir) {
    $solutionDir = $env:SolutionDir
}

# Ask for folder name
$name = Read-Input "Enter the blog folder name (example: 2026-02-16-my-post)" "Create blog post"
if ([string]::IsNullOrWhiteSpace($name)) {
    Write-Host "No name provided. Aborting."
    exit 1
}

# Normalize name (remove leading/trailing slashes or backslashes)
$name = $name.Trim() -replace '^[\\/]+|[\\/]+$',''

# Paths
$imagesDir = Join-Path $solutionDir "src/ViveCortelazor/wwwroot/images/blog/$name"
$contentDir = Join-Path $solutionDir "src/ViveCortelazor/Content/Blog/$name"

# Create directories
Ensure-Dir $imagesDir
Ensure-Dir $contentDir

# Default image size values (strings to match example)
$defaultImageWidth = "800"
$defaultImageHeight = "350"

# JSON templates (ordered and with the exact property names requested)
$dataEsJson = @"
{
  "Title": "",
  "Language": "es",
  "Slug": "$name",
  "Description": "",
  "Keywords": "",
  "Image": "/images/blog/$name/",
  "ImageWidth": "$defaultImageWidth",
  "ImageHeight": "$defaultImageHeight",
  "Robots": "all"
}
"@

$dataEnJson = @"
{
  "Title": "",
  "Language": "en",
  "Slug": "$name",
  "Description": "",
  "Keywords": "",
  "Image": "/images/blog/$name/",
  "ImageWidth": "$defaultImageWidth",
  "ImageHeight": "$defaultImageHeight",
  "Robots": "all"
}
"@

# Files to create
$files = @(
    @{ Path = Join-Path $contentDir "data.es.json"; Content = $dataEsJson },
    @{ Path = Join-Path $contentDir "data.en.json"; Content = $dataEnJson },
    @{ Path = Join-Path $contentDir "text.es.md"; Content = "---`n# Write the Spanish article content below (Markdown)`n---`n" },
    @{ Path = Join-Path $contentDir "text.en.md"; Content = "---`n# Write the English article content below (Markdown)`n---`n" }
)

foreach ($f in $files) {
    if (Test-Path $f.Path) {
        Write-Host "Skipping existing file: $($f.Path)"
    } else {
        $f.Content | Out-File -FilePath $f.Path -Encoding UTF8
        Write-Host "Created: $($f.Path)"
    }
}

# Create a placeholder image README for the images folder
$imgReadme = Join-Path $imagesDir "README.md"
if (-not (Test-Path $imgReadme)) {
    "# Place images for the post here" | Out-File -FilePath $imgReadme -Encoding UTF8
}

# Open the content folder in Explorer
Start-Process explorer.exe $contentDir

Write-Host "Done. Created blog folders for '$name'."