using HowlDev.IO.Text.ConfigFile.Enums;
using HowlDev.IO.Text.ConfigFile.Interfaces;
using HowlDev.IO.Text.ConfigFile.Primitives;
using HowlDev.IO.Text.Parsers;
using HowlDev.IO.Text.Parsers.Enums;

namespace HowlDev.IO.Text.ConfigFile;

/// <summary>
/// This config file takes in a file path and reads it as either a JSON, YAML, or TXT file.
/// </summary>
public class TextConfigFile : IBaseConfigOption {
    private IBaseConfigOption option;

    #region Option Exports
    /// <summary/>
    public ConfigOptionType Type => option.Type;
    /// <summary/>
    public int Count => option.Count;
    /// <summary/>
    public IEnumerable<IBaseConfigOption> Items => option.Items;
    /// <summary/>
    public IEnumerable<string> Keys => option.Keys;

    /// <summary/>
    public IBaseConfigOption this[string key] => option[key];
    /// <summary/>
    public IBaseConfigOption this[int index] => option[index];

    /// <summary/>
    public bool TryGet(string key, out IBaseConfigOption value) => option.TryGet(key, out value);
    /// <summary/>
    public bool Contains(string key) => option.Contains(key);
    /// <inheritdoc/>
    public TypeCode GetTypeCode() => option.GetTypeCode();
    /// <inheritdoc/>
    public bool ToBoolean(IFormatProvider? provider) => option.ToBoolean(provider);
    /// <inheritdoc/>
    public byte ToByte(IFormatProvider? provider) => option.ToByte(provider);
    /// <inheritdoc/>
    public char ToChar(IFormatProvider? provider) => option.ToChar(provider);
    /// <inheritdoc/>
    public DateTime ToDateTime(IFormatProvider? provider) => option.ToDateTime(provider);
    /// <inheritdoc/>
    public decimal ToDecimal(IFormatProvider? provider) => option.ToDecimal(provider);
    /// <inheritdoc/>
    public double ToDouble(IFormatProvider? provider) => option.ToDouble(provider);
    /// <inheritdoc/>
    public short ToInt16(IFormatProvider? provider) => option.ToInt16(provider);
    /// <inheritdoc/>
    public int ToInt32(IFormatProvider? provider) => option.ToInt32(provider);
    /// <inheritdoc/>
    public long ToInt64(IFormatProvider? provider) => option.ToInt64(provider);
    /// <inheritdoc/>
    public sbyte ToSByte(IFormatProvider? provider) => option.ToSByte(provider);
    /// <inheritdoc/>
    public float ToSingle(IFormatProvider? provider) => option.ToSingle(provider);
    /// <inheritdoc/>
    public string ToString(IFormatProvider? provider) => option.ToString(provider);
    /// <inheritdoc/>
    public object ToType(Type conversionType, IFormatProvider? provider) => option.ToType(conversionType, provider);
    /// <inheritdoc/>
    public ushort ToUInt16(IFormatProvider? provider) => option.ToUInt16(provider);
    /// <inheritdoc/>
    public uint ToUInt32(IFormatProvider? provider) => option.ToUInt32(provider);
    /// <inheritdoc/>
    public ulong ToUInt64(IFormatProvider? provider) => option.ToUInt64(provider);

    /// <inheritdoc/>
    public T As<T>() => option.As<T>();
    /// <inheritdoc/>
    public T As<T>(OptionMappingOptions options) => option.As<T>(options);
    /// <inheritdoc/>
    public T AsStrict<T>() => option.AsStrict<T>();
    /// <inheritdoc/>
    public T AsStrict<T>(OptionMappingOptions options) => option.AsStrict<T>(options);
    /// <inheritdoc/>
    public T AsConstructed<T>() => option.AsConstructed<T>();
    /// <inheritdoc/>
    public T AsStrictConstructed<T>() => option.AsStrictConstructed<T>();
    /// <inheritdoc/>
    public T AsProperties<T>() => option.AsProperties<T>();
    /// <inheritdoc/>
    public T AsStrictProperties<T>() => option.AsStrictProperties<T>();
    /// <inheritdoc/>
    public IEnumerable<T> AsEnumerable<T>() => option.AsEnumerable<T>();
    /// <inheritdoc/>
    public IEnumerable<T> AsEnumerable<T>(OptionMappingOptions options) => option.AsEnumerable<T>(options);
    #endregion

