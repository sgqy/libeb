/*
 * Copyright (c) 1999  Motoyuki Kasahara
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

/*
 * $B%U%!%$%kL>(B:
 *     word.c
 *
 * $B;HMQJ}K!(B:
 *     word book-path word subbook-index
 *
 * $BNc(B:
 *     word /cdrom apple 0
 *
 * $B@bL@(B:
 *     $B$3$N%W%m%0%i%`$O0l:}$N(B CD-ROM $B=q@R$N$"$kI{K\$NCf$+$i(B `word'
 *     $B$rC5$7=P$7$^$9!#%R%C%H$7$?$9$Y$F$N%(%s%H%j$N8+=P$7$rI8=`=PNO(B
 *     $B$X=PNO$7$^$9!#(B`book-path' $B$O=q@R$N%H%C%W%G%#%l%/%H%j!"$D$^$j(B
 *     CATALOG $B$^$?$O(B CATALOGS $B%U%!%$%k$NB8:_$9$k%G%#%l%/%H%j$r;X$9(B
 *     $B$h$&$K$7$^$9!#(B
 *     `subbook-index' $B$O8!:wBP>]$NI{K\$N%$%s%G%C%/%9$G$9!#=q@R$NCf(B
 *     $B$N:G=i$NI{K\$N%$%s%G%C%/%9$,(B `0'$B!"<!$,(B `1' $B$K$J$j$^$9!#(B
 */

#ifdef HAVE_CONFIG_H
#include "config.h"
#endif

#include <stdio.h>

#ifdef HAVE_STDLIB_H
#include <stdlib.h>
#endif

#include <eb/eb.h>
#include <eb/error.h>
#include <eb/text.h>

#define MAX_HITS 50
#define MAXLEN_HEADING 127

int
main(argc, argv)
    int argc;
    char *argv[];
{
    EB_Book book;
    EB_Subbook_Code sublist[EB_MAX_SUBBOOKS];
    EB_Hit hits[MAX_HITS];
    char heading[MAXLEN_HEADING + 1];
    int subcount;
    int subindex;
    int hitcount;
    size_t len;
    int i;

    /*
     * $B%3%^%s%I9T0z?t$r%A%'%C%/!#(B
     */
    if (argc != 4) {
        fprintf(stderr, "Usage: %s book-path subbook-index word\n",
            argv[0]);
        exit(1);
    }

    /*
     * `book' $B$r=i4|2=!#(B
     */
    eb_initialize(&book);

    /*
     * $B=q@R$r(B `book' $B$K7k$SIU$1$k!#(B
     */
    if (eb_bind(&book, argv[1]) == -1) {
        fprintf(stderr, "%s: failed to bind the book, %s: %s\n",
            argv[0], eb_error_message(), argv[1]);
        goto failed;
    }

    /*
     * $BI{K\$N0lMw$r<hF@!#(B
     */
    subcount = eb_subbook_list(&book, sublist);
    if (subcount < 0) {
        fprintf(stderr, "%s: failed to get the subbbook list, %s\n",
            argv[0], eb_error_message());
        goto failed;
    }

    /*
     * $BI{K\$N%$%s%G%C%/%9$r<hF@!#(B
     */
    subindex = atoi(argv[2]);
    if (subindex < 0 || subindex >= subcount) {
        fprintf(stderr, "%s: invalid subbbook-index: %s\n",
            argv[0], argv[2]);
        goto failed;
    }

    /*
     * $B!V8=:_$NI{K\(B (current subbook)$B!W$r@_Dj!#(B
     */
    if (eb_set_subbook(&book, sublist[subindex]) < 0) {
        fprintf(stderr, "%s: failed to set the current subbook, %s\n",
            argv[0], eb_error_message());
        goto failed;
    }

    /*
     * $BC18l8!:w$N%j%/%(%9%H$rAw=P!#(B
     */
    if (eb_search_word(&book, argv[3]) < 0) {
        fprintf(stderr, "%s: failed to search for the word, %s: %s\n",
            argv[0], eb_error_message(), argv[3]);
        goto failed;
    }

    for (;;) {
        /*
         * $B;D$C$F$$$k%R%C%H%(%s%H%j$r<hF@!#(B
         */
        hitcount = eb_hit_list(&book, hits, MAX_HITS);
        if (hitcount == 0) {
            break;
        } else if (hitcount < 0) {
            fprintf(stderr, "%s: failed to get hit entries, %s\n",
                argv[0], eb_error_message());
            goto failed;
        }

        for (i = 0; i < hitcount; i++) {
            /*
             * $B8+=P$7$N0LCV$X0\F0!#(B
             */
            if (eb_seek(&book, &(hits[i].heading)) < 0) {
                fprintf(stderr, "%s: failed to seek the subbook, %s\n",
                    argv[0], eb_error_message());
                goto failed;
            }

            /*
             * $B8+=P$7$r<hF@!#(B
             */
            len = eb_heading(&book, NULL, NULL, heading, MAXLEN_HEADING);
            if (len < 0) {
                fprintf(stderr, "%s: failed to read the subbook, %s\n",
                    argv[0], eb_error_message());
                goto failed;
            }

            /*
             * $B8+=P$7$r=PNO!#(B
             */
            printf("%s\n", heading);
        }
    }
        
    /*
     * $B=q@R$NMxMQ$r=*N;!#(B
     */
    eb_clear(&book);

    exit(0);

    /*
     * $B%(%i!<$,H/@8!#(B
     */
  failed:
    eb_clear(&book);
    exit(1);
}
