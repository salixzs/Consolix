using System.Globalization;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Salix.Extensions;

/// <summary>
/// Determines and prepares command, issued by running console application.
/// </summary>
public class ConsoleOperationHandler
{
    private readonly List<IConsoleOperation> _operations;
    private string[] _args;
    private readonly ILogger<ConsoleOperationHandler> _logger;

    /// <summary>
    /// Determines and prepares command, issued by running console application.
    /// </summary>
    /// <param name="logger">Instance of Logger object.</param>
    /// <param name="operations">A list of operations handled by console application (all instances, inplementing IConsoleOperation).</param>
    public ConsoleOperationHandler(IEnumerable<IConsoleOperation> operations, ILogger<ConsoleOperationHandler> logger)
    {
        _logger = logger;
        _operations = operations.ToList();
    }

    /// <summary>
    /// Determines which operation to run and loads its parameters.
    /// </summary>
    /// <param name="args">Arguments from command line.</param>
    /// <param name="configuration">[Optional] Reference to configuration object.</param>
    public void PrepareOperation(string[] args, IConfigurationRoot configuration = null)
    {
        _args = args;
        var operationNames = new List<string>(_operations.Count);
        foreach (IConsoleOperation? consoleOperation in _operations)
        {
            operationNames.Add(consoleOperation.OperationName);
            if (_args.Contains(consoleOperation.OperationName, StringComparer.InvariantCultureIgnoreCase))
            {
                this.SelectedOperation = consoleOperation;
            }
        }

        this.Operations = new HashSet<string>(operationNames);
        if (this.SelectedOperation == null && _operations.Count == 1)
        {
            this.SelectedOperation = _operations.First();
        }

        if (this.SelectedOperation == null)
        {
            // Should result in showing help.
            return;
        }

        _logger.LogDebug($"Parsing operation \"{this.SelectedOperation.OperationName}\"");
        this.ValidateOperationProperties(this.SelectedOperation);
        this.SetOperationProperties(this.SelectedOperation, configuration, args);
    }

    private void SetOperationProperties(IConsoleOperation consoleOperation, IConfigurationRoot configuration, string[] args)
    {
        IEnumerable<PropertyInfo>? properties = consoleOperation.GetType()
            .GetProperties()
            .Where(prop => prop.IsDefined(typeof(ConsoleOptionAttribute), inherit: false));
        bool configurationExists = false;
        IConfigurationSection operationOptionsInConfiguration = null;
        if (configuration != null)
        {
            operationOptionsInConfiguration = configuration.GetSection(consoleOperation.OperationName);
            if (operationOptionsInConfiguration.Exists())
            {
                configurationExists = true;
            }
            else
            {
                _logger.LogDebug($"Operation \"{consoleOperation.OperationName}\" does not have a section in configuration (optional - not necessary).");
            }
        }

        foreach (PropertyInfo? operationArgumentProperty in properties)
        {
            ConsoleOptionAttribute? optionAttribute = ((ConsoleOptionAttribute[])operationArgumentProperty
                .GetCustomAttributes(typeof(ConsoleOptionAttribute), inherit: false))[0];
            _logger.LogDebug($"Parsing operation \"{consoleOperation.OperationName}\" option \"{optionAttribute.Name}\".");

            if (configurationExists)
            {
                this.LoadOptionFromConfiguration(consoleOperation, operationOptionsInConfiguration, operationArgumentProperty, optionAttribute);
            }

            this.LoadOptionsFromArguments(consoleOperation, operationArgumentProperty, optionAttribute, args);
            _logger.LogInformation($"Operation \"{consoleOperation.OperationName}\" option \"{optionAttribute.Name}\" = {operationArgumentProperty.GetValue(consoleOperation)}");
        }
    }

    private void LoadOptionFromConfiguration(IConsoleOperation consoleOperation, IConfigurationSection operationOptionsInConfiguration, PropertyInfo operationArgumentProperty, ConsoleOptionAttribute optionAttribute)
    {
        string? configurationStringValue = operationOptionsInConfiguration[optionAttribute.Name];
        try
        {
            object? argumentTypedValue = Convert.ChangeType(configurationStringValue, operationArgumentProperty.PropertyType, CultureInfo.InvariantCulture);
            operationArgumentProperty.SetValue(consoleOperation, argumentTypedValue);
            _logger.LogDebug($"  - option \"{optionAttribute.Name}\" value is set to \"{configurationStringValue}\" from configuration.");
        }
        catch (Exception ex)
        {
            // IsReady checks properties before execution of operation.
            _logger.LogWarning($"  - option \"{optionAttribute.Name}\" value \"{configurationStringValue}\" cannot be set to property. Error: {ex.Message}.");
        }
    }

