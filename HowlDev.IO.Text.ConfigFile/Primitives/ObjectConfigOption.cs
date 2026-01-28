using HowlDev.IO.Text.ConfigFile.Enums;
using HowlDev.IO.Text.ConfigFile.Exceptions;
using HowlDev.IO.Text.ConfigFile.Interfaces;
using System.ComponentModel;

namespace HowlDev.IO.Text.ConfigFile.Primitives;

/// <summary/>
[EditorBrowsable(EditorBrowsableState.Never)]
public class ObjectConfigOption : IBaseConfigOption {
    private Dictionary<string, IBaseConfigOption> obj = new Dictionary<string, IBaseConfigOption>();
    private string resourcePath;

    /// <summary/>
    public ConfigOptionType Type => ConfigOptionType.Object;
    /// <summary/>
    public int Count => obj.Count;
    /// <summary/>
    public IEnumerable<IBaseConfigOption> Items => throw new InvalidOperationException("Item enumeration not allowed on type of ObjectConfigOption.");
    /// <summary/>
    public IEnumerable<string> Keys => obj.Keys;

    /// <summary/>
    public ObjectConfigOption(Dictionary<string, IBaseConfigOption> obj, string parentPath = "", string myPath = "") {
        this.obj = new(obj, StringComparer.OrdinalIgnoreCase);
        resourcePath = parentPath;
        if (myPath.Length > 0) resourcePath += "[" + myPath + "]";
    }
    /// <summary/>
    public IBaseConfigOption this[string key] {
        get {
            if (!obj.TryGetValue(key, out var value)) {
                string error = $"Object does not contain key \"{key}\".";
                if (resourcePath.Length >= 3) error += $"\n\tPath: {resourcePath}";
                throw new KeyNotFoundException(error);
            }

            return value;
        }
    }
    /// <summary/>
    public IBaseConfigOption this[int index] => throw new InvalidOperationException("Operation invalid on type of ObjectConfigOption.");

    /// <summary/>
    public bool TryGet(string key, out IBaseConfigOption value) {
        return obj.TryGetValue(key, out value!); // I'm just passing it through. 
    }

    /// <summary/>
    public bool Contains(string key) {
        return obj.ContainsKey(key);
    }

    /// <inheritdoc/>
    public TypeCode GetTypeCode() => throw new InvalidOperationException("GetTypeCode not allowed on type of ObjectConfigOption.");

    /// <inheritdoc/>
    public bool ToBoolean(IFormatProvider? provider) => throw new InvalidOperationException("ToBoolean not allowed on type of ObjectConfigOption.");

    /// <inheritdoc/>
    public byte ToByte(IFormatProvider? provider) => throw new InvalidOperationException("ToByte not allowed on type of ObjectConfigOption.");

    /// <inheritdoc/>
    public char ToChar(IFormatProvider? provider) => throw new InvalidOperationException("ToChar not allowed on type of ObjectConfigOption.");

    /// <inheritdoc/>
    public DateTime ToDateTime(IFormatProvider? provider) => throw new InvalidOperationException("ToDateTime not allowed on type of ObjectConfigOption.");

    /// <inheritdoc/>
    public decimal ToDecimal(IFormatProvider? provider) => throw new InvalidOperationException("ToDecimal not allowed on type of ObjectConfigOption.");

    /// <inheritdoc/>
    public double ToDouble(IFormatProvider? provider) => throw new InvalidOperationException("ToDouble not allowed on type of ObjectConfigOption.");

    /// <inheritdoc/>
    public short ToInt16(IFormatProvider? provider) => throw new InvalidOperationException("ToInt16 not allowed on type of ObjectConfigOption.");

    /// <inheritdoc/>
    public int ToInt32(IFormatProvider? provider) => throw new InvalidOperationException("ToInt32 not allowed on type of ObjectConfigOption.");

    /// <inheritdoc/>
    public long ToInt64(IFormatProvider? provider) => throw new InvalidOperationException("ToInt64 not allowed on type of ObjectConfigOption.");

    /// <inheritdoc/>
    public sbyte ToSByte(IFormatProvider? provider) => throw new InvalidOperationException("ToSByte not allowed on type of ObjectConfigOption.");

    /// <inheritdoc/>
    public float ToSingle(IFormatProvider? provider) => throw new InvalidOperationException("ToSingle not allowed on type of ObjectConfigOption.");

    /// <inheritdoc/>
    public string ToString(IFormatProvider? provider) => throw new InvalidOperationException("ToString not allowed on type of ObjectConfigOption.");

    /// <inheritdoc/>
    public object ToType(Type conversionType, IFormatProvider? provider) => throw new InvalidOperationException("ToType not allowed on type of ObjectConfigOption.");

    /// <inheritdoc/>
    public ushort ToUInt16(IFormatProvider? provider) => throw new InvalidOperationException("ToUInt16 not allowed on type of ObjectConfigOption.");

    /// <inheritdoc/>
    public uint ToUInt32(IFormatProvider? provider) => throw new InvalidOperationException("ToUInt32 not allowed on type of ObjectConfigOption.");

    /// <inheritdoc/>
    public ulong ToUInt64(IFormatProvider? provider) => throw new InvalidOperationException("ToUInt64 not allowed on type of ObjectConfigOption.");

