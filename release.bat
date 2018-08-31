@echo off

setlocal

for /f "tokens=*" %%i in ('where git.exe') do set GIT_CONSOLE=%%i
if "%GIT_CONSOLE%" == "" goto NO_GIT
if "%SIGNINGKEYS%" == "" goto setup

rem set RELEASE_KEY=USE_RELEASE_KEY

rem copy "%SIGNINGKEYS%\Lasse V. Karlsen Private.snk" "%PROJECT%\Lasse V. Karlsen.snk"

call project.bat
call build.bat
if errorlevel 1 goto error

rem "%GIT_CONSOLE%" checkout "%PROJECT%\Lasse V. Karlsen.snk"
rem if errorlevel 1 goto error

echo=
echo================================================
set /P PUSHYESNO=Push packages to nuget? [y/N]
if "%PUSHYESNO%" == "Y" GOTO PUSH
if "%PUSHYESNO%" == "y" GOTO PUSH
exit /B 0

:PUSH
for /R %%f in (*.nupkg) do call :push1 %%f
"%GIT_CONSOLE%" tag version/%VERSION%%SUFFIX%
if errorlevel 1 goto error
start "" "https://www.nuget.org/account/Packages"
exit /B 0

:PUSH1
set fname1=%1
set fname2=%fname1:symbols=%

rem echo (%fname1%) == (%fname2%)
if "%fname1%" == "%fname2%" nuget push "%1" -Source nuget.org
goto end

:NO_GIT
echo Unable to locate "git.exe", is it on the path?
goto error

:error
goto exitwitherror

:setup
echo Requires SIGNINGKEYS environment variable to be set
goto exitwitherror

:exitwitherror
rem "%GIT_CONSOLE%" checkout "%PROJECT%\Lasse V. Karlsen.snk"
exit /B 1

:end