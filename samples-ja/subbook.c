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
 * �ե�����̾:
 *     subbook.c
 *
 * ������ˡ:
 *     subbook book-path
 *
 * ��:
 *     subbook /cdrom
 *
 * ����:
 *     ���Υץ����ϰ���� CD-ROM ���Ҥ˴ޤޤ�Ƥ������ܤ���̾��ɽ
 *     �����ޤ���`book-path' �Ͻ��ҤΥȥåץǥ��쥯�ȥꡢ�Ĥޤ�
 *     CATALOG �ޤ��� CATALOGS �ե������¸�ߤ���ǥ��쥯�ȥ��ؤ���
 *     ���ˤ��ޤ���
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

int
main(argc, argv)
    int argc;
    char *argv[];
{
    EB_Book book;
    EB_Subbook_Code sublist[EB_MAX_SUBBOOKS];
    int subcount;
    const char *title;
    int i;

    /*
     * ���ޥ�ɹ԰���������å���
     */
    if (argc != 2) {
	fprintf(stderr, "Usage: %s book-path\n", argv[0]);
	exit(1);
    }

    /*
     * `book' ��������
     */
    eb_initialize(&book);

    /*
     * ���Ҥ� `book' �˷���դ��롣
     */
    if (eb_bind(&book, argv[1]) == -1) {
	fprintf(stderr, "%s: failed to bind the book, %s: %s\n",
	    argv[0], eb_error_message(), argv[1]);
	exit(1);
    }

    /*
     * ���ܤΰ����������
     */
    subcount = eb_subbook_list(&book, sublist);
    if (subcount < 0) {
	fprintf(stderr, "%s: failed to get the subbbook list, %s\n",
	    argv[0], eb_error_message());
	eb_clear(&book);
	exit(1);
    }

    /*
     * ���Ҥ˴ޤޤ�Ƥ������ܤ���̾����ϡ�
     */
    for (i = 0; i < subcount; i++) {
	title = eb_subbook_title2(&book, sublist[i]);
	if (title == NULL) {
	    fprintf(stderr, "%s: failed to get the title, %s:\n",
		argv[0], eb_error_message());
	    continue;
	}
	printf("%d: %s\n", i, title);
    }

    /*
     * ���Ҥ����Ѥ�λ��
     */
    eb_clear(&book);

    exit(0);
}
