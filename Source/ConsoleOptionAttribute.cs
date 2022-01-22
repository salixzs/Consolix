namespace Salix.Extensions;

/// <summary>
/// Console command parameter attribute. Put on <see cref="IConsoleOperation"/> class property.
/// Automatically maps command line parameter to this property by name.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public class ConsoleOptionAttribute : Attribute
{
    /// <summary>
    /// Console command parameter attribute. Put on <see cref="IConsoleOperation"/> class property.
    /// Automatically maps command line parameter to this property by name.
    /// </summary>
    /// <param name="name">Full name of input parameter.</param>
    /// <param name="description">Description of parameter. Used to display help on it.</param>
    /// <param name="shortName">Shortened parameter name. (Example: Name = show, shortenedName = s. This would accept command line input of --show [param] and --s [param]).</param>
    public ConsoleOptionAttribute(string name, string description, string shortName)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Option name should be specified.", nameof(name));
        }

        if (name.Contains(" "))
        {
            throw new ArgumentException("Option name should be a single word.", nameof(name));
        }

        if (name.Length > 12)
        {
            throw new ArgumentException("Option name should have no more than 12 letters.", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("Description/Help should be specified.", nameof(description));
        }

        this.Name = name;
        this.Description = description;
        this.ShortName = shortName;
    }

    /// <summary>
    /// Full name of command-line input parameter. (e.g. "show", would expect command-line "--show" parameter).
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Shortened parameter name. (Example: <see cref="Name"/> = show, ShortName = s. This would accept command line input of --show [param] or --s [param]).
    /// </summary>
    public string ShortName { get; }

    /// <summary>
    /// Description of parameter. Used to display help on it.
    /// </summary>
    public string Description { get; }
}
