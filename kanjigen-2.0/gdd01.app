#
# Copyright (C) 1997  Motoyuki Kasahara
#
# This program is free software; you can redistribute it and/or modify
# it under the terms of the GNU General Public License as published by
# the Free Software Foundation; either version 2, or (at your option)
# any later version.
# 
# This program is distributed in the hope that it will be useful,
# but WITHOUT ANY WARRANTY; without even the implied warranty of
# MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
# GNU General Public License for more details.
#

character-code	jisx0208

stop-code	0x1f09 0x0001

begin narrow
	range-start	0xa121
	range-end	0xa13d

	0xa121		=a
	0xa122		'a
	0xa123		<a
	0xa124		`a
	0xa125		=e
	0xa126		'e
	0xa127		<e
	0xa128		`e
	0xa129		=i
	0xa12a		'i
	0xa12b		<i
	0xa12c		`i
	0xa12d		=o
	0xa12e		'o
	0xa12f		<o
	0xa130		`o
	0xa131		=u
	0xa132		'u
	0xa133		<u
	0xa134		`u
	0xa135		="u
	0xa136		'"u
	0xa137		<"u
	0xa138		`"u
	0xa139		"u
	0xa13a		○
	0xa13b		○
	0xa13c		ヰ
	0xa13d		ヱ
end

begin wide
	range-start	0xa121
	range-end	0xa15c

	0xa121		(おつにょう|乱)
	0xa122		(にんべん|作)
	0xa123		(?かしら|党)
	0xa124		(したごころ|慕)
	0xa125		(りっしんべん|快)
	0xa126		(さんずい|海)
	0xa127		(てへん|投)
	0xa128		(れんが|熱)
	0xa129		(したみず|泰)
	0xa12a		(つめかんむり|爵)
	0xa12b		(しょうへん|壮)
	0xa12c		(けものへん|狩)
	0xa12d		(やまいだれ|病)
	0xa12e		(?あし|禺)
	0xa12f		(?かしら|首)
	0xa130		(おいかんむり|老)
	0xa131		(くさかんむり|草)
	0xa132		(ころもへん|裕)
	0xa133		(しんにょう|道)
	0xa134		(おおざと|部)
	0xa135		(采)
	0xa136		(黒)
	0xa137		(臼)
	0xa138		(あみがしら|置)
	0xa139		(凛)
	0xa13a		(?)
	0xa13b		(1)
	0xa13c		(2)
	0xa13d		(3)
	0xa13e		(4)
	0xa13f		(5)
	0xa140		(6)
	0xa141		(7)
	0xa142		(8)
	0xa143		(9)
	0xa144		(10)
	0xa145		(11)
	0xa146		(12)
	0xa147		(13)
	0xa148		(14)
	0xa149		(15)
	0xa14a		[一]
	0xa14b		[二]
	0xa14c		[三]
	0xa14d		[四]
	0xa14e		[1]
	0xa14f		[2]
	0xa150		[3]
	0xa151		[4]
	0xa152		[5]
	0xa153		─
	0xa154		　
	0xa155		[│]
	0xa156		　
	0xa157		[┌]
	0xa158		[└]
	0xa159		[□]
	0xa15a		[■]
	0xa15b		(こざとへん|限)
	0xa15c		−
end

