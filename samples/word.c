/*                                                            -*- C -*-
 * Copyright (c) 1999, 2000, 01
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

/*
 * 使用方法:
 *     word <book-path> <subbook-index> <word>
 * 例:
 *     word /cdrom 0 apple
 * 説明:
 *     <book-path> で指定した CD-ROM 書籍の特定の副本の中から <word>
 *     という単語を完全一致検索で探し、ヒットしたすべてのエントリの
 *     見出しを表示します。
 *
 *     <subbook-index> には、検索対象の副本のインデックスを指定しま
 *     す。インデックスは、書籍の最初の副本から順に 0、1、2 ... に
 *     なります。
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
    EB_Error_Code error_code;
    EB_Book book;
    EB_Subbook_Code subbook_list[EB_MAX_SUBBOOKS];
    EB_Hit hits[MAX_HITS];
    char heading[MAXLEN_HEADING + 1];
    int subbook_count;
    int subbook_index;
    int hit_count;
    size_t heading_length;
    int i;

    /* コマンド行引数をチェック。*/
    if (argc != 4) {
        fprintf(stderr, "Usage: %s book-path subbook-index word\n",
            argv[0]);
        exit(1);
    }

    /* EB ライブラリと `book' を初期化。*/
    eb_initialize_library();
    eb_initialize_book(&book);

    /* 書籍を `book' に結び付ける。*/
    error_code = eb_bind(&book, argv[1]);
    if (error_code != EB_SUCCESS) {
        fprintf(stderr, "%s: failed to bind the book, %s: %s\n",
            argv[0], eb_error_message(error_code), argv[1]);
        goto die;
    }

    /* 副本の一覧を取得。*/
    error_code = eb_subbook_list(&book, subbook_list, &subbook_count);
    if (error_code != EB_SUCCESS) {
        fprintf(stderr, "%s: failed to get the subbbook list, %s\n",
            argv[0], eb_error_message(error_code));
        goto die;
    }

    /* 副本のインデックスを取得。*/
    subbook_index = atoi(argv[2]);

    /*「現在の副本 (current subbook)」を設定。*/
    error_code = eb_set_subbook(&book, subbook_list[subbook_index]);
    if (error_code != EB_SUCCESS) {
        fprintf(stderr, "%s: failed to set the current subbook, %s\n",
            argv[0], eb_error_message(error_code));
        goto die;
    }

    /* 単語検索のリクエストを送出。*/
    error_code = eb_search_exactword(&book, argv[3]);
    if (error_code != EB_SUCCESS) {
        fprintf(stderr, "%s: failed to search for the word, %s: %s\n",
            argv[0], eb_error_message(error_code), argv[3]);
        goto die;
    }

    for (;;) {
        /* 残っているヒットエントリを取得。*/
        error_code = eb_hit_list(&book, MAX_HITS, hits, &hit_count);
	if (error_code != EB_SUCCESS) {
            fprintf(stderr, "%s: failed to get hit entries, %s\n",
                argv[0], eb_error_message(error_code));
            goto die;
        }
        if (hit_count == 0)
            break;

        for (i = 0; i < hit_count; i++) {
            /* 見出しの位置へ移動。*/
	    error_code = eb_seek_text(&book, &(hits[i].heading));
            if (error_code != EB_SUCCESS) {
                fprintf(stderr, "%s: failed to seek the subbook, %s\n",
                    argv[0], eb_error_message(error_code));
                goto die;
            }

            /* 見出しを取得して表示。*/
            error_code = eb_read_heading(&book, NULL, NULL, NULL,
		MAXLEN_HEADING, heading, &heading_length);
            if (error_code != EB_SUCCESS) {
                fprintf(stderr, "%s: failed to read the subbook, %s\n",
                    argv[0], eb_error_message(error_code));
                goto die;
            }
            printf("%s\n", heading);
        }
    }
        
    /* 書籍と EB ライブラリの利用を終了。*/
    eb_finalize_book(&book);
    eb_finalize_library();
    exit(0);

    /* エラー発生で終了するときの処理。*/
  die:
    eb_finalize_book(&book);
    eb_finalize_library();
    exit(1);
}