    private List<string> acceptedExtensions = [".txt", ".yml", ".yaml", ".json"];

    private TextConfigFile() {
        option = new PrimitiveConfigOption("");
    }

    /// <summary/>
    public TextConfigFile(string filePath) {
        string extension = Path.GetExtension(filePath);
        if (!acceptedExtensions.Contains(extension)) {
            throw new FormatException($"Extension not recognized: {extension}");
        }

        string file = File.ReadAllText(filePath);
        switch (extension) {
            case ".txt":
                option = ConvertTokenStreamToConfigOption(new TXTParser(file));
                return;
            case ".yaml":
            case ".yml":
                option = ConvertTokenStreamToConfigOption(new YAMLParser(file));
                break;
            case ".json":
                option = ConvertTokenStreamToConfigOption(new JSONParser(file));
                break;
            default: throw new Exception("Extension error. Should be added to the acceptedExtensions inner array. Extension was not recognized.");
        }
    }

    /// <summary>
    /// Get a configuration file without the file system through this method. <br/>
    /// If you need an option that just reads all lines as a single primitive, use
    /// the YAML type. Otherwise, pick the type that best fits the format.
    /// </summary>
    /// <param name="type">File type to parse</param>
    /// <param name="fileValue">File string</param>
    public static TextConfigFile ReadTextAs(FileTypes type, string fileValue) {
        TextConfigFile file = new TextConfigFile();
        switch (type) {
            case FileTypes.TXT: file.option = ConvertTokenStreamToConfigOption(new TXTParser(fileValue)); break;
            case FileTypes.YAML: file.option = ConvertTokenStreamToConfigOption(new YAMLParser(fileValue)); break;
            case FileTypes.JSON: file.option = ConvertTokenStreamToConfigOption(new JSONParser(fileValue)); break;
        }
        return file;
    }

    /// <summary>
    /// This is the internal tool that creates the configuration, but I've decided to expose it if 
    /// you create your own parser functions. Generally, you don't need to worry about 
    /// this at all, but if you do, just provide an enumerable of tokens. 
    /// You can use this to check your parser.
    /// </summary>
    /// <param name="func">Object that implements <c>TokenParser</c> (IEnumerable&lt;(TextToken, string)&gt;).</param>
    /// <returns><see cref="IBaseConfigOption"/></returns>
    public static IBaseConfigOption ConvertTokenStreamToConfigOption(ITokenParser func) {
        var stack = new Stack<Frame>();
        stack.Push(new Frame(FrameKind.Root));

        foreach (var (type, value) in func) {
            var frame = stack.Peek();
            switch (type) {
                case TextToken.StartObject:
                    stack.Push(new Frame(FrameKind.Object));
                    break;
                case TextToken.EndObject:
                    var obj = stack.Pop();
                    if (obj.Kind != FrameKind.Object) {
                        throw new InvalidOperationException("Cannot close an Array object with an EndObject token.");
                    }
                    var parent = stack.Peek();
                    parent.Add(obj.AsOption());
                    break;
                case TextToken.StartArray:
                    stack.Push(new Frame(FrameKind.Array));
                    break;
                case TextToken.EndArray:
                    var arr = stack.Pop();
                    if (arr.Kind != FrameKind.Array) {
                        throw new InvalidOperationException("Cannot close an Object object with an EndArray token.");
                    }
                    parent = stack.Peek();
                    parent.Add(arr.AsOption());
                    break;
                case TextToken.KeyValue:
                    frame.PendingKey = value;
                    break;
                case TextToken.Primitive:
                    frame.Add(new PrimitiveConfigOption(value));
                    break;
                case TextToken.Comment:
                    break;
            }
        }

        Frame root = stack.Pop();
        if (stack.Count > 0) {
            throw new InvalidOperationException($"Not all objects were closed. Stack count post-parse: {stack.Count}");
        }
        return root.AsOption();
    }
}