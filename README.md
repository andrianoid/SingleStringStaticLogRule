# DE-USG-099 - Single String Static Log

**Rule ID:** `DE-USG-099`  
**Scope:** Activity

### Description

The rule check if the message property of LogMessage activities contain a static string. This rule supports a requirement to ensure no data elements (variables, etc) are exposed via log messages.

This rule ensures:
- Log message contains exactly two double-quotes
- Log message begins with a double-quote
- Log message ends with a double-quote

### Recommendation

Ensure that all log messages contain a single, static string. Variables and string concatenation are restricted.