@echo off

setlocal

call project.bat

if exist *.nupkg del *.nupkg
if errorlevel 1 goto error

for /d %%f in (*.*) do (
    if exist "%%f\bin" rd /s /q "%%f\bin"
    if errorlevel 1 goto error
    if exist "%%f\obj" rd /s /q "%%f\obj"
    if errorlevel 1 goto error
)
if errorlevel 1 goto error

dotnet restore
if errorlevel 1 goto error

msbuild "%PROJECT%.sln" /target:Clean,Rebuild /p:Configuration=%CONFIGURATION% /p:Version=%VERSION%%SUFFIX% /p:AssemblyVersion=%VERSION% /p:FileVersion=%VERSION% /p:DefineConstants="TRACE;%CONFIGURATION%;%RELEASE_KEY%;NETSTANDARD2_0;JETBRAINS_ANNOTATIONS" /verbosity:minimal
if errorlevel 1 goto error

for /D %%f in (*.*) do call :test %%f
if errorlevel 1 goto error
goto end

:test
set fname1=%1
set fname2=%fname1:.Tests=%

if "%fname1%"=="%fname2%" goto end
if "%fname1%"=="LVK.Tests.Framework" goto end
dotnet test %1 --no-build --no-restore
goto end

:error
exit /B 1

:end
