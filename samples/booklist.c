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

/*
 * ������ˡ:
 *     booklist <remote-access-ideintifier>
 * ��:
 *     booklist ebnet://localhost
 * ����:
 *     <remote-access-ideintifier> �ǻ��ꤷ�� EBNET �����Ф���³��
 *     �ơ������Ф��󶡤�����ҡ�appendix �ΰ�����ɽ�����ޤ���
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
#include <eb/booklist.h>

int
main(argc, argv)
    int argc;
    char *argv[];
{
    EB_Error_Code error_code;
    EB_BookList bl;
    int book_count;
    char *name, *title;
    int i;

    /* ���ޥ�ɹ԰���������å���*/
    if (argc != 2) {
        fprintf(stderr, "Usage: %s book-path remote-access-identifier\n",
            argv[0]);
        exit(1);
    }

    /* EB �饤�֥��� `bl' ��������*/
    eb_initialize_library();
    eb_initialize_booklist(&bl);

    /* EBNET �����Ф� `bl' �˷���դ��롣*/
    error_code = eb_bind_booklist(&bl, argv[1]);
    if (error_code != EB_SUCCESS) {
        fprintf(stderr, "%s: failed to bind the EBNET server, %s: %s\n",
            argv[0], eb_error_message(error_code), argv[1]);
        goto die;
    }

    /* �����о�ν��ҡ�appendix �θĿ��������*/
    error_code = eb_booklist_book_count(&bl, &book_count);
    if (error_code != EB_SUCCESS) {
        fprintf(stderr, "%s: failed to get the number of books, %s\n",
            argv[0], eb_error_message(error_code));
        goto die;
    }

    for (i = 0; i < book_count; i++) {
        /* ���ҡ�appendix ��̾�Τ������*/
        error_code = eb_booklist_book_name(&bl, i, &name);
	if (error_code != EB_SUCCESS) {
            fprintf(stderr, "%s: failed to get book name #%d, %s\n",
                argv[0], i, eb_error_message(error_code));
            goto die;
        }

            }
        /* ���ҡ�appendix ����̾�������*/
        error_code = eb_booklist_book_name(&bl, i, &title);
	if (error_code != EB_SUCCESS) {
            fprintf(stderr, "%s: failed to get book title #%d, %s\n",
                argv[0], i, eb_error_message(error_code));
            goto die;
        }

        printf("%-20s  %s\n", name, title);
    }
        
    /* `bl' �� EB �饤�֥������Ѥ�λ��*/
    eb_finalize_book(&bl);
    eb_finalize_library();
    exit(0);

    /* ���顼ȯ���ǽ�λ����Ȥ��ν�����*/
  die:
    eb_finalize_book(&bl);
    eb_finalize_library();
    exit(1);
}
