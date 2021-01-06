@echo off

CHOICE /C BP /N /M "Select [B] Bootstrap, [P] Plain"
IF %ERRORLEVEL% EQU 1 goto:bootstrap
IF %ERRORLEVEL% EQU 2 goto plain

:bootstrap
set config=--configuration Bootstrap
goto:run

:plain
set config=
goto:run

:run
dotnet watch --project .\Demo\FormGeneratorDemo.csproj run %config%

