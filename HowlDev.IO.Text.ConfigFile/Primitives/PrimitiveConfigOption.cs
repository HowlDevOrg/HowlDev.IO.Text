using HowlDev.IO.Text.ConfigFile.Enums;
using System.ComponentModel;

namespace HowlDev.IO.Text.ConfigFile.Primitives;

/// <summary/>
[EditorBrowsable(EditorBrowsableState.Never)]
public class PrimitiveConfigOption : BaseConfigOption {
    private string value;

    /// <summary/>
    public override ConfigOptionType Type => ConfigOptionType.Primitive;

    /// <summary/>
    public PrimitiveConfigOption(string value) {
        this.value = value.Trim();
    }
    /// <summary/>
    public override string ToString() {
        return value;
    }

    /// <inheritdoc/>
    public override TypeCode GetTypeCode() => TypeCode.String;

    /// <inheritdoc/>
    public override bool ToBoolean(IFormatProvider? provider = null) {
        bool succeeded = bool.TryParse(value, out bool outValue);
        if (succeeded) {
            return outValue;
        } else {
            throw new InvalidCastException($"Value \"{value}\" is not castable to a Boolean.");
        }
    }

    /// <inheritdoc/>
    public override DateTime ToDateTime(IFormatProvider? provider) => DateTime.Parse(value);

    /// <inheritdoc/>
    public override decimal ToDecimal(IFormatProvider? provider) => (decimal)ToDouble();

    /// <inheritdoc/>
    public override double ToDouble(IFormatProvider? provider = null) {
        bool succeeded = double.TryParse(value, out double outValue);
        if (succeeded) {
            return outValue;
        } else {
            throw new InvalidCastException($"Value \"{value}\" is not castable to a Double.");
        }
    }

    /// <inheritdoc/>
    public override short ToInt16(IFormatProvider? provider) => (short)ToInt32();

    /// <inheritdoc/>
    public override int ToInt32(IFormatProvider? provider = null) {
        bool succeeded = int.TryParse(value, out int outValue);
        if (succeeded) {
            return outValue;
        } else {
            throw new InvalidCastException($"Value \"{value}\" is not castable to an Int.");
        }
    }

    /// <inheritdoc/>
    public override long ToInt64(IFormatProvider? provider) => ToInt32();

    /// <inheritdoc/>
    public override string ToString(IFormatProvider? provider = null) => value;

    /// <inheritdoc/>
    public override ushort ToUInt16(IFormatProvider? provider) => (ushort)ToInt32();

    /// <inheritdoc/>
    public override uint ToUInt32(IFormatProvider? provider) => (uint)ToInt32();

    /// <inheritdoc/>
    public override ulong ToUInt64(IFormatProvider? provider) => (ulong)ToInt32();

    /// <inheritdoc/>
    public override T As<T>() => (T)Convert.ChangeType(value.ToString(null), typeof(T));

}