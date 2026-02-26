using HowlDev.IO.Text.ConfigFile.Enums;
using HowlDev.IO.Text.ConfigFile.Exceptions;
using HowlDev.IO.Text.ConfigFile.Interfaces;
using System.Collections.Frozen;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace HowlDev.IO.Text.ConfigFile.Primitives;

/// <summary/>
[EditorBrowsable(EditorBrowsableState.Never)]
public class ObjectConfigOption : BaseConfigOption {
    private FrozenDictionary<string, IBaseConfigOption> obj;
    private string resourcePath;

    /// <summary/>
    public override ConfigOptionType Type => ConfigOptionType.Object;
    /// <summary/>
    public override int Count => obj.Count;
    /// <summary/>
    public override IEnumerable<string> Keys => obj.Keys;

    /// <summary/>
    public ObjectConfigOption(Dictionary<string, IBaseConfigOption> obj, string parentPath = "", string myPath = "") {
        StringComparer comparer = new();
        this.obj = obj.ToFrozenDictionary(comparer);
        resourcePath = parentPath;
        if (myPath.Length > 0) resourcePath += "[" + myPath + "]";
    }
    /// <summary/>
    public override IBaseConfigOption this[string key] {
        get {
            if (!obj.TryGetValue(key, out IBaseConfigOption? value)) {
                string error = $"Object does not contain key \"{key}\".";
                if (resourcePath.Length >= 3) error += $"\n\tPath: {resourcePath}";
                throw new KeyNotFoundException(error);
            }

            return value;
        }
    }

    /// <summary/>
    public override bool TryGet(string key, out IBaseConfigOption value) {
        return obj.TryGetValue(key, out value!); // I'm just passing it through. 
    }

    /// <summary/>
    public override bool Contains(string key) {
        return obj.ContainsKey(key);
    }



    /// <inheritdoc/>
    public override T As<T>() {
        return Map<T>(new OptionMappingOptions() { UseProperties = true, UseConstructors = true }, Contains, this);
    }

    /// <inheritdoc/>
    public override T As<T>(OptionMappingOptions option) {
        return Map<T>(option, Contains, this);
    }

    /// <inheritdoc/>
    public override T AsStrict<T>() {
        return Map<T>(new OptionMappingOptions() { UseProperties = true, UseConstructors = true, StrictMatching = true }, Contains, this);
    }

    /// <inheritdoc/>
    public override T AsStrict<T>(OptionMappingOptions option) {
        return Map<T>(new OptionMappingOptions(option) { StrictMatching = true }, Contains, this);
    }

    /// <inheritdoc/>
    public override T AsConstructed<T>() {
        return Map<T>(new OptionMappingOptions() { UseConstructors = true }, Contains, this);
    }

    /// <inheritdoc/>
    public override T AsStrictConstructed<T>() {
        return Map<T>(new OptionMappingOptions() { UseConstructors = true, StrictMatching = true }, Contains, this);
    }

    /// <inheritdoc/>
    public override T AsProperties<T>() {
        return Map<T>(new OptionMappingOptions() { UseProperties = true }, Contains, this);
    }

    /// <inheritdoc/>
    public override T AsStrictProperties<T>() {
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
            ConstructorInfo[] ctors = typeof(T).GetConstructors();

            if (options.UseProperties) {
                ctors = [.. ctors.Where(p => p.GetParameters().Length > 0)];
            }

            if (options.StrictMatching) {
                ctors = [.. ctors.Where(p => p.GetParameters().Length == option.Count)];
            }

            foreach (ConstructorInfo? ctor in ctors.OrderByDescending(c => c.GetParameters().Length)) {
                ParameterInfo[] parameters = ctor.GetParameters();
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
            IEnumerable<PropertyInfo> properties = typeof(T).GetProperties().Where(p => p.CanWrite);

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

            foreach (PropertyInfo? prop in properties) {
                if (option.TryGet(prop.Name, out IBaseConfigOption? value)) {
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

    private class StringComparer : IEqualityComparer<string> {
        public bool Equals(string? x, string? y) {
            return x?.ToLower() == y?.ToLower();
        }

        public int GetHashCode([DisallowNull] string obj) {
            return obj.ToLower().GetHashCode();
        }
    }
}
