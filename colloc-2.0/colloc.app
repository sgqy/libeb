#
# Copyright (C) 1998  Ryuichi Arafune
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

	0xa121	(+)
	0xa122	=
	0xa123	~c,
	0xa124	\e
	0xa125	'~
	0xa126	'n
	0xa127	:
	0xa128	~epsi
	0xa129	~a
	0xa12a	a
	0xa12b	n~
	0xa12c  
	0xa12d	
	0xa12e	
	0xa12f	

	0xa130	o'
	0xa131	E'
	0xa132	a'
	0xa133	e'
	0xa134	i'
	0xa135	a`
	0xa136	e`
	0xa137	e"
	0xa138	i"
	0xa139	o"
	0xa13a	a^
	0xa13b	e^
	0xa13c	o^
	0xa13d	o-
end

begin wide
	range-start	0xb121
	range-end	0xb12e

	0xb121  [[E]]
	0xb122	→
	0xb123  !
	0xb124	[口素]
	0xb125	[月各]
	0xb126	
	0xb127	***
	0xb128  ***
	0xb129	***
	0xb12a  ***
	0xb12b	(c)
	0xb12c	キ
	0xb12d	↑
	0xb12e	SeeText
end

