#! /bin/sh
#
# Make a source distribution package.
#
VERSION="0.0"

rm -f .distfiles
sed -e 's/^[ 	]*//' > .distfiles <<__END__
    src/AssemblyInfo.cs
    src/EBCommand.cs
    src/EBInfo.cs
    src/EBRefile.cs
    src/EBZip.cs
    src/FormMain.cs
    src/FormMain.resx
    src/FormOverwrite.cs
    src/FormOverwrite.resx
    src/FormVersion.cs
    src/FormVersion.resx
    src/FormZipConfig.cs
    src/FormZipConfig.resx
    src/FormZipProgress.cs
    src/FormZipProgress.resx
    src/Registry.cs
    src/WinEBZip.csproj
    src/WinEBZip.ico
    src/WinEBZip.sln
    src/unzip.bmp
    src/zip.bmp
    doc/WinEBZip.css
    doc/WinEBZip.html
    doc/book-browser.png
    doc/complete.png
    doc/eb-browser.png
    doc/eb_b.png
    doc/explorer.png
    doc/main-book.png
    doc/main-empty.png
    doc/toolbar.png
    doc/zip-config-size.png
    doc/zip-config1.png
    doc/zip-config2.png
    doc/zip-config3.png
    doc/zip-config4.png
    doc/zip-progress.png
    HISTORY.txt
    COPYING.txt
    WinEBZip.css
    makedist.sh
    setver.sh
__END__

rm -rf WinEBZip-$VERSION
rm -f WinEBZip-$VERSION.zip
mkdir WinEBZip-$VERSION

echo "Creating: WinEBZip-$VERSION.zip"

cat .distfiles | while read I; do
    for J in $I; do
	[ -f $J ] || continue
	[ -f WinEBZip-$VERSION/$J ] && continue
	DIR="`dirname $J`"
	mkdir -p WinEBZip-$VERSION/$DIR > /dev/null
	cp $J WinEBZip-$VERSION/$J
    done
done

zip -9 -q -r WinEBZip-$VERSION.zip WinEBZip-$VERSION
rm -rf WinEBZip-$VERSION .distfiles
