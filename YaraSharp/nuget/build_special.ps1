param (
    $SourceDirectory = "$PSScriptRoot/..",
    $msbuild = 'msbuild',
    $nuget = 'nuget'
)

$projectName = (Split-Path $SourceDirectory -Leaf)
$ErrorActionPreference = 'Stop'

& $msbuild /m "$SourceDirectory/$projectName.sln" /p:Configuration=Release /p:Platform="x86" '/t:Clean;Rebuild'
if (-not $?) {
    throw "msbuild returned error code: $LASTEXITCODE"
}
& $msbuild /m "$SourceDirectory/$projectName.sln" /p:Configuration=Release /p:Platform="x64" '/t:Clean;Rebuild'
if (-not $?) {
    throw "msbuild returned error code: $LASTEXITCODE"
}

(Get-Content "targets").replace('PROJECTNAME', $projectName) | Set-Content "$SourceDirectory/Build/Release/$projectName.targets"

& $nuget pack -Exclude **\*.pdb -BasePath "$SourceDirectory/Build" "$PSScriptRoot/$projectName.nuspec"

Write-Host -NoNewLine 'Press any key to continue...';
$null = $Host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown');