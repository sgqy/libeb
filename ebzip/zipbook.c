/*                                                            -*- C -*-
 * Copyright (c) 1998, 99, 2000, 01  
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

#ifdef HAVE_CONFIG_H
#include "config.h"
#endif

#include <stdio.h>
#include <sys/types.h>

#if defined(STDC_HEADERS) || defined(HAVE_STRING_H)
#include <string.h>
#if !defined(STDC_HEADERS) && defined(HAVE_MEMORY_H)
#include <memory.h>
#endif /* not STDC_HEADERS and HAVE_MEMORY_H */
#else /* not STDC_HEADERS and not HAVE_STRING_H */
#include <strings.h>
#endif /* not STDC_HEADERS and not HAVE_STRING_H */

#ifdef HAVE_STDLIB_H
#include <stdlib.h>
#endif

#ifdef HAVE_UNISTD_H
#include <unistd.h>
#endif

#ifdef HAVE_LIMITS_H
#include <limits.h>
#endif

#ifdef ENABLE_NLS
#ifdef HAVE_LOCALE_H
#include <locale.h>
#endif
#include <libintl.h>
#endif

/*
 * The maximum length of path name.
 */
#ifndef PATH_MAX
#ifdef MAXPATHLEN
#define PATH_MAX        MAXPATHLEN
#else /* not MAXPATHLEN */
#define PATH_MAX        1024
#endif /* not MAXPATHLEN */
#endif /* not PATH_MAX */

#include "eb.h"
#include "error.h"
#include "internal.h"
#include "font.h"

#include "getumask.h"
#include "makedir.h"

#include "ebutils.h"
#include "ebzip.h"

/*
 * Trick for function protypes.
 */
#ifndef EB_P
#ifdef __STDC__
#define EB_P(p) p
#else /* not __STDC__ */
#define EB_P(p) ()
#endif /* not __STDC__ */
#endif /* EB_P */

/*
 * Tricks for gettext.
 */
#ifdef ENABLE_NLS
#define _(string) gettext(string)
#ifdef gettext_noop
#define N_(string) gettext_noop(string)
#else
#define N_(string) (string)
#endif
#else
#define _(string) (string)       
#define N_(string) (string)
#endif

/*
 * Unexported function.
 */
static int ebzip_zip_book_eb EB_P((EB_Book *, const char *, const char *,
    EB_Subbook_Code *, int));
static int ebzip_zip_book_epwing EB_P((EB_Book *, const char *, const char *,
    EB_Subbook_Code *, int));


/*
 * Compress files in `book' and output them under `out_top_path'.
 * If it succeeds, 0 is returned.  Otherwise -1 is returned.
 */
int
ebzip_zip_book(out_top_path, book_path, subbook_name_list, subbook_name_count)
    const char *out_top_path;
    const char *book_path;
    char subbook_name_list[][EB_MAX_DIRECTORY_NAME_LENGTH + 1];
    int subbook_name_count;
{
    EB_Book book;
    EB_Error_Code error_code;
    EB_Subbook_Code subbook_list[EB_MAX_SUBBOOKS];
    EB_Subbook_Code subbook_code;
    int subbook_count;
    int result;
    int i;

    eb_initialize_book(&book);

    /*
     * Bind a book.
     */
    error_code = eb_bind(&book, book_path);
    if (error_code != EB_SUCCESS) {
	fprintf(stderr, "%s: %s\n", invoked_name,
	    eb_error_message(error_code));
	fflush(stderr);
	return -1;
    }

    /*
     * For each targe subbook, convert a subbook-names to a subbook-codes.
     * If no subbook is specified by `--subbook'(`-S'), set all subbooks
     * as the target.
     */
    if (subbook_name_count == 0) {
	error_code = eb_subbook_list(&book, subbook_list, &subbook_count);
	if (error_code != EB_SUCCESS) {
	    fprintf(stderr, "%s: %s\n", invoked_name,
		eb_error_message(error_code));
	    fflush(stderr);
	    return -1;
	}
    } else {
	subbook_count = 0;
	for (i = 0; i < subbook_name_count; i++) {
	    error_code = find_subbook(&book, subbook_name_list[i],
		&subbook_code);
	    if (error_code != EB_SUCCESS) {
		fprintf(stderr, _("%s: unknown subbook name `%s'\n"),
		    invoked_name, subbook_name_list[i]);
		return -1;
	    }
	    subbook_list[subbook_count++] = subbook_code;
	}
    }

    /*
     * Compress the book.
     */
    if (book.disc_code == EB_DISC_EB) {
	result = ebzip_zip_book_eb(&book, out_top_path, book_path,
	    subbook_list, subbook_count);
    } else {
	result = ebzip_zip_book_epwing(&book, out_top_path, book_path,
	    subbook_list, subbook_count);
    }

    eb_finalize_book(&book);

    return result;
}


/*
 * Internal function for `zip_book'.
 * This is used to compress an EB book.
 */
static const char *catalog_hint_list[] = {"catalog", NULL};

