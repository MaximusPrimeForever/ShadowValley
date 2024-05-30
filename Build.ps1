# Define paths
$projectPath = "."
$outputDllPath = "$projectPath\bin\Release\net8.0\DynamicShadows.dll"
$destinationPath = "C:\Program Files (x86)\Steam\steamapps\common\Stardew Valley\Mods\ShadowValley\DynamicShadows.dll"

# Compile the project
Write-Output "Cleaning before building..."
Remove-Item -Path "$projectPath\bin" -Recurse -Force
Remove-Item -Path "$projectPath\obj\Release" -Recurse -Force

Write-Output "Building the project..."
dotnet build $projectPath --configuration Release --force /property:WarningLevel=0

# Check if the build was successful
if ($?) {
    Write-Output "Build succeeded. Copying the DLL..."
    
    # Copy the DLL to the destination path
    Copy-Item -Path $outputDllPath -Destination $destinationPath -Force
    
    # Check if the copy operation was successful
    if ($?) {
        Write-Output "DLL successfully copied to the mods directory."
    } else {
        Write-Error "Failed to copy the DLL."
    }
} else {
    Write-Error "Build failed."
}