
			EB Library for Windows
		 C Library for accessing CD-ROM books


1. Introduction

EB Library is a C library for accessing CD-ROM books.  It is
originally developed for UNIX derived systems.  This package provides
provides stuff to build EB Library on Microsoft Windows with Visual
Studio 6.0.

EB Library supports to access CD-ROM books of EB, EBG, EBXA, EBXA-C,
S-EBXA and EPWING formats.  CD-ROM books of those formats are popular
in Japan.  Since CD-ROM books themseves are stands on the ISO 9660
format, you can mount the discs by the same way as other ISO 9660
discs.

EB Library also provides some utility commands:

	ebfont		get font data of local defined characters in
			a CD-ROM book.
	ebinfo		list information about a CD-ROM book.
	ebrefile	refile a catalog file in a CD-ROM book.
	ebstopcode	utility to analyze text data in a CD-ROM book for
			stop code.
	ebunzip		uncompress a CD-ROM book.
	ebzip		compress a CD-ROM book.
	ebzipinfo	output information about a compressed CD-ROM
			book.

Note that this package doesn't provide the `ebappendix' command since
that is Perl script.

Beginning with version 4.0, EB Library supports remote book access.
You can specify a CD-ROM book on a remote host with remote access
identifer.  The format of the identifer is:

	ebnet://<host>:<port>/<book-name>	  (CD-ROM book)
	ebnet://<host>:<port>/<book-name>.app	  (appendix package)

<host> is a host name or an IP address of the remote host.  Note that
an IPv6 address must be enclosed in `[' and `]'.  Note: The binary
distribution supports IPv4 only.

<port> is a port number that the peer listens on.  You can omit the
`:<port>' part if the host listens on the default port, 22010.

For example:
	ebnet://eb.example.com/dict
	ebnet://eb.example.com:22010/dict.app
	ebnet://192.168.1.1/dict.app
	ebnet://192.168.1.1:22010/dict
	ebnet://[fe80::290:27ff:fe3]/dict.app
	ebnet://[fe80::290:27ff:fe3]:22010/dict

To use the remote access, you have to install EBNETD on <host>,
which is remote access server software for EB Library.


EB Library is free software; you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation; either version 2, or (at your option)
any later version.

EB Library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.


You can get the latest EB Library from

	ftp://ftp.sra.co.jp/pub/misc/eb/

You can get information about EB Library from

	http://www.sra.co.jp/people/m-kasahr/eb/

Mail comments and bug reports for these programs to

	m-kasahr@sra.co.jp

in Japanese or English.


2. Build from Source

Visual Studio 6.0 is required to build EB Library binaries from source
distribution.  Extract EB Library source distribution and its 
corresponding version of Windows source distribution at the same
directory.  After the extraction, the work directory have the
sub-diretories:

	eb-<version>-win32\
	eb-<version>\

Then, make Visual Stdio read the workspace file

	eb-<version>-win32\eb.dsw

and perform build for all projects in the workspace.


3. Generated Binaries

If you builds binaries without any modification, or use the binary
distribution, the binaries have the following features:

    * Remote Acess
      avaialable, but IPv6 is not supported.

    * Native Language Support (NLS)
      available, but the supported character encoding is Shift JIS
      only.

    * Multi Thread
      not avaialable.

The binary files in the binary distribution are built by the `Release'
targets.


4. Link EB Libary with Other Application

If you'd like to distribute your application which links EB Library,
you have to put `eb.dll' into the distribution.  If you need native
language support of EB Library (i.e. internationalization of 
eb_error_message()), also put `locale/eb.mo' into the distribution.

`eb.mo' must be located at either:

	locale\LC_MESSAGES\Japanese\eb.mo
	..\locale\LC_MESSAGES\Japanese\eb.mo

relative to the directory where your application resides.


5. Uninstall

To remove EB Library installed by the binary distribution, Execute
`unins000.exe' in the installed directory.

