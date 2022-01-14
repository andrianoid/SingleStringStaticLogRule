# ST-USG-099 - Single String Static Log

**Rule ID:** `ST-USG-099`  
**Scope:** Activity

### Description

The rule check if the message property of LogMessage activities contain a static string. This rule supports a requirement to ensure no data elements (variables, etc) are exposed via log messages.

This rule ensures:  
- Log message contains exactly two double-quotes  
- Log message begins with a double-quote  
- Log message ends with a double-quote  

### Exceptions
Cases may be excluded via use of the **`Whitelist of Keywords`** input argument, which accepts a comma-separated list of keywords. If a message contains a given keyword, the exception will be of type `Warning` instead of `Error`, allowing for manual review.


### Recommendation

Ensure that all log messages contain a single, static string. Variables and string concatenation are restricted, but can be excluded using the **`Whitelist of Keywords`** input argument.