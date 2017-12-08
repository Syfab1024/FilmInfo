@echo off

setlocal

set BaseFile=H:\Eigene Dateien\dev\FilmInfo\FilmInfo

rem --------------------------------- DatumsInfos --------------------------------- 
set jahr=%date:~-4%
set monat=%date:~-7,2%
set tag=%date:~-10,2%

rem --------------------------------- ZeitInfos --------------------------------- 
set stunde=%time:~0,2%
set minute=%time:~3,2%
set sekunde=%time:~6,2%

rem --------------------------------- alles zusammensetzen --------------------------------- 
set zeit=%stunde%%minute%%sekunde%
set heute=%jahr%%monat%%tag%
set DestFile=%BaseFile%_%heute%_%zeit%

rem --------------------------------- und komprimieren --------------------------------- 
7z a -r "%DestFile%" -x@"backup_exclude.txt"  *.*


echo Name der Backup-Datei:
echo %DestFile%
pause>nul