static int
ebzip_zip_book_eb(book, out_top_path, book_path, subbook_list, subbook_count)
    EB_Book *book;
    const char *out_top_path;
    const char *book_path;
    EB_Subbook_Code *subbook_list;
    int subbook_count;
{
    EB_Subbook *subbook;
    char in_path_name[PATH_MAX + 1];
    char out_sub_path[PATH_MAX + 1];
    char out_path_name[PATH_MAX + 1];
    char catalog_file_name[EB_MAX_FILE_NAME_LENGTH];
    mode_t out_directory_mode;
    Zio_Code in_zio_code;
    int i;

    /*
     * If `out_top_path' and/or `book_path' represents "/", replace it
     * to an empty string.
     */
    if (strcmp(out_top_path, "/") == 0)
	out_top_path++;
    if (strcmp(book_path, "/") == 0)
	book_path++;

    /*
     * Initialize variables.
     */
    out_directory_mode = 0777 ^ get_umask();
    eb_initialize_all_subbooks(book);

    /*
     * Compress a book.
     */
    for (i = 0; i < subbook_count; i++) {
	subbook = book->subbooks + subbook_list[i];

	/*
	 * Make an output directory for the current subbook.
	 */
	eb_compose_path_name(out_top_path, subbook->directory_name,
	    out_sub_path);
	if (!ebzip_test_flag
	    && make_missing_directory(out_sub_path, out_directory_mode) < 0)
	    return -1;

	/*
	 * Compress START file.
	 */
	in_zio_code = zio_mode(&subbook->text_zio);

	if (in_zio_code != ZIO_INVALID) {
	    eb_compose_path_name2(book->path, subbook->directory_name,
		subbook->text_file_name, in_path_name);
	    eb_compose_path_name2(out_top_path, subbook->directory_name,
		subbook->text_file_name, out_path_name);
	    fix_path_name_suffix(out_path_name, EBZIP_SUFFIX_EBZ);
	    ebzip_zip_file(out_path_name, in_path_name, in_zio_code);
	}
    }

    /*
     * Compress a language file.
     */
    in_zio_code = zio_mode(&book->language_zio);

    if (in_zio_code != ZIO_INVALID) {
	eb_compose_path_name(book->path, book->language_file_name,
	    in_path_name);
	eb_compose_path_name(out_top_path, book->language_file_name,
	    out_path_name);
	fix_path_name_suffix(out_path_name, EBZIP_SUFFIX_EBZ);
	ebzip_zip_file(out_path_name, in_path_name, in_zio_code);
    }

    /*
     * Copy CATALOG file.
     */
    if (eb_find_file_name(book->path, catalog_hint_list, catalog_file_name,
	NULL) == EB_SUCCESS) {
	eb_compose_path_name(book->path, catalog_file_name, in_path_name);
	eb_compose_path_name(out_top_path, catalog_file_name, out_path_name);
	ebzip_copy_file(out_path_name, in_path_name);
    }

    return 0;
}


/*
 * Internal function for `zip_book'.
 * This is used to compress an EPWING book.
 */
static const char *catalogs_hint_list[] = {"catalogs", NULL};

