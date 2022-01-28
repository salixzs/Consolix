namespace Salix.Extensions;

/// <summary>
/// Menu settings.
/// </summary>
public sealed class SelectSingleSettings
{
    /// <summary>
    /// Caption (Prompt) to show above menu
    /// </summary>
    public string Caption { get; set; } = "Use up, down keys and select one of choices by pressing Enter: ";

    /// <summary>
    /// String prefix to put before selected menu item. Default >
    /// </summary>
    public string SelectedPrefix { get; set; } = " > ";

    /// <summary>
    /// Foreground color for selected menu item. Default - white.
    /// </summary>
    public ConsoleColor SelectedForegroundColor { get; set; } = ConsoleColor.White;

    /// <summary>
    /// Background color for selected menu item. Default - Dark Magenta (Dark Purple).
    /// </summary>
    public ConsoleColor SelectedBackgroundColor { get; set; } = ConsoleColor.DarkMagenta;

    /// <summary>
    /// Prefix to put before unselected menu item. Default - empty.
    /// </summary>
    public string UnselectedPrefix { get; set; } = "   ";

    /// <summary>
    /// Foreground color for unselected menu items. Default - Gray.
    /// </summary>
    public ConsoleColor UnselectedForegroundColor { get; set; } = ConsoleColor.Gray;

    /// <summary>
    /// Background color to draw for unselected menu items. Default - black.
    /// </summary>
    public ConsoleColor UnselectedBackgroundColor { get; set; } = ConsoleColor.Black;

    /// <summary>
    /// Prompt of selected menu item when menu item is chosen.
    /// </summary>
    public string EndResultPrompt { get; set; } = "Selected: ";
}
