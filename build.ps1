
$buildFolder = "build/$(Get-Date -Format "yyyyMMdd_HHmm")"
dotnet publish "./HomeStorage.API/HomeStorage.API.csproj" -o "$(Join-Path "./" $buildFolder)"
Set-Location "frontend-vite"
npm run build
Write-Output "Copying files to wwwroot"
Copy-Item "./dist/*" -Destination "$(Join-Path "../" "$(Join-Path $buildFolder "wwwroot")")"

Read-Host -Prompt "Press Enter to exit"
