using HowlDev.IO.Text.Parsers.Enums;
using System.Collections;

#pragma warning disable IDE0130
namespace HowlDev.IO.Text.Parsers;

/// <summary/>
public class JSONParser(string file) : ITokenParser {
    private static char[] chars = ['{', '}', '[', ']', ','];
    /// <summary/>
    public IEnumerator<(TextToken, string)> GetEnumerator() {
        string fileValue = file.Replace('\r', ' ').Replace('\n', ' ');

        if (!file.StartsWith("[") && !file.StartsWith("{")) {
            throw new InvalidDataException("JSON file must start with either [ or {");
        }

        foreach ((TextToken, string) item in ParseFileContents(fileValue)) yield return item;
    }



    private static IEnumerable<(TextToken, string)> ParseFileContents(string file) {
        int index = 0;
        Stack<bool> contextStack = new(); // true = in object, false = in array

        while (index < file.Length) {
            (int i, char c) = NextCharacter(file, index);
            switch (c) {
                case '{':
                    // Process any content before the opening brace (could be a key)
                    if (i > index) {
                        string segment = file.Substring(index, i - index).Trim();
                        if (!string.IsNullOrEmpty(segment)) {
                            bool inObject = contextStack.Count > 0 && contextStack.Peek();
                            foreach ((TextToken, string) item in ProcessSegment(segment, inObject)) yield return item;
                        }
                    }

                    yield return (TextToken.StartObject, "");
                    contextStack.Push(true); // Push object context
                    index = i + 1;
                    break;
                case '}':
                    // Process any content before the closing brace
                    if (i > index) {
                        string segment = file.Substring(index, i - index).Trim();
                        if (!string.IsNullOrEmpty(segment)) {
                            bool inObject = contextStack.Count > 0 && contextStack.Peek();
                            foreach ((TextToken, string) item in ProcessSegment(segment, inObject)) yield return item;
                        }
                    }

                    yield return (TextToken.EndObject, "");
                    if (contextStack.Count > 0) contextStack.Pop(); // Pop object context
                    index = i + 1;
                    break;
                case '[':
                    // Process any content before the opening bracket (could be a key)
                    if (i > index) {
                        string segment = file.Substring(index, i - index).Trim();
                        if (!string.IsNullOrEmpty(segment)) {
                            bool inObject = contextStack.Count > 0 && contextStack.Peek();
                            foreach ((TextToken, string) item in ProcessSegment(segment, inObject)) yield return item;
                        }
                    }

                    yield return (TextToken.StartArray, "");
                    contextStack.Push(false); // Push array context
                    index = i + 1;
                    break;
                case ']':
                    // Process any content before the closing bracket
                    if (i > index) {
                        string segment = file.Substring(index, i - index).Trim();
                        if (!string.IsNullOrEmpty(segment)) {
                            bool inObject = contextStack.Count > 0 && contextStack.Peek();
                            foreach ((TextToken, string) item in ProcessSegment(segment, inObject)) yield return item;
                        }
                    }

                    yield return (TextToken.EndArray, "");
                    if (contextStack.Count > 0) contextStack.Pop(); // Pop array context
                    index = i + 1;
                    break;
                case ',':
                    // Process the content between the last index and this comma
                    if (i > index) {
                        string segment = file.Substring(index, i - index).Trim();
                        if (!string.IsNullOrEmpty(segment)) {
                            bool inObject = contextStack.Count > 0 && contextStack.Peek();
                            foreach ((TextToken, string) item in ProcessSegment(segment, inObject)) yield return item;
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

    private static IEnumerable<(TextToken, string)> ProcessSegment(string segment, bool inObject) {
        // Only look for key-value pairs if we're inside an object
        if (inObject) {
            // Check if this segment contains a colon (key-value pair)
            int colonIndex = segment.IndexOf(':');
            if (colonIndex != -1) {
                // This is a key-value pair
                string key = segment.Substring(0, colonIndex).Trim().Trim('"');
                string value = segment.Substring(colonIndex + 1).Trim();

                yield return (TextToken.KeyValue, key);

                // Only emit a primitive value if there's actually content after the colon
                // If the value is empty, it means the actual value is an array or object that follows
                if (!string.IsNullOrWhiteSpace(value)) {
                    value = value.Trim('"');
                    yield return (TextToken.Primitive, value);
                }

                yield break;
            }
        }

        // This is a primitive value (either in an array or a value without a key in an object)
        string primitiveValue = segment.Trim('"');
        yield return (TextToken.Primitive, primitiveValue);
    }

    private static (int index, char c) NextCharacter(string file, int index) {
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

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }
}
