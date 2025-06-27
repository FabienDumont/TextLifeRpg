@echo off
cd /d "%~dp0"

powershell -NoProfile -Command "Get-ChildItem -Path '.' -Recurse -Filter '*.cs' | Where-Object { $_.Name -notlike '*.g.cs' -and $_.FullName -notmatch '\\obj\\' -and $_.FullName -notmatch '\\test(s?)\\' -and $_.Name -notlike '*.razor' -and $_.Name -notlike 'AssemblyInfo.cs' } | ForEach-Object { '// File: ' + ($_.FullName.Substring($pwd.Path.Length + 1)) } | Out-File -FilePath 'AllFilesPaths.txt' -Encoding UTF8"

echo Done. Check AllFilesPaths.txt
pause
