using HowlDev.IO.Text.ConfigFile.Exceptions;
using System.ComponentModel;

namespace HowlDev.IO.Text.ConfigFile.Interfaces;
/// <summary/>
[EditorBrowsable(EditorBrowsableState.Never)]
public interface IBaseObjectOption {
    /// <summary/>
    IBaseConfigOption this[string key] { get; }
    /// <summary/>
    IEnumerable<string> Keys { get; }
    /// <summary/>
    bool TryGet(string key, out IBaseConfigOption value);
    /// <summary/>
    bool Contains(string key);
    /// <summary>
    /// Defaults to attempt a constructor call first, and if that fails, uses a parameterless constructor and 
    /// fills in any available properties. <br/>
    /// For Primitives, also allows you to pass in primitive types, such as <code>option.As&lt;int&gt;()</code>
    /// to quickly return a primitive type. This is also done internally for the AsEnumerable function.
    /// </summary>
    /// <exception cref="InvalidOperationException"/>
    T As<T>();
    /// <summary>
    /// Takes in an <see cref="OptionMappingOptions"/> type through to the inner Map function. 
    /// </summary>
    /// <exception cref="InvalidOperationException"/>
    /// <exception cref="StrictMappingException"/>
    T As<T>(OptionMappingOptions option);
    /// <summary>
    /// Defaults to attempt a constructor call first, and if that fails, uses a parameterless constructor and 
    /// fills in any available properties. <br/>
    /// Will strictly test for the number of values in the object and the constructor and/or number of writable 
    /// properties in the passed in type. 
    /// </summary>
    /// <exception cref="InvalidOperationException"/>
    /// <exception cref="StrictMappingException"/>
    T AsStrict<T>();
    /// <summary>
    /// Defaults to attempt a constructor call first, and if that fails, uses a parameterless constructor and 
    /// fills in any available properties. <br/>
    /// Will strictly test for the number of values in the object and the constructor and/or number of writable 
    /// properties in the passed in type. 
    /// </summary>
    /// <exception cref="InvalidOperationException"/>
    /// <exception cref="StrictMappingException"/>
    T AsStrict<T>(OptionMappingOptions option);
    /// <summary>
    /// Uses constructors to build an object. It sorts by descending length of parameters 
    /// and uses the first that the object satisfies all values.
    /// </summary>
    /// <exception cref="InvalidOperationException"/>
    T AsConstructed<T>();
    /// <summary>
    /// Uses constructors to build an object. 
    /// It uses Strict matching, so it only checks constructors that have the same length 
    /// as the object this contains.
    /// </summary>
    /// <exception cref="InvalidOperationException"/>
    /// <exception cref="StrictMappingException"/>
    T AsStrictConstructed<T>();
    /// <summary>
    /// Uses parameters to build an object. It requires a parameterless constructor to be available 
    /// on the type. 
    /// </summary>
    /// <exception cref="InvalidOperationException"/>
    T AsProperties<T>();
    /// <summary>
    /// Uses parameters to build an object. It requires a parameterless constructor to be available 
    /// on the type. <br/>
    /// For Strict mapping, it will either throw an exception if the writable keys and the object count 
    /// have a different number, or will throw any property name that does not have an object equivalent. <br/><br/>
    /// </summary>
    /// <exception cref="InvalidOperationException"/>
    /// <exception cref="StrictMappingException"/>
    T AsStrictProperties<T>();
}