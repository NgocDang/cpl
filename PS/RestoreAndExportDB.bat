@echo off
SET dir=%~dp0
SET "RestoreDB=RestoreDB.ps1"
SET "ExportDB=ExportDB.ps1"
SET "RestoreDBPath=%dir%%RestoreDB%"
SET "ExportDBPath=%dir%%ExportDB%"
Powershell.exe -executionpolicy remotesigned -File  %RestoreDBPath%
Powershell.exe -executionpolicy remotesigned -File  %ExportDBPath%
pause