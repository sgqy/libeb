# Microsoft Developer Studio Project File - Name="eb" - Package Owner=<4>
# Microsoft Developer Studio Generated Build File, Format Version 6.00
# ** DO NOT EDIT **

# TARGTYPE "Win32 (x86) Dynamic-Link Library" 0x0102

CFG=eb - Win32 Debug
!MESSAGE This is not a valid makefile. To build this project using NMAKE,
!MESSAGE use the Export Makefile command and run
!MESSAGE 
!MESSAGE NMAKE /f "eb.mak".
!MESSAGE 
!MESSAGE You can specify a configuration when running NMAKE
!MESSAGE by defining the macro CFG on the command line. For example:
!MESSAGE 
!MESSAGE NMAKE /f "eb.mak" CFG="eb - Win32 Debug"
!MESSAGE 
!MESSAGE Possible choices for configuration are:
!MESSAGE 
!MESSAGE "eb - Win32 Release" (based on "Win32 (x86) Dynamic-Link Library")
!MESSAGE "eb - Win32 Debug" (based on "Win32 (x86) Dynamic-Link Library")
!MESSAGE 

# Begin Project
# PROP AllowPerConfigDependencies 0
# PROP Scc_ProjName ""
# PROP Scc_LocalPath ""
CPP=cl.exe
MTL=midl.exe
RSC=rc.exe

!IF  "$(CFG)" == "eb - Win32 Release"

# PROP BASE Use_MFC 0
# PROP BASE Use_Debug_Libraries 0
# PROP BASE Output_Dir "Release"
# PROP BASE Intermediate_Dir "Release"
# PROP BASE Target_Dir ""
# PROP Use_MFC 0
# PROP Use_Debug_Libraries 0
# PROP Output_Dir "Release"
# PROP Intermediate_Dir "Release\eb"
# PROP Target_Dir ""
# ADD BASE CPP /nologo /W3 /Gm /GX /O2 /D "WIN32" /D "_MBCS" /YX /FD /c /D "_EB" /D "EB_EXPORTS" /D "NDEBUG"
# ADD CPP /nologo /W3 /Gm /GX /O2 /D "WIN32" /D "_MBCS" /YX /FD /c /I ".\portsrc" /I ".\portsrc\eb" /I ".\intl"   /I "..\eb-4.0\zlib" /D "EB_BUILD_LIBRARY"   /D "HAVE_CONFIG_H" /MD /D "_EB" /D "EB_EXPORTS" /D "NDEBUG"
# ADD BASE MTL /nologo /mktyplib203 /win32 /D "NDEBUG"
# ADD MTL /nologo /mktyplib203 /win32 /D "NDEBUG"
# ADD BASE RSC /l 0x411 /D "NDEBUG"
# ADD RSC /l 0x411 /D "NDEBUG"
BSC32=bscmake.exe
# ADD BASE BSC32 /nologo
# ADD BSC32 /nologo
LIB32=link.exe -lib
# ADD BASE LINK32 kernel32.lib user32.lib gdi32.lib winspool.lib comdlg32.lib advapi32.lib   shell32.lib ole32.lib oleaut32.lib uuid.lib odbc32.lib odbccp32.lib   /nologo /dll /subsystem:console /machine:I386 /pdbtype:sept
# ADD LINK32 kernel32.lib user32.lib gdi32.lib winspool.lib comdlg32.lib advapi32.lib   shell32.lib ole32.lib oleaut32.lib uuid.lib odbc32.lib odbccp32.lib   /nologo /dll /subsystem:console /machine:I386 /pdbtype:sept Release/zlib.lib ws2_32.lib

!ELSEIF  "$(CFG)" == "eb - Win32 Debug"

# PROP BASE Use_MFC 0
# PROP BASE Use_Debug_Libraries 0
# PROP BASE Output_Dir "Debug"
# PROP BASE Intermediate_Dir "Debug"
# PROP BASE Target_Dir ""
# PROP BASE Use_MFC 0
# PROP Use_Debug_Libraries 0
# PROP Output_Dir "Debug"
# PROP Intermediate_Dir "Debug\eb"
# PROP Target_Dir ""
# ADD BASE CPP /nologo /W3 /Gm /GX /ZI /Od /D "WIN32" /D "_MBCS" /YX /FD /GZ /c /D "_EB" /D "EB_EXPORTS" /D "_DEBUG"
# ADD CPP /nologo /W3 /Gm /GX /ZI /Od /D "WIN32" /D "_MBCS" /YX /FD /GZ /c /I ".\portsrc" /I ".\portsrc\eb" /I ".\intl"   /I "..\eb-4.0\zlib" /D "EB_BUILD_LIBRARY"   /D "HAVE_CONFIG_H" /MDd /D "_EB" /D "EB_EXPORTS" /D "_DEBUG"
# ADD BASE MTL /nologo /mktyplib203 /win32 /D "_DEBUG"
# ADD MTL /nologo /mktyplib203 /win32 /D "_DEBUG"
# ADD BASE RSC /l 0x411 /D "_DEBUG"
# ADD RSC /l 0x411 /D "_DEBUG"
BSC32=bscmake.exe
# ADD BASE BSC32 /nologo
# ADD BSC32 /nologo
LIB32=link.exe -lib
# ADD BASE LINK32 kernel32.lib user32.lib gdi32.lib winspool.lib comdlg32.lib advapi32.lib   shell32.lib ole32.lib oleaut32.lib uuid.lib odbc32.lib odbccp32.lib   /nologo /dll /subsystem:console /incremental:yes /debug /machine:I386 /pdbtype:sept /debug
# ADD LINK32 kernel32.lib user32.lib gdi32.lib winspool.lib comdlg32.lib advapi32.lib   shell32.lib ole32.lib oleaut32.lib uuid.lib odbc32.lib odbccp32.lib   /nologo /dll /subsystem:console /incremental:yes /debug /machine:I386 /pdbtype:sept Debug/zlib.lib ws2_32.lib /debug

