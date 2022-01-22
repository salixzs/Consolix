namespace Salix.Extensions;

public static partial class Consolix
{
    /// <summary>
    /// Creates a selectable menu from supplied items. One can be selected by up/down arrows.
    /// Returns zero-based index of chosen menu item.
    /// </summary>
    /// <param name="optionsList">List of strings to show as menu items.</param>
    /// <param name="settings">A settings for menu. Either supply own or leave unsupplied to get default settings.</param>
    /// <param name="startingIndex">Initial menu item to be selected (zero-based). Default = 0.</param>
    /// <returns>Selected menu item index (Zero-based).</returns>
    public static int MenuSelectSingle(
        IEnumerable<string> optionsList,
        SelectSingleSettings settings = null,
        int startingIndex = 0)
    {
        if (optionsList is null)
        {
            throw new ArgumentNullException(nameof(optionsList));
        }

        var options = optionsList.ToList();
        if (options.Count < 2)
        {
            throw new ArgumentException("Options list must have more than one item.", nameof(optionsList));
        }

        settings ??= new SelectSingleSettings();
        int longestOption = options.Aggregate(string.Empty, (seed, f) => (f?.Length ?? 0) > seed.Length ? f : seed).Length + settings.SelectedPrefix.Length + 1;
        ClearLine();
        bool cursorVisible = Console.CursorVisible;
        Console.CursorVisible = false;
        try
        {
            int topLine = Console.CursorTop;
            int startLine = topLine + 1;

            Console.WriteLine(settings.Caption);

            int selectedChoice = startingIndex >= 0 && startingIndex < options.Count
                ? startingIndex
                : 0;
            string? unselectedPrefix =
                settings.UnselectedPrefix ?? new string(c: ' ', settings.SelectedPrefix.Length);

            // Print the initial list with the selected value highlighted
            for (int i = 0; i < options.Count; i++)
            {
                if (i == selectedChoice)
                {
                    WriteLine($"{settings.SelectedPrefix}{options[i]}".PadRight(longestOption), settings.SelectedForegroundColor, settings.SelectedBackgroundColor);
                }
                else
                {
                    WriteLine($"{settings.UnselectedPrefix}{options[i]}".PadRight(longestOption), settings.UnselectedForegroundColor, settings.UnselectedBackgroundColor);
                }
            }

            // Repeatedly handle up and down arrow key presses until Enter is pressed
            ConsoleKey pressed = ReadKeys(ConsoleKey.UpArrow, ConsoleKey.DownArrow, ConsoleKey.Enter);
            while (pressed != ConsoleKey.Enter)
            {
                int oldChoice = selectedChoice;

                if (pressed == ConsoleKey.UpArrow)
                {
                    selectedChoice--;
                }
                else
                {
                    selectedChoice++;
                }

                if (selectedChoice < 0)
                {
                    selectedChoice = options.Count - 1;
                }
                else if (selectedChoice >= options.Count)
                {
                    selectedChoice = 0;
                }

                Console.SetCursorPosition(left: 0, startLine + oldChoice);
                WriteLine($"{settings.UnselectedPrefix}{options[oldChoice]}".PadRight(longestOption), settings.UnselectedForegroundColor, settings.UnselectedBackgroundColor);
                Console.SetCursorPosition(left: 0, startLine + selectedChoice);
                WriteLine($"{settings.SelectedPrefix}{options[selectedChoice]}".PadRight(longestOption), settings.SelectedForegroundColor, settings.SelectedBackgroundColor);
                pressed = ReadKeys(ConsoleKey.UpArrow, ConsoleKey.DownArrow, ConsoleKey.Enter);
            }

            for (int i = 0; i < options.Count + 1; i++)
            {
                ClearLine(i + topLine);
            }

            Console.SetCursorPosition(left: 0, topLine);
            Console.WriteLine($"{settings.EndResultPrompt} {options[selectedChoice]}");
            return selectedChoice;
        }
        finally
        {
            Console.CursorVisible = cursorVisible;
        }
    }
}

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