    private void LoadOptionsFromArguments(IConsoleOperation consoleOperation, PropertyInfo operationArgumentProperty, ConsoleOptionAttribute optionAttribute, string[] args)
    {
        int index = Array.FindIndex(args, arg => arg == $"--{optionAttribute.Name}");
        if (index == -1 && !string.IsNullOrEmpty(optionAttribute.ShortName))
        {
            index = Array.FindIndex(args, arg => arg == $"--{optionAttribute.ShortName}");
        }

        // For booleans we just need it present without value
        if (index != -1 && operationArgumentProperty.PropertyType == typeof(bool))
        {
            _logger.LogDebug($"  - option \"{optionAttribute.Name}\" is set from arguments to TRUE.");
            operationArgumentProperty.SetValue(consoleOperation, true);
            return;
        }

        if (index == -1 || args.Length <= index + 1)
        {
            _logger.LogDebug($"  - option \"{optionAttribute.Name}\" is not found in arguments.");
            return;
        }

        string? argumentStringValue = args[index + 1];
        try
        {
            // Handle ENUMs
            if (operationArgumentProperty.PropertyType.IsEnum)
            {
                // Special case - FLAGged ENUMs
                if (operationArgumentProperty.PropertyType.IsDefined(typeof(FlagsAttribute), false))
                {
                    if (IsInteger(argumentStringValue))
                    {
                        object? enumInt = Convert.ChangeType(argumentStringValue, typeof(int), CultureInfo.InvariantCulture);
                        operationArgumentProperty.SetValue(consoleOperation, enumInt);
                        _logger.LogDebug($"  - option \"{optionAttribute.Name}\" is set from arguments to {enumInt} (Flagged Enum value).");
                    }
                    else
                    {
                        int enumInt = 0;
                        foreach (string? enumValueName in argumentStringValue.Split(','))
                        {
                            foreach (object? enumValue in Enum.GetValues(operationArgumentProperty.PropertyType))
                            {
                                if (enumValue.ToString().Equals(enumValueName, StringComparison.OrdinalIgnoreCase))
                                {
                                    enumInt += (int)enumValue;
                                }
                            }
                        }
                        operationArgumentProperty.SetValue(consoleOperation, enumInt);
                        _logger.LogDebug($"  - option \"{optionAttribute.Name}\" is set from arguments to {enumInt} (Flagged Enum value).");
                    }
                }

                // Normal ENUMs
                if (IsInteger(argumentStringValue))
                {
                    object? enumInt = Convert.ChangeType(argumentStringValue, typeof(int), CultureInfo.InvariantCulture);
                    foreach (object? enumValue in Enum.GetValues(operationArgumentProperty.PropertyType))
                    {
                        if ((int)enumValue == (int)enumInt)
                        {
                            operationArgumentProperty.SetValue(consoleOperation, enumValue);
                            _logger.LogDebug($"  - option \"{optionAttribute.Name}\" is set from arguments to {enumValue}.");
                        }
                    }
                }
                else
                {
                    foreach (object? enumValue in Enum.GetValues(operationArgumentProperty.PropertyType))
                    {
                        if (enumValue.ToString().Equals(argumentStringValue, StringComparison.OrdinalIgnoreCase))
                        {
                            operationArgumentProperty.SetValue(consoleOperation, enumValue);
                            _logger.LogDebug($"  - option \"{optionAttribute.Name}\" is set from arguments to {enumValue}.");
                        }
                    }
                }
            }
            else
            {
                object? argumentTypedValue = Convert.ChangeType(argumentStringValue, operationArgumentProperty.PropertyType, CultureInfo.InvariantCulture);
                operationArgumentProperty.SetValue(consoleOperation, argumentTypedValue);
                _logger.LogDebug($"  - option \"{optionAttribute.Name}\" is set from arguments to {argumentStringValue}.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"  - option \"{optionAttribute.Name}\" value {argumentStringValue} cannot be set to property. Error: {ex.Message}.");
            // IsReady checks properties before execution of operation.
        }
    }

    private void ValidateOperationProperties(IConsoleOperation consoleOperation)
    {
        if (string.IsNullOrEmpty(consoleOperation.OperationName))
        {
            Consolix.WriteLine("Console operation is missing name", ConsoleColor.Red);
            _logger.LogError($"Operation {consoleOperation.OperationName} is missing name");
            throw new ArgumentException($"Operation {consoleOperation.OperationName} is missing name");
        }
        if (consoleOperation.OperationName.Contains(" "))
        {
            Consolix.WriteLine("Option name should be a single word.", ConsoleColor.Red);
            _logger.LogError($"Operation {consoleOperation.OperationName} name should be a single word.");
            throw new ArgumentException($"Operation \"{consoleOperation.OperationName}\" name should be a single word.");
        }

        if (consoleOperation.OperationName.Length > 12)
        {
            Consolix.WriteLine("Option name should have no more than 12 letters.", ConsoleColor.Red);
            _logger.LogError($"Operation {consoleOperation.OperationName} name should have no more than 12 letters.");
            throw new ArgumentException($"Operation {consoleOperation.OperationName} name should have no more than 12 letters.");
        }

        if (string.IsNullOrWhiteSpace(consoleOperation.HelpText))
        {
            Consolix.WriteLine("Description/Help should be specified.", ConsoleColor.Red);
            _logger.LogError($"Operation {consoleOperation.OperationName} should have description specified.");
            throw new ArgumentException($"Operation {consoleOperation.OperationName} should have description specified.");
        }
    }

    /// <summary>
    /// Outputs the HELP on Application.
    /// </summary>
    /// <param name="name">The name of executable (Default: Program).</param>
    /// <param name="generalDescription">The general description to show on startup.</param>
    public void OutputHelp(string name, string generalDescription)
    {
        if (!string.IsNullOrEmpty(generalDescription))
        {
            Consolix.WriteLine(generalDescription, ConsoleColor.Green);
        }

        Consolix.WriteLine();
        Consolix.Write("Usage: ", ConsoleColor.Gray);
        Consolix.Write(name ?? "program", ConsoleColor.Cyan);
        Consolix.Write(" [operation] [options]", ConsoleColor.DarkCyan);

        Consolix.WriteLine();
        Consolix.WriteLine("General options:", ConsoleColor.Gray);
        Consolix.Write($"  --help|--h     ", ConsoleColor.Cyan);
        Consolix.WriteLine("Display help.", ConsoleColor.Green);

        if (this.SelectedOperation != null)
        {
            Consolix.WriteLine();
            Consolix.WriteLine("Selected operation: ", ConsoleColor.Gray);
            Consolix.Write("  " + this.SelectedOperation.OperationName.PadRight(15), ConsoleColor.Cyan);
            Consolix.WriteLine(this.SelectedOperation.HelpText, ConsoleColor.Green);

            var properties = this.SelectedOperation.GetType()
                .GetProperties()
                .Where(prop => prop.IsDefined(typeof(ConsoleOptionAttribute), inherit: false)).ToList();
            if (properties.Any())
            {
                Consolix.WriteLine("Selected operation options:", ConsoleColor.Gray);
            }

            foreach (PropertyInfo? propertyInfo in properties)
            {
                ConsoleOptionAttribute? optionAttribute = ((ConsoleOptionAttribute[])
                    propertyInfo.GetCustomAttributes(typeof(ConsoleOptionAttribute), inherit: false))[0];
                Consolix.Write(
                    ("  --" + optionAttribute.Name + (string.IsNullOrEmpty(optionAttribute.ShortName) ? string.Empty : $"|--{optionAttribute.ShortName}")).PadRight(20),
                    ConsoleColor.Cyan);
                Consolix.WriteLine(optionAttribute.Description, ConsoleColor.Green);
                if (propertyInfo.PropertyType.IsEnum)
                {
                    Consolix.Write("                    Enums: ", ConsoleColor.Gray);
                    Consolix.WriteLine(GetEnumNamesAndValues(propertyInfo.PropertyType), ConsoleColor.DarkCyan);
                }
            }
            return;
        }

        Consolix.WriteLine();
        Consolix.WriteLine("Operations:", ConsoleColor.Gray);
        foreach (IConsoleOperation? operation in _operations)
        {
            Consolix.Write($"  {operation.OperationName}".PadRight(18), ConsoleColor.Cyan);
            Consolix.WriteLine(operation.HelpText, ConsoleColor.Green);
        }
    }

    /// <summary>
    /// Gets the enum names and values joined together with comma-separator (Name(0),Name(1),Name(2)...).
    /// </summary>
    /// <param name="enumType">Type of the enumeration.</param>
    private static string GetEnumNamesAndValues(Type enumType)
    {
        IEnumerable<FieldInfo>? fields = enumType.GetFields().Where(field => field.IsLiteral);
        var values = fields.Select(field => new { field, value = field.GetValue(enumType) })
            .Select(@t => $"{@t.field.Name}({(int)@t.value})")
            .ToList();
        return string.Join(",", values);
    }

    /// <summary>
    /// Determines whether the specified string to check is integer. Also handles empty/null strings accordingly.
    /// </summary>
    /// <param name="stringToCheck">The string to check.</param>
    /// <returns>
    /// <c>true</c> if the specified string to check is integer; otherwise (incl. empty/null), <c>false</c>.
    /// </returns>
    public static bool IsInteger(string stringToCheck) => !string.IsNullOrWhiteSpace(stringToCheck) && stringToCheck.Trim().All(char.IsNumber);

    /// <summary>
    /// Should contain operation chosen to execute either through args or in any other mean (the only operation).
    /// </summary>
    public IConsoleOperation SelectedOperation { get; set; }

    /// <summary>
    /// List of operation names.
    /// </summary>
    public HashSet<string> Operations { get; private set; }
}
