#
# Copyright (C) 2003  Motoyuki Kasahara
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

#
# この辞書の本文を読む際は、本文の終端を意味する「表示終了指定コード」
# (0x1f03) に出会うまで読み続けるのが、ユーザの期待する動作となります。
# つまり、この辞書には stop-code が存在しません。
#
# しかし、appendix には「stop-code が存在しない」ということを明示的に
# 指定する方法はありません。
# 
# appendix で stop-code を定義しないようにしても、EB ライブラリが
# stop-code を自動判定してしまいます。この辞書には stop-code が無いわ
# けですから、自動判定も抑止しなくてはならないのですが、EB ライブラリ
# ではそれができないのです。
#
# そのため、この辞書では、stop-code として辞書の本文中には一度も登場し
# ないコードを指定するようにしました。こうすると、stop-code に遭遇して
# 出力が停止することは起こらないため、必ず表示終了コード (0x1f03) に出
# 会ったところで出力が止まります。
#
stop-code	0x1f09 0xffff
