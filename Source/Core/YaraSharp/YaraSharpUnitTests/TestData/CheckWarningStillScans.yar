// This file contains a rule that should trigger a warning. This should not prevent scanning from being successful

rule WarningRule
{
  strings:
    $error_str = /this.+?is.+?a.+?slow.+?regex/

  condition:
    $error_str
}
