using HowlDev.IO.Text.ConfigFile.Enums;
using HowlDev.IO.Text.ConfigFile.Interfaces;
using System.ComponentModel;

namespace HowlDev.IO.Text.ConfigFile.Primitives;

/// <summary>
/// Base abstract class that provides default InvalidOperationException implementations
/// for methods that are not supported by specific config option types.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public abstract class BaseConfigOption : IBaseConfigOption {
    /// <summary>
    /// Gets the name of the current type for use in exception messages.
    /// </summary>
    protected string TypeName => GetType().Name;

    #region Abstract members that must be implemented by derived classes
    /// <inheritdoc/>
    public abstract ConfigOptionType Type { get; }
    #endregion

    #region Virtual members with default InvalidOperationException implementations

    /// <inheritdoc/>
    public virtual IEnumerable<string> Keys =>
        throw new InvalidOperationException($"Key enumeration not allowed on type of {TypeName}.");

    /// <inheritdoc/>
    public virtual int Count =>
        throw new InvalidOperationException($"Count not allowed on type of {TypeName}.");

    /// <inheritdoc/>
    public virtual IEnumerable<IBaseConfigOption> Items =>
        throw new InvalidOperationException($"Item enumeration not allowed on type of {TypeName}.");

    /// <inheritdoc/>
    public virtual IBaseConfigOption this[string key] =>
        throw new InvalidOperationException($"Key indexing operation invalid on type of {TypeName}.");

    /// <inheritdoc/>
    public virtual IBaseConfigOption this[int index] =>
        throw new InvalidOperationException($"List indexing operation invalid on type of {TypeName}.");

    /// <inheritdoc/>
    public virtual bool TryGet(string key, out IBaseConfigOption value) {
        value = null!;
        throw new InvalidOperationException($"TryGet not allowed on type of {TypeName}.");
    }

    /// <inheritdoc/>
    public virtual bool Contains(string key) =>
        throw new InvalidOperationException($"Contains not allowed on type of {TypeName}.");

    #endregion

    #region IConvertible default implementations throwing InvalidOperationException

    /// <inheritdoc/>
    public virtual TypeCode GetTypeCode() =>
        throw new InvalidOperationException($"GetTypeCode not allowed on type of {TypeName}.");

    /// <inheritdoc/>
    public virtual bool ToBoolean(IFormatProvider? provider) =>
        throw new InvalidOperationException($"ToBoolean not allowed on type of {TypeName}.");

    /// <inheritdoc/>
    public virtual byte ToByte(IFormatProvider? provider) =>
        throw new InvalidOperationException($"ToByte not allowed on type of {TypeName}.");

    /// <inheritdoc/>
    public virtual char ToChar(IFormatProvider? provider) =>
        throw new InvalidOperationException($"ToChar not allowed on type of {TypeName}.");

    /// <inheritdoc/>
    public virtual DateTime ToDateTime(IFormatProvider? provider) =>
        throw new InvalidOperationException($"ToDateTime not allowed on type of {TypeName}.");

    /// <inheritdoc/>
    public virtual decimal ToDecimal(IFormatProvider? provider) =>
        throw new InvalidOperationException($"ToDecimal not allowed on type of {TypeName}.");

    /// <inheritdoc/>
    public virtual double ToDouble(IFormatProvider? provider) =>
        throw new InvalidOperationException($"ToDouble not allowed on type of {TypeName}.");

    /// <inheritdoc/>
    public virtual short ToInt16(IFormatProvider? provider) =>
        throw new InvalidOperationException($"ToInt16 not allowed on type of {TypeName}.");

    /// <inheritdoc/>
    public virtual int ToInt32(IFormatProvider? provider) =>
        throw new InvalidOperationException($"ToInt32 not allowed on type of {TypeName}.");

    /// <inheritdoc/>
    public virtual long ToInt64(IFormatProvider? provider) =>
        throw new InvalidOperationException($"ToInt64 not allowed on type of {TypeName}.");

    /// <inheritdoc/>
    public virtual sbyte ToSByte(IFormatProvider? provider) =>
        throw new InvalidOperationException($"ToSByte not allowed on type of {TypeName}.");

    /// <inheritdoc/>
    public virtual float ToSingle(IFormatProvider? provider) =>
        throw new InvalidOperationException($"ToSingle not allowed on type of {TypeName}.");

    /// <inheritdoc/>
    public virtual string ToString(IFormatProvider? provider) =>
        throw new InvalidOperationException($"ToString not allowed on type of {TypeName}.");

    /// <inheritdoc/>
    public virtual object ToType(Type conversionType, IFormatProvider? provider) =>
        throw new InvalidOperationException($"ToType not allowed on type of {TypeName}.");

    /// <inheritdoc/>
    public virtual ushort ToUInt16(IFormatProvider? provider) =>
        throw new InvalidOperationException($"ToUInt16 not allowed on type of {TypeName}.");

    /// <inheritdoc/>
    public virtual uint ToUInt32(IFormatProvider? provider) =>
        throw new InvalidOperationException($"ToUInt32 not allowed on type of {TypeName}.");

    /// <inheritdoc/>
    public virtual ulong ToUInt64(IFormatProvider? provider) =>
        throw new InvalidOperationException($"ToUInt64 not allowed on type of {TypeName}.");

    #endregion

    #region AsEnumerable default implementations throwing InvalidOperationException

    /// <inheritdoc/>
    public virtual IEnumerable<T> AsEnumerable<T>() =>
        throw new InvalidOperationException($"AsEnumerable not allowed on type of {TypeName}.");

    /// <inheritdoc/>
    public virtual IEnumerable<T> AsEnumerable<T>(OptionMappingOptions options) =>
        throw new InvalidOperationException($"AsEnumerable not allowed on type of {TypeName}.");

    /// <inheritdoc/>
    public virtual IEnumerable<T> AsStrictEnumerable<T>() =>
        throw new InvalidOperationException($"AsStrictEnumerable not allowed on type of {TypeName}.");

    #endregion

    #region As methods default implementations throwing InvalidOperationException

    /// <inheritdoc/>
    public virtual T As<T>() =>
        throw new InvalidOperationException($"As not allowed on type of {TypeName}.");

    /// <inheritdoc/>
    public virtual T As<T>(OptionMappingOptions option) =>
        throw new InvalidOperationException($"As not allowed on type of {TypeName}.");

    /// <inheritdoc/>
    public virtual T AsStrict<T>() =>
        throw new InvalidOperationException($"AsStrict not allowed on type of {TypeName}.");

    /// <inheritdoc/>
    public virtual T AsStrict<T>(OptionMappingOptions option) =>
        throw new InvalidOperationException($"AsStrict not allowed on type of {TypeName}.");

    /// <inheritdoc/>
    public virtual T AsConstructed<T>() =>
        throw new InvalidOperationException($"AsConstructed not allowed on type of {TypeName}.");

    /// <inheritdoc/>
    public virtual T AsStrictConstructed<T>() =>
        throw new InvalidOperationException($"AsStrictConstructed not allowed on type of {TypeName}.");

    /// <inheritdoc/>
    public virtual T AsProperties<T>() =>
        throw new InvalidOperationException($"AsProperties not allowed on type of {TypeName}.");

    /// <inheritdoc/>
    public virtual T AsStrictProperties<T>() =>
        throw new InvalidOperationException($"AsStrictProperties not allowed on type of {TypeName}.");

    #endregion
}
