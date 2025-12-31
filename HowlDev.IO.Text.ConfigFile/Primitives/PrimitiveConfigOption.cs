using HowlDev.IO.Text.ConfigFile.Enums;
using HowlDev.IO.Text.ConfigFile.Interfaces;
using System.ComponentModel;

namespace HowlDev.IO.Text.ConfigFile.Primitives;

/// <summary/>
[EditorBrowsable(EditorBrowsableState.Never)]
public class PrimitiveConfigOption : IBaseConfigOption {
    private string value;

    /// <summary/>
    public ConfigOptionType Type => ConfigOptionType.Primitive;

    /// <summary/>
    public IEnumerable<string> Keys => throw new InvalidOperationException("Key enumeration not allowed on type of PrimitiveConfigOption.");
    /// <summary/>
    public int Count => throw new InvalidOperationException("Count not allowed on type of PrimitiveConfigOption.");
    /// <summary/>
    public IEnumerable<IBaseConfigOption> Items => throw new InvalidOperationException("Item enumeration not allowed on type of PrimitiveConfigOption.");

    /// <summary/>
    public PrimitiveConfigOption(string value) {
        this.value = value.Trim();
    }
    /// <summary/>
    public IBaseConfigOption this[string key] => throw new InvalidOperationException("Key indexing operation invalid on type of PrimitiveConfigOption.");
    /// <summary/>
    public IBaseConfigOption this[int index] => throw new InvalidOperationException("List indexing operation invalid on type of PrimitiveConfigOption.");
    /// <summary/>
    public override string ToString() {
        return value;
    }

    /// <summary/>
    public bool TryGet(string key, out IBaseConfigOption value) => throw new InvalidOperationException("TryGet not allowed on type PrimitiveConfigOption");

    /// <summary/>
    public bool Contains(string key) => throw new InvalidOperationException("Contains not allowed on type PrimitiveConfigOption");

    /// <inheritdoc/>
    public TypeCode GetTypeCode() => TypeCode.String;

    /// <inheritdoc/>
    public bool ToBoolean(IFormatProvider? provider = null) {
        bool succeeded = bool.TryParse(value, out bool outValue);
        if (succeeded) {
            return outValue;
        } else {
            throw new InvalidCastException($"Value \"{value}\" is not castable to a Boolean.");
        }
    }

    /// <inheritdoc/>
    public byte ToByte(IFormatProvider? provider) => throw new InvalidOperationException("ToByte not supported on type PrimitiveConfigOption");

    /// <inheritdoc/>
    public char ToChar(IFormatProvider? provider) => throw new InvalidOperationException("ToChar not supported on type PrimitiveConfigOption");

    /// <inheritdoc/>
    public DateTime ToDateTime(IFormatProvider? provider) => DateTime.Parse(value);

    /// <inheritdoc/>
    public decimal ToDecimal(IFormatProvider? provider) => (decimal)ToDouble();

    /// <inheritdoc/>
    public double ToDouble(IFormatProvider? provider = null) {
        bool succeeded = double.TryParse(value, out double outValue);
        if (succeeded) {
            return outValue;
        } else {
            throw new InvalidCastException($"Value \"{value}\" is not castable to a Double.");
        }
    }

    /// <inheritdoc/>
    public short ToInt16(IFormatProvider? provider) => (short)ToInt32();

    /// <inheritdoc/>
    public int ToInt32(IFormatProvider? provider = null) {
        bool succeeded = int.TryParse(value, out int outValue);
        if (succeeded) {
            return outValue;
        } else {
            throw new InvalidCastException($"Value \"{value}\" is not castable to an Int.");
        }
    }

    /// <inheritdoc/>
    public long ToInt64(IFormatProvider? provider) => ToInt32();

    /// <inheritdoc/>
    public sbyte ToSByte(IFormatProvider? provider) => throw new InvalidOperationException("ToSByte not supported on type PrimitiveConfigOption");

    /// <inheritdoc/>
    public float ToSingle(IFormatProvider? provider) => throw new InvalidOperationException("ToSingle not supported on type PrimitiveConfigOption");

    /// <inheritdoc/>
    public string ToString(IFormatProvider? provider = null) => value;

    /// <inheritdoc/>
    public object ToType(Type conversionType, IFormatProvider? provider) => throw new InvalidOperationException("ToType not supported on type PrimitiveConfigOption");

    /// <inheritdoc/>
    public ushort ToUInt16(IFormatProvider? provider) => (ushort)ToInt32();

    /// <inheritdoc/>
    public uint ToUInt32(IFormatProvider? provider) => (uint)ToInt32();

    /// <inheritdoc/>
    public ulong ToUInt64(IFormatProvider? provider) => (ulong)ToInt32();

    /// <inheritdoc/>
    public T As<T>() => (T)Convert.ChangeType(value.ToString(null), typeof(T));

    /// <inheritdoc/>
    public T As<T>(OptionMappingOptions option) => throw new InvalidOperationException("Reflection not supported on type PrimitiveConfigOption");

    /// <inheritdoc/>
    public T AsStrict<T>() => throw new InvalidOperationException("Reflection not supported on type PrimitiveConfigOption");

    /// <inheritdoc/>
    public T AsStrict<T>(OptionMappingOptions option) => throw new InvalidOperationException("Reflection not supported on type PrimitiveConfigOption");

    /// <inheritdoc/>
    public T AsConstructed<T>() => throw new InvalidOperationException("Reflection not supported on type PrimitiveConfigOption");

    /// <inheritdoc/>
    public T AsStrictConstructed<T>() => throw new InvalidOperationException("Reflection not supported on type PrimitiveConfigOption");

    /// <inheritdoc/>
    public T AsProperties<T>() => throw new InvalidOperationException("Reflection not supported on type PrimitiveConfigOption");

    /// <inheritdoc/>
    public T AsStrictProperties<T>() => throw new InvalidOperationException("Reflection not supported on type PrimitiveConfigOption");

    /// <inheritdoc/>
    public IEnumerable<T> AsEnumerable<T>() => throw new InvalidOperationException("AsEnumerable not supported on type PrimitiveConfigOption");

    /// <inheritdoc/>
    public IEnumerable<T> AsEnumerable<T>(OptionMappingOptions options) => throw new InvalidOperationException("AsEnumerable not supported on type PrimitiveConfigOption");

    /// <inheritdoc/>
    public IEnumerable<T> AsStrictEnumerable<T>() => throw new InvalidOperationException("AsEnumerable not supported on type PrimitiveConfigOption");
}