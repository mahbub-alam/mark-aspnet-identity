REM Written by: MAB
@echo off

echo Target-Files : %1.* (.dll, .xml, .pdb)
echo Destination  : %2..\build
echo Build-Config : %3

xcopy /Y %1.dll %2..\build

if exist %1.xml (
	xcopy /Y %1.xml %2..\build
)

if exist %1.pdb (
	xcopy /Y %1.pdb %2..\build
)
