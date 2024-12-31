
$buildFolder = "build/$(Get-Date -Format "yyyyMMdd_HHmm")";
dotnet publish "./HomeStorage.API/HomeStorage.API.csproj" -o "$(Join-Path "./" $buildFolder)";
Set-Location "frontend-vite"
npm run build --outDir "$(Join-Path "../" "$(Join-Path $buildFolder "wwwroot")")" --cleanOutDir;

Read-Host -Prompt "Press Enter to exit"
