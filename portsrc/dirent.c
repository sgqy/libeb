/*
 * dirent.c
 * $Id: dirent.c 3.2 2002/03/14 05:18:27 satomii Exp $
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
#ifndef WIN32
#error This file is intended to be used with a Win32 compiler.
#endif	/* !WIN32 */

#include "dirent.h"


DIR *__win32_opendir(const char *path)
{
	DIR *dir;
	DWORD len, attr;

	/*
	 *	check the source path length.
	 */
	len = lstrlen(path);
	if (!len) {
		SetLastError(ERROR_BAD_PATHNAME);
		return(NULL);
	}
	if (3 < len && '\\' == *(path + len - 1)) len--;

	/*
	 * allocate DIR buffer - here do not use malloc() or any other
	 * runtime functions, so that both we and the caller do not have to
	 * care about the thread model.
	 */
	dir = (DIR *)LocalAlloc(LPTR, sizeof(DIR) + len + 5);
	if (!dir) return(NULL);

	/*
	 * copy the path to the work buffer and check if it points to a
	 * directory.
	 */
	lstrcpy(dir->pattern, path);
	*(dir->pattern + len) = '\0';

	attr = GetFileAttributes(dir->pattern);
	if ((DWORD)-1 == attr || !(attr & FILE_ATTRIBUTE_DIRECTORY)) {
		/* not a valid directory. */
		LocalFree(dir);
		return(NULL);
	}

	if ('\\' != *(dir->pattern + len - 1)) {
		*(dir->pattern + len) = '\\';
		len++;
	}
	lstrcpy(dir->pattern + len, "*.*");
	dir->entry.d_reclen = sizeof(dir->entry);
	return(dir);
}

struct dirent *__win32_readdir(DIR *dir)
{
	if (!dir) return(NULL);

	if (!(dir->handle)) {
		dir->handle = FindFirstFile(dir->pattern, &(dir->wfd));
		if (INVALID_HANDLE_VALUE == dir->handle) return(NULL);

		while (!lstrcmp(".", dir->wfd.cFileName)
				|| !lstrcmp("..", dir->wfd.cFileName)) {
			if (!FindNextFile(dir->handle, &(dir->wfd)))
				return(NULL);
		}

	} else {
		if (!FindNextFile(dir->handle, &(dir->wfd))) return(NULL);
	}

	lstrcpy(dir->entry.d_name, dir->wfd.cFileName);
	return(&(dir->entry));
}

int __win32_closedir(DIR *dir)
{
	if (!dir) return(-1);
	if (dir->handle) FindClose(dir->handle);
	return(LocalFree(dir) ? -1 : 0);
}
