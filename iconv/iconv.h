/*                                                            -*- C -*-
 * Copyright (c) 2003
 *    Motoyuki Kasahara
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2, or (at your option)
 * any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 */

#ifndef ICONV_H
#define ICONV_H

#include <stddef.h>

typedef int iconv_t;

iconv_t iconv_open(const char *, const char *);
iconv_t iconv_close(iconv_t);
size_t iconv(iconv_t, const char **, size_t *, char **, size_t *);

#endif /* ICONV_H */