!ENDIF 

# Begin Target

# Name "eb - Win32 Release"
# Name "eb - Win32 Debug"
# Begin Group "Source Files"

# PROP Default_Filter "cpp;c;cxx;rc;def;r;odl;idl;hpj;bat"
# Begin Source File

SOURCE=..\eb-4.0\eb\appendix.c
# End Source File
# Begin Source File

SOURCE=..\eb-4.0\eb\appsub.c
# End Source File
# Begin Source File

SOURCE=..\eb-4.0\eb\bcd.c
# End Source File
# Begin Source File

SOURCE=..\eb-4.0\eb\binary.c
# End Source File
# Begin Source File

SOURCE=..\eb-4.0\eb\bitmap.c
# End Source File
# Begin Source File

SOURCE=..\eb-4.0\eb\book.c
# End Source File
# Begin Source File

SOURCE=..\eb-4.0\eb\booklist.c
# End Source File
# Begin Source File

SOURCE=..\eb-4.0\eb\copyright.c
# End Source File
# Begin Source File

SOURCE=..\eb-4.0\eb\eb.c
# End Source File
# Begin Source File

SOURCE=..\eb-4.0\eb\endword.c
# End Source File
# Begin Source File

SOURCE=..\eb-4.0\eb\error.c
# End Source File
# Begin Source File

SOURCE=..\eb-4.0\eb\exactword.c
# End Source File
# Begin Source File

SOURCE=..\eb-4.0\eb\filename.c
# End Source File
# Begin Source File

SOURCE=..\eb-4.0\eb\font.c
# End Source File
# Begin Source File

SOURCE=..\eb-4.0\eb\hook.c
# End Source File
# Begin Source File

SOURCE=..\eb-4.0\eb\jacode.c
# End Source File
# Begin Source File

SOURCE=..\eb-4.0\eb\keyword.c
# End Source File
# Begin Source File

SOURCE=..\eb-4.0\eb\lock.c
# End Source File
# Begin Source File

SOURCE=..\eb-4.0\eb\log.c
# End Source File
# Begin Source File

SOURCE=..\eb-4.0\eb\match.c
# End Source File
# Begin Source File

SOURCE=..\eb-4.0\eb\menu.c
# End Source File
# Begin Source File

SOURCE=..\eb-4.0\eb\memmove.c
# End Source File
# Begin Source File

SOURCE=..\eb-4.0\eb\multi.c
# End Source File
# Begin Source File

SOURCE=..\eb-4.0\eb\narwalt.c
# End Source File
# Begin Source File

SOURCE=..\eb-4.0\eb\narwfont.c
# End Source File
# Begin Source File

SOURCE=..\eb-4.0\eb\readtext.c
# End Source File
# Begin Source File

SOURCE=..\eb-4.0\eb\search.c
# End Source File
# Begin Source File

SOURCE=..\eb-4.0\eb\setword.c
# End Source File
# Begin Source File

SOURCE=..\eb-4.0\eb\stopcode.c
# End Source File
# Begin Source File

SOURCE=..\eb-4.0\eb\strcasecmp.c
# End Source File
# Begin Source File

SOURCE=..\eb-4.0\eb\subbook.c
# End Source File
# Begin Source File

SOURCE=..\eb-4.0\eb\text.c
# End Source File
# Begin Source File

SOURCE=..\eb-4.0\eb\word.c
# End Source File
# Begin Source File

SOURCE=..\eb-4.0\eb\zio.c
# End Source File
# Begin Source File

SOURCE=..\eb-4.0\eb\ebnet.c
# End Source File
# Begin Source File

SOURCE=..\eb-4.0\eb\multiplex.c
# End Source File
# Begin Source File

SOURCE=..\eb-4.0\eb\linebuf.c
# End Source File
# Begin Source File

SOURCE=..\eb-4.0\eb\urlparts.c
# End Source File
# Begin Source File

SOURCE=..\eb-4.0\eb\getaddrinfo.c
# End Source File
# Begin Source File

SOURCE=..\eb-4.0\eb\dummyin6.c
# End Source File
# Begin Source File

SOURCE=..\eb-4.0\eb\widealt.c
# End Source File
# Begin Source File

SOURCE=..\eb-4.0\eb\widefont.c
# End Source File
# Begin Source File

SOURCE=.\portsrc\dirent.c
# End Source File
# Begin Source File

SOURCE=.\portsrc\eb.def
# End Source File
# Begin Source File

SOURCE=.\portsrc\localedir.c
# End Source File
# Begin Source File

SOURCE=.\portsrc\eb.rc
# End Source File
# End Group
# Begin Group "Header Files"

# PROP Default_Filter "h;hpp;hxx;hm;inl"
# End Group
# End Target
# End Project
