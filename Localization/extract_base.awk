BEGIN {
  namespacecount = 0;
  partial_seen = 0;
  print "using System.Windows.Forms;";
}

partial_seen == 0 && /^[ \t]*namespace +[^[:space:]]+ *{/ {
  print $0;
  ++namespacecount;
}

partial_seen == 0 && /^[ \t]*((public|private|internal) +)?partial +class +[^[:space:]]+ *: .*/ {
  print $0;
  print "}";
  partial_seen = 1;
}

END {
  while (namespacecount > 0) {
    print "}";
    --namespacecount;
  }
}
