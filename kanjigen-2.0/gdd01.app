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
	0xa13a		$B!{(B
	0xa13b		$B!{(B
	0xa13c		$B%p(B
	0xa13d		$B%q(B
end

begin wide
	range-start	0xa121
	range-end	0xa15c

	0xa121		($B$*$D$K$g$&(B|$BMp(B)
	0xa122		($B$K$s$Y$s(B|$B:n(B)
	0xa123		(?$B$+$7$i(B|$BE^(B)
	0xa124		($B$7$?$4$3$m(B|$BJi(B)
	0xa125		($B$j$C$7$s$Y$s(B|$B2w(B)
	0xa126		($B$5$s$:$$(B|$B3$(B)
	0xa127		($B$F$X$s(B|$BEj(B)
	0xa128		($B$l$s$,(B|$BG.(B)
	0xa129		($B$7$?$_$:(B|$BBY(B)
	0xa12a		($B$D$a$+$s$`$j(B|$B<_(B)
	0xa12b		($B$7$g$&$X$s(B|$BAT(B)
	0xa12c		($B$1$b$N$X$s(B|$B<m(B)
	0xa12d		($B$d$^$$$@$l(B|$BIB(B)
	0xa12e		(?$B$"$7(B|$Bc<(B)
	0xa12f		(?$B$+$7$i(B|$B<s(B)
	0xa130		($B$*$$$+$s$`$j(B|$BO7(B)
	0xa131		($B$/$5$+$s$`$j(B|$BAp(B)
	0xa132		($B$3$m$b$X$s(B|$BM5(B)
	0xa133		($B$7$s$K$g$&(B|$BF;(B)
	0xa134		($B$*$*$6$H(B|$BIt(B)
	0xa135		($B:S(B)
	0xa136		($B9u(B)
	0xa137		($B11(B)
	0xa138		($B$"$_$,$7$i(B|$BCV(B)
	0xa139		($BQ[(B)
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
	0xa14a		[$B0l(B]
	0xa14b		[$BFs(B]
	0xa14c		[$B;0(B]
	0xa14d		[$B;M(B]
	0xa14e		[1]
	0xa14f		[2]
	0xa150		[3]
	0xa151		[4]
	0xa152		[5]
	0xa153		$B(!(B
	0xa154		$B!!(B
	0xa155		[$B("(B]
	0xa156		$B!!(B
	0xa157		[$B(#(B]
	0xa158		[$B(&(B]
	0xa159		[$B""(B]
	0xa15a		[$B"#(B]
	0xa15b		($B$3$6$H$X$s(B|$B8B(B)
	0xa15c		$B!](B
end

