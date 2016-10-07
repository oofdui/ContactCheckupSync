@ECHO OFF
REM ######################################################
SET pathDefault="\\10.121.103.29\share$\@MyProject\"
SET projectName="ContactCheckupSync"
SET projectVersion="v.1.28"
REM ######################################################

ECHO ######## CopyTo@MyProject '%projectName%' ########
ECHO.

SET pathDestination=%pathDefault%\%projectName%\%projectVersion%
ECHO #### The Destination Path is %pathDestination% ####
ECHO.

ECHO #### Copying %projectName%\bin\Debug\ To %pathDestination% ####
ECHO.
xcopy %projectName%\bin\Debug\*.* %pathDestination% /D /Y /I
SET pathREADME=%pathDefault%\%projectName%\
xcopy README.md %pathREADME% /d /y

ECHO.
ECHO #### Delete Exclude File %pathDestination%\*.vshost.exe.* ####
ECHO.
DEL %pathDestination%\*.vshost.exe.*
DEL %pathDestination%\*.pdb

ECHO #### Copy Completed ####
ECHO.
TIMEOUT /T 15