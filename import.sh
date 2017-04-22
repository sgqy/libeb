
### alias llt='ls -alF --time-style=long-iso --sort=time'
### git alias : ac = !git add -A && git commit

flist=(`llt -r $1 | grep 'eb-' | awk '{print $8}'`)
ftime=(`llt -r $1 | grep 'eb-' | awk '{print $6"T"$7}'`)
dname=(`llt -r $1 | grep 'eb-' | awk '{print $8}' | sed 's/\.tar\..*//g'`)
len=${#flist[@]}

for (( i=0; i<${len}; i++ ));
do
     BR=${flist[$i]:0:6}
echo git branch ${BR}
     git branch ${BR}
echo git checkout ${BR}
     git checkout ${BR}
echo git rm -rf . && git clean -xfd 
     git rm -rf . && git clean -xfd 
echo tar xf "$1/${flist[$i]}" -C "$1/"
     tar xf "$1/${flist[$i]}" -C "$1/"
echo mv "$1/${dname[$i]}/"* .
     mv "$1/${dname[$i]}/"* .
echo rm -rf "$1/${dname[$i]}"
     rm -rf "$1/${dname[$i]}"
     TGT_DATE="${ftime[$i]}"
     GIT_AUTHOR_DATE="${TGT_DATE}"
     GIT_COMMITTER_DATE="${TGT_DATE}" 
echo -- au ${GIT_AUTHOR_DATE} -- cm ${GIT_COMMITTER_DATE}
echo git ac -m "Import ${dname[$i]}"
     GIT_AUTHOR_DATE="${TGT_DATE}"     \
     GIT_COMMITTER_DATE="${TGT_DATE}"  \
     git ac -m "Import ${dname[$i]}"
echo
done


### other used command:
### llt -r | grep 'eb-' | awk '{print "mv ~/eb/win32/"$8"* .\n""TGT_DATE=\""$6,$7"\" GIT_AUTHOR_DATE=\"\${TGT_DATE}\" GIT_COMMITTER_DATE=\"\${TGT_DATE}\" git ac -m \"Import "$8"\"\ngit rm -rf . && git clean -xfd"}

