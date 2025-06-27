@echo off
:: Navigate to the directory where this script is located
cd /d "%~dp0"

:: Execute the PowerShell command
PowerShell -Command "Get-ChildItem -Path . -Recurse -Filter '*.cs' | Where-Object { $_.Name -notlike '*.g.cs' -and $_.FullName -notmatch '\\obj\\' -and $_.FullName -notmatch '\\test(s?)\\' -and $_.Name -notlike '*.razor' -and $_.Name -notlike 'AssemblyInfo.cs' } | ForEach-Object { $relativePath = $_.FullName.Substring($(Get-Location).Path.Length + 1); \"`n// File: $relativePath`n\" + (Get-Content $_.FullName) + \"`n-----------------`n\" } | Out-File AllClasses.txt"

echo All C# files (excluding .g.cs, .razor, AssemblyInfo.cs, obj folders, and test folders) have been combined into AllClasses.txt with relative paths.
pause