static int
ebzip_zip_book_epwing(book, out_top_path, book_path, subbook_list,
    subbook_count)
    EB_Book *book;
    const char *out_top_path;
    const char *book_path;
    EB_Subbook_Code *subbook_list;
    int subbook_count;
{
    EB_Subbook *subbook;
    EB_Font *font;
    char in_path_name[PATH_MAX + 1];
    char out_sub_path[PATH_MAX + 1];
    char out_path_name[PATH_MAX + 1];
    char catalogs_file_name[EB_MAX_FILE_NAME_LENGTH];
    mode_t out_directory_mode;
    Zio_Code in_zio_code;
    int i, j;

    /*
     * If `out_top_path' and/or `book_path' represents "/", replace it
     * to an empty string.
     */
    if (strcmp(out_top_path, "/") == 0)
	out_top_path++;
    if (strcmp(book_path, "/") == 0)
	book_path++;

    /*
     * Initialize variables.
     */
    out_directory_mode = 0777 ^ get_umask();
    eb_initialize_all_subbooks(book);

    /*
     * Compress a book.
     */
    for (i = 0; i < subbook_count; i++) {
	subbook = book->subbooks + subbook_list[i];

	/*
	 * Make an output directory for the current subbook.
	 */
	eb_compose_path_name(out_top_path, subbook->directory_name,
	    out_sub_path);
	if (!ebzip_test_flag
	    && make_missing_directory(out_sub_path, out_directory_mode) < 0)
	    return -1;

	/*
	 * Make `data' sub directory for the current subbook.
	 */
	eb_compose_path_name2(out_top_path, subbook->directory_name,
	    subbook->data_directory_name, out_sub_path);
	if (!ebzip_test_flag
	    && make_missing_directory(out_sub_path, out_directory_mode) < 0)
	    return -1;

	/*
	 * Compress HONMON/HONMON2 file.
	 */
	in_zio_code = zio_mode(&subbook->text_zio);

	if (in_zio_code != ZIO_INVALID) {
	    eb_compose_path_name3(book->path, subbook->directory_name,
		subbook->data_directory_name, subbook->text_file_name,
		in_path_name);
	    eb_compose_path_name3(out_top_path, subbook->directory_name,
		subbook->data_directory_name, subbook->text_file_name,
		out_path_name);
	    fix_path_name_suffix(out_path_name, EBZIP_SUFFIX_EBZ);
	    ebzip_zip_file(out_path_name, in_path_name, in_zio_code);
	}

	/*
	 * Compress HONMONS file.
	 */
	in_zio_code = zio_mode(&subbook->sound_zio);

	if (!ebzip_skip_flag_sound
	    && in_zio_code != ZIO_INVALID
	    && strncasecmp(subbook->sound_file_name, "honmons", 7) == 0) {
	    eb_compose_path_name3(book->path, subbook->directory_name,
		subbook->data_directory_name, subbook->sound_file_name,
		in_path_name);
	    eb_compose_path_name3(out_top_path, subbook->directory_name,
		subbook->data_directory_name, subbook->sound_file_name,
		out_path_name);
	    fix_path_name_suffix(out_path_name, EBZIP_SUFFIX_EBZ);
	    ebzip_zip_file(out_path_name, in_path_name, in_zio_code);
	}

	/*
	 * Copy HONMONG file.
	 */
	in_zio_code = zio_mode(&subbook->graphic_zio);

	if (!ebzip_skip_flag_graphic
	    && in_zio_code != ZIO_INVALID
	    && strncasecmp(subbook->graphic_file_name, "honmong", 7) == 0) {
	    eb_compose_path_name3(book->path, subbook->directory_name,
		subbook->data_directory_name, subbook->graphic_file_name,
		in_path_name);
	    eb_compose_path_name3(out_top_path, subbook->directory_name,
		subbook->data_directory_name, subbook->graphic_file_name,
		out_path_name);
	    ebzip_copy_file(out_path_name, in_path_name);
	}

	if (!ebzip_skip_flag_font) {
	    /*
	     * Make `gaiji' sub directory for the current subbook.
	     */
	    eb_compose_path_name2(out_top_path, subbook->directory_name,
		subbook->gaiji_directory_name, out_sub_path);
	    if (!ebzip_test_flag
		&& make_missing_directory(out_sub_path, out_directory_mode)
		< 0) {
		return -1;
	    }

	    /*
	     * Compress narrow font files.
	     */
	    for (j = 0; j < EB_MAX_FONTS; j++) {
		font = subbook->narrow_fonts + j;
		if (font->font_code == EB_FONT_INVALID)
		    continue;

		in_zio_code = zio_mode(&font->zio);

		if (in_zio_code != ZIO_INVALID) {
		    eb_compose_path_name3(book->path,
			subbook->directory_name, subbook->gaiji_directory_name,
			font->file_name, in_path_name);
		    eb_compose_path_name3(out_top_path,
			subbook->directory_name, subbook->gaiji_directory_name,
			font->file_name, out_path_name);
		    fix_path_name_suffix(out_path_name, EBZIP_SUFFIX_EBZ);
		    ebzip_copy_file(out_path_name, in_path_name);
		}
	    }

	    /*
	     * Compress wide font files.
	     */
	    for (j = 0; j < EB_MAX_FONTS; j++) {
		font = subbook->wide_fonts + j;
		if (font->font_code == EB_FONT_INVALID)
		    continue;

		in_zio_code = zio_mode(&font->zio);

		if (in_zio_code != ZIO_INVALID) {
		    eb_compose_path_name3(book->path,
			subbook->directory_name, subbook->gaiji_directory_name,
			font->file_name, in_path_name);
		    eb_compose_path_name3(out_top_path,
			subbook->directory_name, subbook->gaiji_directory_name,
			font->file_name, out_path_name);
		    fix_path_name_suffix(out_path_name, EBZIP_SUFFIX_EBZ);
		    ebzip_copy_file(out_path_name, in_path_name);
		}
	    }
	}

	/*
	 * Copy movie files.
	 */
	if (!ebzip_skip_flag_movie) {
	    eb_compose_path_name2(book->path, subbook->directory_name,
		subbook->movie_directory_name, in_path_name);
	    eb_compose_path_name2(out_top_path, subbook->directory_name,
		subbook->movie_directory_name, out_path_name);
	    ebzip_copy_files_in_directory(out_path_name, in_path_name);
	}
    }

    /*
     * Copy CATALOGS file.
     */
    if (eb_find_file_name(book->path, catalogs_hint_list, catalogs_file_name,
	NULL) == EB_SUCCESS) {
	eb_compose_path_name(book->path, catalogs_file_name, in_path_name);
	eb_compose_path_name(out_top_path, catalogs_file_name, out_path_name);
	ebzip_copy_file(out_path_name, in_path_name);
    }

    return 0;
}
