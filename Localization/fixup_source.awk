BEGIN {
  FS = " ";
  controlcount = 0;
}

/[+]= new .*[(].*[)];[\r]?$/ { # eliminate event handlers
  next;
}

/[.]ie[^ .]+[.]Item =/ { # eliminate an item editor's Item property
  next;
}

/^[[:space:]]*this[.]TabName = \".*\";[\r]?$/ { # eliminate a PropertyPages.IThing's TabName property
  next;
}

{ # keep every line (with perhaps a few substitutions)
  gsub("ThemedTabPage", "TabPage");
  print $0;
}
