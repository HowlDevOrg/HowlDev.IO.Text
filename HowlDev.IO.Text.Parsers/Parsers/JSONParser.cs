using System.Collections;
using HowlDev.IO.Text.Parsers.Enums;

#pragma warning disable IDE0130
namespace HowlDev.IO.Text.Parsers;

/// <summary/>
/// <summary>
/// Computes the internal enumerable on creation.
/// </summary>
public class JSONParser(string file) : ITokenParser {
    private static char[] chars = ['{', '}', '[', ']', ','];
    private List<(TextToken, string)> computation = GetFileValue(file);

    /// <summary/>
    private static List<(TextToken, string)> GetFileValue(string file) {
        if (!file.StartsWith('[') && !file.StartsWith('{')) {
            throw new InvalidDataException("JSON file must start with either [ or {");
        }

        ReadOnlySpan<char> fileValue = file.Replace('\r', ' ').Replace('\n', ' ');
        List<(TextToken, string)> returnVals = [];

        ParseFileContents(fileValue, returnVals);
        return returnVals;
    }



    private static void ParseFileContents(ReadOnlySpan<char> file, List<(TextToken, string)> list) {
        int index = 0;
        Stack<bool> contextStack = new(); // true = in object, false = in array

        while (index < file.Length) {
            (int i, char c) = NextCharacter(file, index);
            switch (c) {
                case '{':
                    // Process any content before the opening brace (could be a key)
                    if (i > index) {
                        string segment = file[index..i].ToString().Trim();
                        if (!string.IsNullOrEmpty(segment)) {
                            bool inObject = contextStack.Count > 0 && contextStack.Peek();
                            ProcessSegment(segment, inObject, list);
                        }
                    }

                    list.Add((TextToken.StartObject, ""));
                    contextStack.Push(true); // Push object context
                    index = i + 1;
                    break;
                case '}':
                    // Process any content before the closing brace
                    if (i > index) {
                        string segment = file[index..i].ToString().Trim();
                        if (!string.IsNullOrEmpty(segment)) {
                            bool inObject = contextStack.Count > 0 && contextStack.Peek();
                            ProcessSegment(segment, inObject, list);
                        }
                    }

                    list.Add((TextToken.EndObject, ""));
                    if (contextStack.Count > 0) contextStack.Pop(); // Pop object context
                    index = i + 1;
                    break;
                case '[':
                    // Process any content before the opening bracket (could be a key)
                    if (i > index) {
                        string segment = file[index..i].ToString().Trim();
                        if (!string.IsNullOrEmpty(segment)) {
                            bool inObject = contextStack.Count > 0 && contextStack.Peek();
                            ProcessSegment(segment, inObject, list);
                        }
                    }

                    list.Add((TextToken.StartArray, ""));
                    contextStack.Push(false); // Push array context
                    index = i + 1;
                    break;
                case ']':
                    // Process any content before the closing bracket
                    if (i > index) {
                        string segment = file[index..i].ToString().Trim();
                        if (!string.IsNullOrEmpty(segment)) {
                            bool inObject = contextStack.Count > 0 && contextStack.Peek();
                            ProcessSegment(segment, inObject, list);
                        }
                    }

                    list.Add((TextToken.EndArray, ""));
                    if (contextStack.Count > 0) contextStack.Pop(); // Pop array context
                    index = i + 1;
                    break;
                case ',':
                    // Process the content between the last index and this comma
                    if (i > index) {
                        string segment = file[index..i].ToString().Trim();
                        if (!string.IsNullOrEmpty(segment)) {
                            bool inObject = contextStack.Count > 0 && contextStack.Peek();
                            ProcessSegment(segment, inObject, list);
                        }
                    }

                    index = i + 1;
                    break;
                default:
                    index++;
                    break;
            }
        }

    }

    private static void ProcessSegment(string segment, bool inObject, List<(TextToken, string)> values) {
        // Only look for key-value pairs if we're inside an object
        if (inObject) {
            // Check if this segment contains a colon (key-value pair)
            int colonIndex = segment.IndexOf(':');
            if (colonIndex != -1) {
                // This is a key-value pair
                string key = segment[..colonIndex].Trim().Trim('"');
                string value = segment[(colonIndex + 1)..].Trim();

                values.Add((TextToken.KeyValue, key));

                // Only emit a primitive value if there's actually content after the colon
                // If the value is empty, it means the actual value is an array or object that follows
                if (!string.IsNullOrWhiteSpace(value)) {
                    value = value.Trim('"');
                    values.Add((TextToken.Primitive, value));

                }
                
                return;
            }
        }

        // This is a primitive value (either in an array or a value without a key in an object)
        string primitiveValue = segment.Trim('"');
        values.Add((TextToken.Primitive, primitiveValue));
    }

    private static (int index, char c) NextCharacter(ReadOnlySpan<char> file, int index) {
        int currentPos = index;

        while (currentPos < file.Length) {
            char currentChar = file[currentPos];

            // If we encounter a quote, skip everything until the closing quote
            if (currentChar == '"') {
                currentPos++; // Move past the opening quote
                // Find the closing quote
                while (currentPos < file.Length && file[currentPos] != '"') {
                    // Handle escaped quotes
                    if (file[currentPos] == '\\' && currentPos + 1 < file.Length) {
                        currentPos += 2; // Skip escaped character
                    } else {
                        currentPos++;
                    }
                }

                if (currentPos < file.Length) {
                    currentPos++; // Move past the closing quote
                }

                continue;
            }

            // Check if current character is one of our delimiter characters
            if (chars.Contains(currentChar)) {
                return (currentPos, currentChar);
            }

            currentPos++;
        }

        // If we reach here, no delimiter was found
        return (-1, '\0');
    }

    /// <summary>
    /// Get items from the internal computation.
    /// </summary>
    public IEnumerator<(TextToken, string)> GetEnumerator() {
        foreach ((TextToken, string) item in computation) {
            yield return item;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }
}
