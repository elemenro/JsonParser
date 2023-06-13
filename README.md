# JsonParser

Firstly, I try to parse json to jtype to decide is jobject or jarray.if it's jobject take properties and set to properties.

Then, The comparison logic ensures that primitive properties come before non-primitive properties, and properties are sorted alphabetically by name.

It removes all existing child properties from the object.

It then adds the properties back to the object in the reordered sequence.

For each property, it recursively calls ReorderProperties to handle nested objects or arrays within the property value.

If the input token is a JArray (representing a JSON array), it iterates through each item in the array and recursively calls ReorderProperties on each item.

In summary, the ReorderProperties method reorders the properties of JSON objects so that primitive properties appear first, and it recursively handles nested objects or arrays within the JSON structure.
