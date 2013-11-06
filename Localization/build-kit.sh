#!/bin/bash

OUTDIR=translator-kit

# Always create a kit from scratch
test -d "$OUTDIR" || mkdir "$OUTDIR"
rm -rf "$OUTDIR"/*

cp -p solution.base "$OUTDIR/POLUtils-Localization.sln"

exec 5>"$OUTDIR/POLUtils-Localization.csproj"
cat project.top >&5

# Locate designer sources
echo "scanning source tree..."
for FILE in $(find .. -type f '-(' -name '*.Designer.cs' -o -name '*.resx' '-)' -a -! -path '../Localization/*'); do
  dir=$(dirname "$FILE")
  dosdir=$(dirname "$FILE" | cut -d/ -f2- | tr '/' '\\')
  project=$(dirname "$FILE" | cut -d/ -f2)
  projectsubdir=$(dirname "$FILE" | cut -d/ -f3-)
  file=$(basename "$FILE")
  outdir="$OUTDIR/$project/$projectsubdir"
  echo "adding $file from $project"
  test -d "$outdir" || mkdir -p "$outdir"
  case $file in
    *.resx)
      base=$(printf %q "$file" | sed -e 's/\([.][a-z][a-z]\(_[A-Z][A-Z]\|\)\|\)[.]resx$//')
      echo "    <EmbeddedResource Include=\"$dosdir\\$file\">" >&5
      if test -f "$dir/$base.Designer.cs" && test -f "$dir/$base.cs"; then
        echo "      <DependentUpon>$base.cs</DependentUpon>" >&5
      fi
      echo "    </EmbeddedResource>" >&5
      cp -p "$FILE" "$outdir/$file"
      ;;
    *.Designer.cs)
      base=$(printf %q "$file" | sed -e 's/[.]Designer[.]cs$//')
      if test -f "$dir/$base.cs"; then
        if test ! -f "$OUTDIR/$project/$base.cs"; then
          echo "    <Compile Include=\"$dosdir\\$base.cs\" />" >&5
          gawk -f extract_base.awk < "$dir/$base.cs" > "$outdir/$base.cs"
        fi
      fi
      echo "    <Compile Include=\"$dosdir\\$file\">" >&5
      if test -f "$dir/$base.cs"; then
        echo "      <DependentUpon>$base.cs</DependentUpon>" >&5
      fi
      echo "    </Compile>" >&5
      gawk -f fixup_source.awk < "$FILE" > "$outdir/$file"
      ;;
  esac
done

# Addition localization resources
test -d "$OUTDIR/Installer" || mkdir "$OUTDIR/Installer"
cp "../Binaries/Languages.nsh" "$OUTDIR/Installer/"
echo "    <Content Include=\"Installer\\Languages.nsh\" />" >&5
cp "readme.installer" "$OUTDIR/Installer/readme.txt"
echo "    <Content Include=\"Installer\\readme.txt\" />" >&5
cp "readme.vs" "$OUTDIR/readme.txt"
echo "    <Content Include=\"readme.txt\" />" >&5

# Finish up
cat project.bottom >&5
exec 5>&-
