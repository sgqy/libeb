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

#include <stddef.h>
#include <string.h>
#include <errno.h>
#include <windows.h>
#include "iconv.h"

#ifndef EILSEQ
#define EILSEQ ENOENT
#endif

/*
 * Simple iconv() implementation which supports conversion from EUC-JP
 * to Shift_JIS only.
 */

#define ICONV_EUCJP_TO_SJIS	0

iconv_t
iconv_open(const char *tocode, const char *fromcode)
{
    if (_stricmp(fromcode, "eucJP") != 0
	&& _stricmp(fromcode, "EUC-JP") != 0) {
	SetLastError(EINVAL);
	return -1;
    }

    if (_stricmp(tocode, "CP932") != 0
	&& _stricmp(tocode, "SJIS") != 0
	&& _stricmp(tocode, "Shift_JIS") != 0) {
	SetLastError(EINVAL);
	return -1;
    }

    return ICONV_EUCJP_TO_SJIS;
}

iconv_t
iconv_close(iconv_t cd)
{
    return 0;
}

size_t
iconv(iconv_t cd, const char **inbuf, size_t *inbytesleft,
    char **outbuf, size_t *outbytesleft)
{
    int c1, c2, c3;
    int jisx0212 = 0;

    if (cd != ICONV_EUCJP_TO_SJIS) {
	SetLastError(EINVAL);
	return -1;
    }

    /*
     * Set `cd' to the initial state.
     * If `outbuf' or `*outbuf' is not NULL, iconv() must also output
     * a corresponding shift sequence, but there is no shift sequence
     * for ICONV_EUCJP_TO_SJIS.
     */
    if (inbuf == NULL || *inbuf == NULL)
	return 0;

    for (;;) {
	if (*inbytesleft <= 0)
	    break;
	if (*outbytesleft <= 0) {
	    SetLastError(E2BIG);
	    return -1;
	}

	c1 = *(const unsigned char *)*inbuf;
	if (c1 < 0x80) {
	    /*
	     * ASCII.
	     */
	    *(unsigned char *)*outbuf = c1;
	    (*inbuf)++;
	    (*inbytesleft)--;
	    (*outbuf)++;
	    (*outbytesleft)--;

	} else if (c1 == 0x8e) {
	    /*
	     * JIS X 0201 katakana
	     */
	    if (*inbytesleft <= 1) {
		SetLastError(EINVAL);
		return -1;
	    }
	    c2 = *(const unsigned char *)(*inbuf + 1);
	    if (c2 < 0xa0 || 0xdf < c2) {
		SetLastError(EILSEQ);
		return -1;
	    }

	    *(unsigned char *)*outbuf = c2;
	    (*inbuf) += 2;
	    (*inbytesleft) -= 2;
	    (*outbuf)++;
	    (*outbytesleft)--;

	} else if (c1 == 0x8f) {
	    /* 
	     * JIS X 0212 kanji.
	     */
	    if (*inbytesleft <= 2) {
		SetLastError(EINVAL);
		return -1;
	    }

	    c2 = *(const unsigned char *)(*inbuf + 1);
	    c3 = *(const unsigned char *)(*inbuf + 2);
	    if (c2 < 0xa1 || 0xfe < c2 || c3 < 0xa1 || 0xfe < c3) {
		SetLastError(EILSEQ);
		return -1;
	    }

	    *(unsigned char *)*outbuf = ' ';
	    (*inbuf) += 3;
	    (*inbytesleft) -= 3;
	    (*outbuf)++;
	    (*outbytesleft)--;
	    jisx0212++;

	} else if (0xa0 < c1 && c1 < 0xff) {
	    /*
	     * JIS X 0208 kanji.
	     */
	    if (*inbytesleft <= 1) {
		SetLastError(EINVAL);
		return -1;
	    }
	    if (*outbytesleft <= 1) {
		SetLastError(E2BIG);
		return -1;
	    }

	    c2 = *(const unsigned char *)(*inbuf + 1);
	    if (c2 < 0xa1 || 0xfe < c2) {
		SetLastError(EILSEQ);
		return -1;
	    }

	    if (c1 < 0xdf)
		*(unsigned char *)*outbuf = 0x81 + ((c1 - 0xa1) >> 1);
	    else
		*(unsigned char *)*outbuf = 0xe0 + ((c1 - 0xdf) >> 1);

	    if ((c1 & 0x01) == 0)
		*(unsigned char *)(*outbuf + 1) = c2 - 0x02;
	    else if (c2 < 0xe0)
		*(unsigned char *)(*outbuf + 1) = c2 - 0x61;
	    else
		*(unsigned char *)(*outbuf + 1) = c2 - 0x60;

	    (*inbuf) += 2;
	    (*inbytesleft) -= 2;
	    (*outbuf) += 2;
	    (*outbytesleft) -= 2;

	} else {
	    SetLastError(EILSEQ);
	    return -1;
	}
    }

    return jisx0212;
}

#ifdef TEST
#include <stdio.h>
#include <stdlib.h>

int
main()
{
    iconv_t cd;
    char inbuf[512], outbuf[512];
    char *inbufp, *outbufp;
    size_t inlen, outlen;
    int lineno = 0;

    cd = iconv_open("SJIS", "EUC-JP");
    if (cd == -1) {
	fprintf(stderr, "failed to iconv_open() [errno=%s]\n",
	    strerror(errno));
	exit(1);
    }
    while (fgets(inbuf, sizeof(inbuf), stdin) != NULL) {
	lineno++;
	inbufp = inbuf;
	inlen = strlen(inbuf) + 1;
	outbufp = outbuf;
	outlen = sizeof(outbuf);
	if (iconv(cd, &inbufp, &inlen, &outbufp, &outlen) == -1) {
	    fprintf(stderr, "failed to iconv() [lineno=%d, errno=%s]\n",
		lineno, strerror(errno));
	    exit(1);
	}
	fputs(outbuf, stdout);
    }

    iconv_close(cd);
    return 0;
}

#endif /* TEST */
