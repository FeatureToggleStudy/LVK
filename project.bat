set PROJECT=LVK

set /A year=1%date:~6,4%-10000
set /A month=1%date:~3,2%-100
set /A day=1%date:~0,2%-100
set /A tm=%time:~0,2%%time:~3,2%
set VERSION=%year%.%month%.%day%.%tm%

set GITBRANCH=
for /f %%f in ('git rev-parse --abbrev-ref HEAD') do set GITBRANCH=%%f

if "%GITBRANCH%" == "master" (
    set SUFFIX=
    set CONFIGURATION=Release
    echo Building RELEASE build %VERSION%
    exit /B 0
)

echo Building BETA build %VERSION%
set SUFFIX=-beta
set CONFIGURATION=Debug
