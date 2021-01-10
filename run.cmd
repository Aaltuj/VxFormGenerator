@echo off

CHOICE /C BP /N /M "Select framework: [B] Bootstrap, [P] Plain"
IF %ERRORLEVEL% EQU 1 goto:bootstrap
IF %ERRORLEVEL% EQU 2 goto plain

:bootstrap
set config=--configuration Bootstrap
goto:run

:plain
set config=
goto:run

:run
CHOICE /C SW /N /M "Select architecture: [S] Server, [W] Webassembly"
IF %ERRORLEVEL% EQU 1 goto:server
IF %ERRORLEVEL% EQU 2 goto webassembly

:server
dotnet watch --project .\VxFormGeneratorDemo.Server\VxFormGeneratorDemo.Server.csproj run %config%

:webassembly
dotnet watch --project .\VxFormGeneratorDemo.Wasm\VxFormGeneratorDemo.Wasm.csproj run %config%

