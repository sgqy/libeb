# Microsoft Developer Studio Project File - Name="zlib" - Package Owner=<4>
# Microsoft Developer Studio Generated Build File, Format Version 6.00
# ** DO NOT EDIT **

# TARGTYPE "Win32 (x86) Static Library" 0x0104

CFG=zlib - Win32 Debug
!MESSAGE This is not a valid makefile. To build this project using NMAKE,
!MESSAGE use the Export Makefile command and run
!MESSAGE 
!MESSAGE NMAKE /f "zlib.mak".
!MESSAGE 
!MESSAGE You can specify a configuration when running NMAKE
!MESSAGE by defining the macro CFG on the command line. For example:
!MESSAGE 
!MESSAGE NMAKE /f "zlib.mak" CFG="zlib - Win32 Debug"
!MESSAGE 
!MESSAGE Possible choices for configuration are:
!MESSAGE 
!MESSAGE "zlib - Win32 Release" (based on "Win32 (x86) Static Library")
!MESSAGE "zlib - Win32 Debug" (based on "Win32 (x86) Static Library")
!MESSAGE 

# Begin Project
# PROP AllowPerConfigDependencies 0
# PROP Scc_ProjName ""
# PROP Scc_LocalPath ""
CPP=cl.exe
MTL=midl.exe
RSC=rc.exe

!IF  "$(CFG)" == "zlib - Win32 Release"

# PROP BASE Use_MFC 0
# PROP BASE Use_Debug_Libraries 0
# PROP BASE Output_Dir "Release"
# PROP BASE Intermediate_Dir "Release"
# PROP BASE Target_Dir ""
# PROP Use_MFC 0
# PROP Use_Debug_Libraries 0
# PROP Output_Dir "Release"
# PROP Intermediate_Dir "Release\zlib"
# PROP Target_Dir ""
# ADD BASE CPP /nologo /W3 /Gm /GX /O2 /D "_LIB" /D "WIN32" /D "_MBCS" /YX /FD /c /D "NDEBUG"
# ADD CPP /nologo /W3 /Gm /GX /O2 /D "_LIB" /D "WIN32" /D "_MBCS" /YX /FD /c /I ".\zlib" /I ".\portsrc" /I "..\eb-4.1.1\zlib" /FI "zconfig.h"   /MD /D "NDEBUG"
# ADD BASE RSC /l 0x411 /D "NDEBUG"
# ADD RSC /l 0x411 /D "NDEBUG"
BSC32=bscmake.exe
# ADD BASE BSC32 /nologo
# ADD BSC32 /nologo
LIB32=link.exe -lib
# ADD BASE LIB32 /nologo
# ADD LIB32 /nologo

!ELSEIF  "$(CFG)" == "zlib - Win32 Debug"

# PROP BASE Use_MFC 0
# PROP BASE Use_Debug_Libraries 0
# PROP BASE Output_Dir "Debug"
# PROP BASE Intermediate_Dir "Debug"
# PROP BASE Target_Dir ""
# PROP BASE Use_MFC 0
# PROP Use_Debug_Libraries 0
# PROP Output_Dir "Debug"
# PROP Intermediate_Dir "Debug\zlib"
# PROP Target_Dir ""
# ADD BASE CPP /nologo /W3 /Gm /GX /ZI /Od /D "_LIB" /D "WIN32" /D "_MBCS" /YX /FD /GZ /c /D "_DEBUG"
# ADD CPP /nologo /W3 /Gm /GX /ZI /Od /D "_LIB" /D "WIN32" /D "_MBCS" /YX /FD /GZ /c /I ".\zlib" /I ".\portsrc" /I "..\eb-4.1.1\zlib" /FI "zconfig.h"   /MDd /D "_DEBUG"
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

# Name "zlib - Win32 Release"
# Name "zlib - Win32 Debug"
# Begin Group "Source Files"

# PROP Default_Filter "cpp;c;cxx;rc;def;r;odl;idl;hpj;bat"
# Begin Source File

SOURCE=..\eb-4.1.1\zlib\adler32.c
# End Source File
# Begin Source File

SOURCE=..\eb-4.1.1\zlib\compress.c
# End Source File
# Begin Source File

SOURCE=..\eb-4.1.1\zlib\crc32.c
# End Source File
# Begin Source File

SOURCE=..\eb-4.1.1\zlib\gzio.c
# End Source File
# Begin Source File

SOURCE=..\eb-4.1.1\zlib\uncompr.c
# End Source File
# Begin Source File

SOURCE=..\eb-4.1.1\zlib\deflate.c
# End Source File
# Begin Source File

SOURCE=..\eb-4.1.1\zlib\trees.c
# End Source File
# Begin Source File

SOURCE=..\eb-4.1.1\zlib\zutil.c
# End Source File
# Begin Source File

SOURCE=..\eb-4.1.1\zlib\inflate.c
# End Source File
# Begin Source File

SOURCE=..\eb-4.1.1\zlib\infblock.c
# End Source File
# Begin Source File

SOURCE=..\eb-4.1.1\zlib\inftrees.c
# End Source File
# Begin Source File

SOURCE=..\eb-4.1.1\zlib\infcodes.c
# End Source File
# Begin Source File

SOURCE=..\eb-4.1.1\zlib\infutil.c
# End Source File
# Begin Source File

SOURCE=..\eb-4.1.1\zlib\inffast.c
# End Source File
# End Group
# Begin Group "Header Files"

# PROP Default_Filter "h;hpp;hxx;hm;inl"
# End Group
# End Target
# End Project
