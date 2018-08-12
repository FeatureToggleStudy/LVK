@echo off

setlocal enableextensions enabledelayedexpansion

for /D %%f in (*.*) do (
    if exist %%f\bin rd /s /q %%f\bin
    if exist %%f\obj rd /s /q %%f\obj
)

dotnet build -c Release
if errorlevel 1 goto end

for /R %%f in (*.nupkg) do call :fixup %%f
goto end

:fixup

set fname1=%1
set fname2=%fname1:symbols=%

rem echo (%fname1%) == (%fname2%)
if "%fname1%" == "%fname2%" nuget push "%1" -Source nuget.org
goto :end

:end