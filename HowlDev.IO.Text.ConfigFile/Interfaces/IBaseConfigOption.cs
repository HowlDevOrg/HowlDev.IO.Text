using HowlDev.IO.Text.ConfigFile.Enums;
using System.ComponentModel;

namespace HowlDev.IO.Text.ConfigFile.Interfaces;
/// <summary/>
[EditorBrowsable(EditorBrowsableState.Never)]
public interface IBaseConfigOption : IBasePrimitiveOption, IBaseArrayOption, IBaseObjectOption {
    /// <summary>
    /// Type this object is (primitive, array, or object).
    /// </summary>
    ConfigOptionType Type { get; }
}