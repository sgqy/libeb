/*
 * Copyright (c) 1997, 1998  Motoyuki Kasahara
 *
 * This programs is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2, or (at your option)
 * any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 */

/*
 * Requirements for Autoconf:
 *   AC_C_CONST
 *   AC_TYPE_SIZE_T
 */

#ifdef HAVE_CONFIG_H
#include "config.h"
#else

/* Define to empty if the keyword `const' does not work.  */
/* #define const */

/* Define if `size_t' is not defined.  */
/* #define size_t unsigned */

#endif /* not HAVE_CONFIG_H */


/*
 * Character comparison table used in strcasecmp() and strncasecmp().
 */
static const unsigned char cmp_table[] = {
    /* 0x00 -- 0x0f */
    '\000', '\001', '\002', '\003', '\004', '\005', '\006', '\007',
    '\010', '\011', '\012', '\013', '\014', '\015', '\016', '\017',

    /* 0x10 -- 0x1f */
    '\020', '\021', '\022', '\023', '\024', '\025', '\026', '\027',
    '\030', '\031', '\032', '\033', '\034', '\035', '\036', '\037',

    /* 0x20 -- 0x2f */
    '\040', '\041', '\042', '\043', '\044', '\045', '\046', '\047',
    '\050', '\051', '\052', '\053', '\054', '\055', '\056', '\057',

    /* 0x30 -- 0x3f */
    '\060', '\061', '\062', '\063', '\064', '\065', '\066', '\067',
    '\070', '\071', '\072', '\073', '\074', '\075', '\076', '\077',

    /* 0x40 -- 0x4f */
    '\100', '\141', '\142', '\143', '\144', '\145', '\146', '\147',
    '\150', '\151', '\152', '\153', '\154', '\155', '\156', '\157',

    /* 0x50 -- 0x5f */
    '\160', '\161', '\162', '\163', '\164', '\165', '\166', '\167',
    '\170', '\171', '\172', '\133', '\134', '\135', '\136', '\137',

    /* 0x60 -- 0x6f */
    '\140', '\141', '\142', '\143', '\144', '\145', '\146', '\147',
    '\150', '\151', '\152', '\153', '\154', '\155', '\156', '\157',

    /* 0x70 -- 0x7f */
    '\160', '\161', '\162', '\163', '\164', '\165', '\166', '\167',
    '\170', '\171', '\172', '\173', '\174', '\175', '\176', '\177',

    /* 0x80 -- 0x8f */
    '\200', '\201', '\202', '\203', '\204', '\205', '\206', '\207',
    '\210', '\211', '\212', '\213', '\214', '\215', '\216', '\217',

    /* 0x90 -- 0x9f */
    '\220', '\221', '\222', '\223', '\224', '\225', '\226', '\227',
    '\230', '\231', '\232', '\233', '\234', '\235', '\236', '\237',

    /* 0xa0 -- 0xaf */
    '\240', '\241', '\242', '\243', '\244', '\245', '\246', '\247',
    '\250', '\251', '\252', '\253', '\254', '\255', '\256', '\257',

    /* 0xb0 -- 0xbf */
    '\260', '\261', '\262', '\263', '\264', '\265', '\266', '\267',
    '\270', '\271', '\272', '\273', '\274', '\275', '\276', '\277',

    /* 0xc0 -- 0xcf */
    '\300', '\301', '\302', '\303', '\304', '\305', '\306', '\307',
    '\310', '\311', '\312', '\313', '\314', '\315', '\316', '\317',

    /* 0xd0 -- 0xdf */
    '\320', '\321', '\322', '\323', '\324', '\325', '\326', '\327',
    '\330', '\331', '\332', '\333', '\334', '\335', '\336', '\337',

    /* 0xe0 -- 0xef */
    '\340', '\341', '\342', '\343', '\344', '\345', '\346', '\347',
    '\350', '\351', '\352', '\353', '\354', '\355', '\356', '\357',

    /* 0xf0 -- 0xff */
    '\360', '\361', '\362', '\363', '\364', '\365', '\366', '\367',
    '\370', '\371', '\372', '\373', '\374', '\375', '\376', '\377',
};


/*
 * Compare strings.
 * Cases in the strings are insensitive.
 */
int
strcasecmp(s1, s2)
    const char *s1;
    const char *s2;
{
    const unsigned char *us1 = (const unsigned char *)s1;
    const unsigned char *us2 = (const unsigned char *)s2;
    int cmp;

    while (*us1 != '\0') {
	cmp = (int)(cmp_table[(int)(*us1++)] - cmp_table[(int)(*us2++)]);
	if (cmp != 0)
	    return cmp;
    }

    return -(int)(cmp_table[(int)(*us2)]);
}


/*
 * Compare strings within `n' characters.
 * Cases in the strings are insensitive.
 */
int
strncasecmp(s1, s2, n)
    const char *s1;
    const char *s2;
    size_t n;
{
    const unsigned char *us1 = (const unsigned char *)s1;
    const unsigned char *us2 = (const unsigned char *)s2;
    size_t i = n;
    int cmp;

    while (*us1 != '\0') {
	if (i-- <= 0)
	    return 0;
	cmp = (int)(cmp_table[(int)(*us1++)] - cmp_table[(int)(*us2++)]);
	if (cmp != 0)
	    return cmp;
    }

    return -(int)(cmp_table[(int)(*s2)]);
}


