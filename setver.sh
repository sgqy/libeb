#! /bin/sh

if [ $# -eq 0 ]; then
    echo "Usage: $0 version" 1>&2
    exit 1
fi

VERSION=$1

#
# makedist.sh
#
FILE=makedist.sh

rm -f $FILE.tmp
sed -e 's/^VERSION=.*$/VERSION="'"$VERSION"'"/' \
   $FILE > $FILE.tmp && mv -f $FILE.tmp $FILE

#
# WinEBZip.iss
#
FILE=WinEBZip.iss

rm -f $FILE.tmp
sed -e 's/^AppVerName=.*$/AppVerName=WinEBZip '"$VERSION/" \
    -e 's/OutputBaseFileName=.*$/OutputBaseFileName=WinEBZip-'"$VERSION/" \
        $FILE | nkf -s -Lw > $FILE.tmp && mv -f $FILE.tmp $FILE

#
# src/FormVersion.cs
#
FILE=src/FormVersion.cs

rm -f $FILE.tmp
sed -e 's/string Version = .*$/string Version = "'"$VERSION"'";/' \
        $FILE | nkf -s -Lw > $FILE.tmp && mv -f $FILE.tmp $FILE
