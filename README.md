[ConfigFile](https://www.nuget.org/packages/HowlDev.IO.Text.ConfigFile): ![NuGet Version](https://img.shields.io/nuget/v/HowlDev.IO.Text.ConfigFile)

[Parsers](https://www.nuget.org/packages/HowlDev.IO.Text.Parsers): ![NuGet Version](https://img.shields.io/nuget/v/HowlDev.IO.Text.Parsers)

# HowlDev.IO.Text.ConfigFile

This is a hand-written text parser for TXT, YAML, and JSON files. It's designed for use in constructors
and NOT as efficient runtime collections. I simply use internal lists and dictionaries to store the data.

My primary use case is for something like a game, where a file would list all the properties of an enemy 
or item, and you would read from this object to get those parameters. My intention is to make it 
extremely explicit in the way items are read, so I allow for any amount of nested objects and arrays. 
Values are read out as strings, and you can convert them to any type that C# allows them to. This means 
arrays can have any type within them (string, int, double, and bool), and you can decide what they are 
at runtime. I've included a number of error messages to help you debug your files and ensure proper 
type matching back to C# primitives. 

See the wiki [here](https://wiki.codyhowell.dev/io.text) for more information.

## Weak Typing

This was accomplished by my primitive classes implementing an interface (IBaseConfigOption) that includes 
all of their methods and indexes. There are three implementing types, which I will describe below. 

The intended use of these is to have a large object or array, drill down into them via indexers 
(string or int respectively), and get a PrimitiveConfigOption at the end which you would read your value as. 
I can't really prevent you from assigning an Array or Object to something (and indeed, I think that would limit 
how clean the code for deeply nested objects would be), but my intent is to end every call with the full path 
and a method to make explicit what value is being read. 

### Primitive Config

This is a string, and has methods to convert it to any of the above listed types. All other methods throw 
an InvalidOperationExceptions. If a string can't be converted, it will throw a InvalidCastException.

### Array Config

This is a list of IBaseConfigOption objects (to allow for an assorted array of objects, arrays, and primitives). 
It has an int indexer to retreive specific elements, and four additional methods if everything in the array 
is of the same type (since you might be able to do more with a list of primitives). Otherwise, you can continue 
to index down into a primitive and get your value. 

Using the string indexing or attempting to read it as a primitive will throw an InvalidOperationException.

### Object Config

This is a Dictionary<string, IBaseConfigOption>. Its only valid property is the string indexer, which will
retrieve the IBaseConfigOption at that key. Everything else will return an InvalidOperationException.

## Usage

For all file types, you can read through my test cases to see what the file looks like and my test 
to see what exactly the call will look like. I've included my "Realistic" test cases for each file type
and their calls to read the values. In each case they start with "reader[...]", which is the ConfigFile object 
for each file type. The constructor just takes in the path to the file, and some have options for a different 
splitter or something like that. Read the XML comments for more information.

### Collector

I added a new class called `ConfigFileCollector` that takes in a list of filepaths and parses them 
all together. When you need a specific file, request it from the proper method and use it as normal. 

There's a number of exceptions thrown to help with the process. 

### TextConfigFile

There are no more single-type classes. Everything goes through this parent class, `TextConfigFile`. They will automatically parse by the extension or throw an error if it is unrecognized. 

To get text parsed without going through the file system, go through the `ReadTextAs` method. There are comments to guide you. 

Below is the three file types I support and an example (mostly legacy from having 3 separate classes). 

### TXT

This only supports single-order objects, though you can have arrays within them (which are split via commas). It's 
designed to be the simplest to use. Arrays can be inline or multiline, and you can have any amount of whitespace
around them. It reads to the next line that contains the closing "]", then splits the entire string via commas 
and assigns them each to a PrimitiveConfigOption. You can see those options in my example.

As this is the first, note that I don't use any double quotes anywhere; everything is split via the character
and then strings are trimmed afterwards, for both keys and values.

```txt
info: [John Doe, 29, 6.1, True]
data: [
    True,
    42,
    Seattle, ]
preferences: [ Travel,
    Music,
    3.14
]
address: 123 Main St
zip: 98101
```

```csharp
reader["info"][0].AsString(); // "John Doe"
reader["info"][3].AsBool(); // true
reader["data"][1].AsInt(); // 42
reader["address"].AsString(); // "123 Main St"

// Example of exporting a list
reader["info"].AsStringList(); // ["John Doe", "29", "6.1", "True"]
```

### YAML

This supports any amount of nested objects and arrays, and (somewhat) follows the YAML spec. I spent some time reading through it and as I am writing all the parsing logic, I've decided to stop where I'm at. 

If you want text parsed as a single primitive (such as a multiline string), use this file extension (or parse it using this type in `ReadTextAs`).  

Tabs are supported, and I also currently support only 4 space indents. The below object will show what I 
expect in terms of arrays of objects, and I think everything else is somewhat self-explanatory. If you'd 
like to submit a pull request for a better parser, I'd be happy to look at it. 

```yaml
first: 
    simple Array: 
        - 1
        - 2
        - 3
    brother: sample String
    other sibling: 
        sibKey: sibValue
        sibKey2: sibValue2
second: 
    arrayOfObjects: 
        - lorem: ipsum
          something: 1.2
        - lorem2: ipsum2
          something2: false
    otherThing: hopefully
```

```csharp
reader["first"]["simple Array"][1].AsInt(); // 2
reader["first"]["other sibling"]["sibKey"].AsString(); // "sibValue"
reader["second"]["arrayOfObjects"][0]["lorem"].AsString(); // "ipsum"
reader["second"]["arrayOfObjects"][0]["something"].AsDouble(); // 1.2
reader["second"]["arrayOfObjects"][1]["something2"].AsBool(); // false
```

### JSON

The way I'm implementing JSON requires some syntax specifics; first, you can only start with a 
[] or a {} for an array or object. Arrays are always split by commas ',' and objects are 
always keyed with a colon ':' and again split via commas. I don't support escape characters 
at the moment (or comments, in any form), but otherwise is formatted exactly like JSON.

This is the exact same data as stored in the above YAML file, referenced in the same way in C#.

```json
{
  "first": {
    "simple Array": [
      1,
      2,
      3
    ],
    "brother": "sample String",
    "other sibling": {
      "sibKey": "sibValue"
    }
  },
  "second": {
    "arrayOfObjects": [
      {
        "lorem": "ipsum",
        "something": 1.2
      },
      {
        "lorem2": "ipsum2",
        "something2": false
      }
    ],
    "otherThing": "hopefully"
  }
}
```

```csharp
reader["first"]["simple Array"][1].AsInt(); // 2
reader["first"]["other sibling"]["sibKey"].AsString(); // "sibValue"
reader["second"]["arrayOfObjects"][0]["lorem"].AsString(); // "ipsum"
reader["second"]["arrayOfObjects"][0]["something"].AsDouble(); // 1.2
reader["second"]["arrayOfObjects"][1]["something2"].AsBool(); // false
```

### As

You can now export types from config files! It's similar to having the JSON serializer export to a type, but now, you can go through my class instead. There's a few options, such as going only through constructors or properties (not both) and filling in values that way, and there's also a Strict mode which should help if you are experiencing random empty values that you thought would be filled. 

It isn't very well tested (though I have a number of unit tests for those methods), so I will spend some time in the "real world" trying it out. 

Here's a few tests, showing how it works: 

```csharp
[Test]
public async Task PersonRecordTest() {
    string txt = """
    name: Jane
    id: 23
    """;
    TextConfigFile reader = TextConfigFile.ReadTextAs(FileTypes.TXT, txt);

    PersonRecord p = reader.As<PersonRecord>();
    await Assert.That(p.name).IsEqualTo("Jane");
    await Assert.That(p.id).IsEqualTo(23);
}


[Test]
public async Task BookClassTest() {
    string txt = """
    name: Little Women
    weight: 2.3
    height: 12.3
    """;
    TextConfigFile reader = TextConfigFile.ReadTextAs(FileTypes.TXT, txt);

    BookClass b = reader.AsProperties<BookClass>();
    await Assert.That(b.name).IsEqualTo("Little Women");
    await Assert.That(b.weight).IsEqualTo(2.3);
    await Assert.That(b.height).IsEqualTo(12.3);
}

[Test]
public async Task PersonRecordTest() {
    string txt = """
    name: Jane
    id: 23
    """;
    TextConfigFile reader = TextConfigFile.ReadTextAs(FileTypes.TXT, txt);

    PersonRecord p = reader.AsConstructed<PersonRecord>();
    await Assert.That(p.name).IsEqualTo("Jane");
    await Assert.That(p.id).IsEqualTo(23);
}
```

Some goals for next patch: 
- Make AsStrictEnumerable more explicit (I did try to write this and implement tests for it, but it wasn't working as expected. Currently, just pass in an options for what you need, as shown in the new test file AsEnumerable). 
- Complete YAML parser (some minor errors occur in my parsing tests)

## Changelog

3.3.2, 1.1.1 (2/26/26)

- JSON parser now uses ReadOnlySpan&lt;span&gt;. I did a small benchmark of ~64kB file, and the test wasn't very different, but generally faster (maybe at most 5ms out of 15ms), so your mileage may vary. 
- Version bump for both projects. 

3.3.1 (2/25/26)

- Version bump
- Changed ObjectConfigOption to a FrozenDictionary. This led to a slight increase in constructor time, as I now use HashSet to ensure no duplicate keys (though the frozen version wouldn't have both anyways).
- Changed ArrayConfigOption to an ImmutableArray. I just wanted to make internal calls immutable. (and I think that Frozen is slightly better, but immutable is fine above that). 

3.3.0 (2/20/26)

- Now supports nested enumerables with the `As<>` keyword. Nested strings, ints, or other objects listed as an `IEnumerable`, `[]`, or `List`. 

3.2.0 (2/6/26)

- Updated to use most recent version of parser.

3.1.0 (1/5/26)

- Added enumerator to `ConfigFileCollector` so it's much easier to enumerate through a file set. 

3.0.1 (1/5/26)

- Had some problems getting the parser type, testing a simple republish. 

3.0 (12/26/25)

SOME BREAKING CHANGES: 

- Moved reflection elements to the option interface to allow for items like below: 

```csharp
// Inital (and still an option)
reader.As<PersonRecord>();
// Now
reader["person"].As<PersonRecord>();
```

- Enables nested reflection more easily (as before it wasn't possible).
- IMPORTANT: Removed the four primmitive types AsBool, AsInt, AsDouble, AsString, in favor of the IConvertible counterparts (which are ToBoolean, ToInt32, ToDouble, and ToString(null)). You may need to provide null as the format provider when making the calls: `reader.ToInt32(null)`. 
  - This was done to simplify the interface since I was applying the IConvertible interface anyways, so I might as well get rid of extras that were simply extra calls. 
  - This also enables Convert.To___(option). 
  - IMPORTANT: Also removed the As___List() functions. The new functionality is listed below. This was done to make it more extendable in the future. 
- Updated Primitive options to include the As&lt;T&gt; function so that you can more quickly and explicitly say, `option.As<int>()` which immediately returns an int. This is a nice shorthand that resembles the four primitive methods I started with.
  - I've updated the XML comment to discuss both the reflection function (for Objects) and how the Primitive side works. It's a little bit like overloading, but it seems to make the most syntactic sense right now.


```csharp
string json = """
[ "false", "True" ]
""";
TextConfigFile reader = TextConfigFile.ReadTextAs(FileTypes.JSON, json);

List<bool> p = [.. reader.AsEnumerable<bool>()]; // This is collection syntax
await Assert.That(p[0]).IsEqualTo(false);
await Assert.That(p[1]).IsEqualTo(true);
```

2.0.1 (12/17/25)

- (Partially so I can get rid of the Symbols error thrown from my last one...)
- Updated OptionMappingOptions to a class and added dual constructors. 
  - AsStrict now takes in an Options... and adds the strict method to it, which makes strict options more explicit

2.0 (12/15/25)

I couldn't wait. 

- Updated target to Net8.0.
- I think I'm happy with the external signature. I may add or remove small things, but I think it's pretty good for now. Hopefully my next major version is not for a little while
- Made my internal IBaseConfigOption parser-retriever public so you can generate your own using your own parser/experiment with the tokenizer system. 
  - `ConvertTokenStreamToConfigOption`
  - Added a few more tests to cover cases I knew existed but now throws errors (such as sending an EndArray inside of an object) which should hopefully help debug. 
- Created an Actions workflow to automatically publish to NuGet. 
  - Sometimes I'm not very good at CI/CD.

2.0-beta (12/12/25)

- Consolidated to TextConfigFile for all classes
- Moved parsers to another library (to be read externally if it would be helpful)
- Added the As__&lt;T&gt;() functions to strongly type. 

This will be removed from beta probably in a few months. 

1.0 (6/25/25)

- Added new class to compile multiple file readers together, ConfigFileCollector
    - with a number of tests in many configurations
- Removed all warnings
- Updated all documentation

0.8 (5/12/25)

- Added JSON option

0.7 (3/8/25)

- Added a helper class for YAML parsing
- YAML parsing works as expected and passes all tests

0.4 (2/27/25)
	
- Primitive objects are fully implemented with errors and tests
- Arrays and objects seem to be working fine
- TXT files are working within spec and have tests
- Starting on YAML; built a helper class and hope to get it up soon

# HowlDev.IO.Text.Parsers

Contains helpers and enums as well as the 3 text parsers used for the ConfigFile above. There is still a bit more to test, so this will also be in beta for some time. Though it already is sufficient for the current suite of ConfigFile tests. 

## Token Format

The tokens come back in a particular way. There is a Frame object in the ConfigFile library that takes them in a particular order and outputs the results objects, arrays, and primitives. 

That order has a general format with a few rules. I hope to have a better guide for tokenization in the future, but here are some: 
- Objects must specify a key before applying a value
- Primitives, close object tokens, and close array tokens can all be children to an object or array

1.1.0 (2/6/26)

- Updated to allow for reading strings of `{}` or `[]`, which previously did not work in JSON. 

1.0.1 (1/5/26)

- Had problems referencing ITokenParser, testing a simple republish. 

1.0 (12/15/25)

- Not sure why I had it in beta. 
- Updated target to Net8.0.

1.0-beta (12/12/25)

- Created project

