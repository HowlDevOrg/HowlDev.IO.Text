using HowlDev.IO.Text.Parsers.Enums;
using System.Collections;

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
        foreach (var item in ParseFileContents(fileValue)) yield return item;
    }



    private static IEnumerable<(TextToken, string)> ParseFileContents(string file) {
        int index = 0;
        while (index < file.Length) {
            (int i, char c) = NextCharacter(file, index);
            switch (c) {
                case '{':
                    yield return (TextToken.StartObject, "");
                    index = i + 1;
                    break;
                case '}':
                    // Process any content before the closing brace
                    if (i > index) {
                        string segment = file.Substring(index, i - index).Trim();
                        if (!string.IsNullOrEmpty(segment)) {
                            foreach (var item in ProcessSegment(segment)) yield return item;
                        }
                    }
                    yield return (TextToken.EndObject, "");
                    index = i + 1;
                    break;
                case '[':
                    yield return (TextToken.StartArray, "");
                    index = i + 1;
                    break;
                case ']':
                    // Process any content before the closing bracket
                    if (i > index) {
                        string segment = file.Substring(index, i - index).Trim();
                        if (!string.IsNullOrEmpty(segment)) {
                            foreach (var item in ProcessSegment(segment)) yield return item;
                        }
                    }
                    yield return (TextToken.EndArray, "");
                    index = i + 1;
                    break;
                case ',':
                    // Process the content between the last index and this comma
                    if (i > index) {
                        string segment = file.Substring(index, i - index).Trim();
                        if (!string.IsNullOrEmpty(segment)) {
                            foreach (var item in ProcessSegment(segment)) yield return item;
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

    private static IEnumerable<(TextToken, string)> ProcessSegment(string segment) {
        // Check if this segment contains a colon (key-value pair)
        int colonIndex = segment.IndexOf(':');
        if (colonIndex != -1) {
            // This is a key-value pair
            string key = segment.Substring(0, colonIndex).Trim().Trim('"');
            string value = segment.Substring(colonIndex + 1).Trim().Trim('"');
            yield return (TextToken.KeyValue, key);
            yield return (TextToken.Primitive, value);
        } else {
            // This is a primitive value
            string value = segment.Trim('"');
            yield return (TextToken.Primitive, value);
        }
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