    /// <inheritdoc/>
    public IEnumerable<T> AsEnumerable<T>() => throw new InvalidOperationException("AsEnumerable not allowed on type of ObjectConfigOption.");

    /// <inheritdoc/>
    public IEnumerable<T> AsEnumerable<T>(OptionMappingOptions options) => throw new InvalidOperationException("AsEnumerable not allowed on type of ObjectConfigOption.");

    /// <inheritdoc/>
    public IEnumerable<T> AsStrictEnumerable<T>() => throw new InvalidOperationException("AsEnumerable not allowed on type of ObjectConfigOption.");

    /// <inheritdoc/>
    public T As<T>() {
        return Map<T>(new OptionMappingOptions() { UseProperties = true, UseConstructors = true }, Contains, this);
    }

    /// <inheritdoc/>
    public T As<T>(OptionMappingOptions option) {
        return Map<T>(option, Contains, this);
    }

    /// <inheritdoc/>
    public T AsStrict<T>() {
        return Map<T>(new OptionMappingOptions() { UseProperties = true, UseConstructors = true, StrictMatching = true }, Contains, this);
    }

    /// <inheritdoc/>
    public T AsStrict<T>(OptionMappingOptions option) {
        return Map<T>(new OptionMappingOptions(option) { StrictMatching = true }, Contains, this);
    }

    /// <inheritdoc/>
    public T AsConstructed<T>() {
        return Map<T>(new OptionMappingOptions() { UseConstructors = true }, Contains, this);
    }

    /// <inheritdoc/>
    public T AsStrictConstructed<T>() {
        return Map<T>(new OptionMappingOptions() { UseConstructors = true, StrictMatching = true }, Contains, this);
    }

    /// <inheritdoc/>
    public T AsProperties<T>() {
        return Map<T>(new OptionMappingOptions() { UseProperties = true }, Contains, this);
    }

    /// <inheritdoc/>
    public T AsStrictProperties<T>() {
        return Map<T>(new OptionMappingOptions() { UseProperties = true, StrictMatching = true }, Contains, this);
    }

    /// <summary>
    /// Defined in the ObjectConfigOption class for taking in an object and returning an object
    /// of that type. Pass in a function for the ContainsKey for parameters and the config option 
    /// to work on.
    /// </summary>
    /// <exception cref="StrictMappingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    internal static T Map<T>(OptionMappingOptions options, Func<string, bool> func, IBaseConfigOption option) {
        if (options.UseConstructors) {
            var ctors = typeof(T).GetConstructors();

            if (options.UseProperties) {
                ctors = [.. ctors.Where(p => p.GetParameters().Length > 0)];
            }

            if (options.StrictMatching) {
                ctors = [.. ctors.Where(p => p.GetParameters().Length == option.Count)];
            }

            foreach (var ctor in ctors.OrderByDescending(c => c.GetParameters().Length)) {
                var parameters = ctor.GetParameters();
                bool canCreate = parameters.All(p => func(p.Name!));

                if (canCreate) {
                    var args = parameters
                        .Select(p => Convert.ChangeType(option[p.Name!], p.ParameterType))
                        .ToArray();

                    return (T)ctor.Invoke(args);
                }
            }

            if (options.StrictMatching && !options.UseProperties) {
                throw new StrictMappingException(
                    $"""
                    No suitable constructor found for {typeof(T).Name}. Consider removing the StrictMatching flag. 
                    Tried to find a constructor that matched the following keys: {string.Join(", ", option.Keys.ToArray())}.
                    """
                );
            }

            if (!options.UseProperties) {
                throw new InvalidOperationException(
                    $"""
                    No suitable constructor found for {typeof(T).Name}. 
                    Tried to find a constructor that matched the following keys: {string.Join(", ", option.Keys.ToArray())}.
                    """
                );
            }
        }

        if (options.UseProperties) {
            if (typeof(T).GetConstructor(System.Type.EmptyTypes) == null) {
                throw new InvalidOperationException(
                    $"Type {typeof(T).Name} must have a parameterless constructor to use property mapping."
                );
            }

            T instance = Activator.CreateInstance<T>()!;
            var properties = typeof(T).GetProperties().Where(p => p.CanWrite);

            if (options.StrictMatching) {
                if (properties.Count() != option.Count) {
                    throw new StrictMappingException(
                        $"""
                        Property count mismatch for {typeof(T).Name}. Consider removing the StrictMatching flag. 
                        Property count of type: {properties.Count()}. Key count of object: {option.Count}.
                        """
                    );
                }
            }

            foreach (var prop in properties) {
                if (option.TryGet(prop.Name, out var value)) {
                    prop.SetValue(instance, Convert.ChangeType(value, prop.PropertyType));
                } else if (options.StrictMatching) {
                    throw new StrictMappingException(
                        $"""
                        Property mismatch for {typeof(T).Name}. Consider removing the StrictMatching flag. 
                        Could not find matching object key for property: {prop.Name}.
                        """
                    );
                }
            }

            return instance;
        }

        throw new InvalidOperationException(
            $"Was not able to construct object for {typeof(T).Name}."
        );
    }
}