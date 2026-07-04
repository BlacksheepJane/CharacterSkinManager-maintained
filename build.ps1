param(
    [string]$GameDir = $env:STS2_GAME_DIR,
    [switch]$Install
)

$ErrorActionPreference = "Stop"
$RepoDir = Split-Path -Parent $MyInvocation.MyCommand.Path

if ([string]::IsNullOrWhiteSpace($GameDir)) {
    $GameDir = Resolve-Path -LiteralPath (Join-Path $RepoDir "..")
}

$GameDir = (Resolve-Path -LiteralPath $GameDir).Path
$env:STS2_GAME_DIR = $GameDir

dotnet build (Join-Path $RepoDir "CharacterSkinManager.csproj") -c Release

if ($Install) {
    $target = Join-Path $GameDir "mods\CharacterSkinManager"
    New-Item -ItemType Directory -Path $target -Force | Out-Null
    Copy-Item -LiteralPath (Join-Path $RepoDir "bin\Release\net9.0\CharacterSkinManager.dll") -Destination $target -Force
    Copy-Item -LiteralPath (Join-Path $RepoDir "manifest\CharacterSkinManager.json") -Destination $target -Force
    Write-Host "Installed to $target"
}
