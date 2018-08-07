@echo off
SET dir=%~dp0
SET "fileName=RestoreDB.ps1"
SET "filePath=%dir%%fileName%"
Powershell.exe -executionpolicy remotesigned -File  %filePath%
pause