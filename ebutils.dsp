# Microsoft Developer Studio Project File - Name="ebutils" - Package Owner=<4>
# Microsoft Developer Studio Generated Build File, Format Version 6.00
# ** DO NOT EDIT **

# TARGTYPE "Win32 (x86) Static Library" 0x0104

CFG=ebutils - Win32 Debug
!MESSAGE This is not a valid makefile. To build this project using NMAKE,
!MESSAGE use the Export Makefile command and run
!MESSAGE 
!MESSAGE NMAKE /f "ebutils.mak".
!MESSAGE 
!MESSAGE You can specify a configuration when running NMAKE
!MESSAGE by defining the macro CFG on the command line. For example:
!MESSAGE 
!MESSAGE NMAKE /f "ebutils.mak" CFG="ebutils - Win32 Debug"
!MESSAGE 
!MESSAGE Possible choices for configuration are:
!MESSAGE 
!MESSAGE "ebutils - Win32 Release" (based on "Win32 (x86) Static Library")
!MESSAGE "ebutils - Win32 Debug" (based on "Win32 (x86) Static Library")
!MESSAGE 

# Begin Project
# PROP AllowPerConfigDependencies 0
# PROP Scc_ProjName ""
# PROP Scc_LocalPath ""
CPP=cl.exe
MTL=midl.exe
RSC=rc.exe

!IF  "$(CFG)" == "ebutils - Win32 Release"

# PROP BASE Use_MFC 0
# PROP BASE Use_Debug_Libraries 0
# PROP BASE Output_Dir "Release"
# PROP BASE Intermediate_Dir "Release"
# PROP BASE Target_Dir ""
# PROP Use_MFC 0
# PROP Use_Debug_Libraries 0
# PROP Output_Dir "Release"
# PROP Intermediate_Dir "Release\ebutils"
# PROP Target_Dir ""
# ADD BASE CPP /nologo /W3 /Gm /GX /O2 /D "_LIB" /D "WIN32" /D "_MBCS" /YX /FD /c /D "NDEBUG"
# ADD CPP /nologo /W3 /Gm /GX /O2 /D "_LIB" /D "WIN32" /D "_MBCS" /YX /FD /c /I ".\portsrc" /I ".\intl" /I ".\iconv" /I "..\eb-4.1.1"   /I "..\eb-4.1.1\libebutils" /D "HAVE_CONFIG_H" /MD /D "NDEBUG"
# ADD BASE RSC /l 0x411 /D "NDEBUG"
# ADD RSC /l 0x411 /D "NDEBUG"
BSC32=bscmake.exe
# ADD BASE BSC32 /nologo
# ADD BSC32 /nologo
LIB32=link.exe -lib
# ADD BASE LIB32 /nologo
# ADD LIB32 /nologo

!ELSEIF  "$(CFG)" == "ebutils - Win32 Debug"

# PROP BASE Use_MFC 0
# PROP BASE Use_Debug_Libraries 0
# PROP BASE Output_Dir "Debug"
# PROP BASE Intermediate_Dir "Debug"
# PROP BASE Target_Dir ""
# PROP BASE Use_MFC 0
# PROP Use_Debug_Libraries 0
# PROP Output_Dir "Debug"
# PROP Intermediate_Dir "Debug\ebutils"
# PROP Target_Dir ""
# ADD BASE CPP /nologo /W3 /Gm /GX /ZI /Od /D "_LIB" /D "WIN32" /D "_MBCS" /YX /FD /GZ /c /D "_DEBUG"
# ADD CPP /nologo /W3 /Gm /GX /ZI /Od /D "_LIB" /D "WIN32" /D "_MBCS" /YX /FD /GZ /c /I ".\portsrc" /I ".\intl" /I ".\iconv" /I "..\eb-4.1.1"   /I "..\eb-4.1.1\libebutils" /D "HAVE_CONFIG_H" /MDd /D "_DEBUG"
# ADD BASE RSC /l 0x411 /D "_DEBUG"
# ADD RSC /l 0x411 /D "_DEBUG"
BSC32=bscmake.exe
# ADD BASE BSC32 /nologo
# ADD BSC32 /nologo
LIB32=link.exe -lib
# ADD BASE LIB32 /nologo
# ADD LIB32 /nologo

!ENDIF 

# Begin Target

# Name "ebutils - Win32 Release"
# Name "ebutils - Win32 Debug"
# Begin Group "Source Files"

# PROP Default_Filter "cpp;c;cxx;rc;def;r;odl;idl;hpj;bat"
# Begin Source File

SOURCE=..\eb-4.1.1\libebutils\ebutils.c
# End Source File
# Begin Source File

SOURCE=..\eb-4.1.1\libebutils\getopt.c
# End Source File
# Begin Source File

SOURCE=..\eb-4.1.1\libebutils\getumask.c
# End Source File
# Begin Source File

SOURCE=..\eb-4.1.1\libebutils\makedir.c
# End Source File
# Begin Source File

SOURCE=..\eb-4.1.1\libebutils\puts_eucjp.c
# End Source File
# Begin Source File

SOURCE=..\eb-4.1.1\libebutils\samefile.c
# End Source File
# Begin Source File

SOURCE=..\eb-4.1.1\libebutils\strcasecmp.c
# End Source File
# Begin Source File

SOURCE=..\eb-4.1.1\libebutils\yesno.c
# End Source File
# End Group
# Begin Group "Header Files"

# PROP Default_Filter "h;hpp;hxx;hm;inl"
# End Group
# End Target
# End Project
