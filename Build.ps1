# Define paths
$projectPath = "."
$manifestPath = "$projectPath\manifest.json"
$assetsPath = "$projectPath\assets"
$outputDllPath = "$projectPath\bin\Release\net8.0\DynamicShadows.dll"
$destinationPath = "C:\Program Files (x86)\Steam\steamapps\common\Stardew Valley\Mods\ShadowValley\"

# Compile the project
Write-Output "Cleaning before building..."
Remove-Item -Path "$projectPath\bin" -Recurse -Force
Remove-Item -Path "$projectPath\obj\Release" -Recurse -Force

Write-Output "Building the project..."
dotnet build $projectPath --configuration Release --force /property:WarningLevel=0

# Check if the build was successful
if ($?) {
    Write-Output "Build succeeded. Copying the DLL..."
    
    # Build the mod directory in Stardew Valley directory
    New-Item -ItemType Directory -Path $destinationPath -Force
    Copy-Item $outputDllPath -Destination $destinationPath -Force
    Copy-Item $manifestPath -Destination $destinationPath -Force
    Copy-Item $assetsPath -Destination $destinationPath -Recurse -Force
    
    # Check if the copy operation was successful
    if ($?) {
        Write-Output "DLL successfully copied to the mods directory."
    } else {
        Write-Error "Failed to copy the DLL."
    }
} else {
    Write-Error "Build failed."
}