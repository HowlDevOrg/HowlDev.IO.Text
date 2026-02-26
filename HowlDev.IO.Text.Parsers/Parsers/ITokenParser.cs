using HowlDev.IO.Text.Parsers.Enums;
#pragma warning disable IDE0130
namespace HowlDev.IO.Text.Parsers;

/// <summary>
/// Returns tokens following the format listed in the README.
/// </summary>
public interface ITokenParser : IEnumerable<(TextToken, string)>;
