/*
 * dirent.h
 * $Id: dirent.h 3.2 2002/03/14 05:18:27 satomii Exp $
 *
 * implements unix-like (not compatible) functions required to compile
 * eb library on win32 environment without gcc.
 * contact <satomi@ring.gr.jp> for problems or comments.
 *
 * this program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2, or (at your option)
 * any later version.
 *
 * this program is distributed in the hope that it will be useful, but
 * WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
 * General Public License for more details.
 */
#ifndef	WIN32
#error This file is intended to be used with a Win32 compiler.
#endif	/* !WIN32 */

#ifndef	__DIRENT_H__
#define __DIRENT_H__

#include <windows.h>

#pragma pack(1)

struct __win32_dirent_emu {
    unsigned short int d_reclen;	/* length of this record. */
    //unsigned char d_type;			/* N/A */
    char d_name[_MAX_PATH];			/* directory name. */
};

typedef struct __win32_dirstream_emu {
	HANDLE handle;
	WIN32_FIND_DATA wfd;
	struct __win32_dirent_emu entry;
	char pattern[1];
} DIR;

#pragma pack()

#define dirent		__win32_dirent_emu

DIR *__win32_opendir(const char *);
int __win32_closedir(DIR *);
struct dirent *__win32_readdir(DIR *);

#define opendir		__win32_opendir
#define closedir	__win32_closedir
#define readdir		__win32_readdir

#endif	/* __DIRENT_H__  */
