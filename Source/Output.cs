using System.Text.RegularExpressions;
using System;

namespace Salix.Extensions;

public partial class Consolix
{
    public static void Write(string text) => Console.Write(text);
    public static void Write(string text, object arg0) => Console.Write(text, arg0);
    public static void Write(string text, object arg0, object arg1) => Console.Write(text, arg0, arg1);
    public static void Write(string text, params object[] arg) => Console.Write(text, arg);
    public static void WriteLine() => Console.WriteLine();
    public static void WriteLine(string text) => Console.WriteLine(text);
    public static void WriteLine(string text, object arg0) => Console.WriteLine(text, arg0);
    public static void WriteLine(string text, object arg0, object arg1) => Console.WriteLine(text, arg0, arg1);
    public static void WriteLine(string text, params object[] arg) => Console.WriteLine(text, arg);

    /// <summary>
    /// Writes the specified text in given color without line break.
    /// </summary>
    public static void Write(string text, ConsoleColor color) =>
        WriteInColor(Console.Write, text, color);

    /// <summary>
    /// Writes the specified text in given color with line break.
    /// </summary>
    public static void WriteLine(string text, ConsoleColor color) =>
        WriteInColor(Console.WriteLine, text, color);

    /// <summary>
    /// Writes the specified text in given color onto given background color without line break.
    /// </summary>
    public static void Write(string text, ConsoleColor color, ConsoleColor background) =>
        WriteInColor(text, color, background);

    /// <summary>
    /// Writes the specified text in given color onto given background color with line break.
    /// </summary>
    public static void WriteLine(string text, ConsoleColor color, ConsoleColor background) =>
        WriteLineInColor(text, color, background);

    /// <summary>
    /// Writes the specified text-format ("text {0}") in given color without line break.
    /// </summary>
    public static void Write(string formatString, object placeholderValue, ConsoleColor color) =>
        WriteInColor(Console.Write, formatString, placeholderValue, color);

    /// <summary>
    /// Writes the specified text-format with one argument ("text {0}") in given color with line break.
    /// </summary>
    public static void WriteLine(string formatString, object placeholderValue, ConsoleColor color) =>
        WriteInColor(Console.WriteLine, formatString, placeholderValue, color);

    /// <summary>
    /// Writes the specified text-format ("text {0}") with one argument in given colors without line break.
    /// </summary>
    public static void Write(string formatString, object placeholderValue, ConsoleColor mainColor,
        ConsoleColor argumentColor) =>
        WriteInColor(Console.Write, formatString, placeholderValue, mainColor, argumentColor);

    /// <summary>
    /// Writes the specified text-format ("text {0}") in given colors with line break.
    /// </summary>
    public static void WriteLine(string formatString, object placeholderValue, ConsoleColor mainColor,
        ConsoleColor argumentColor) =>
        WriteInColor(Console.WriteLine, formatString, placeholderValue, mainColor, argumentColor);

    /// <summary>
    /// Writes the specified text-format ("text {0}") with one argument in given colors without line break.
    /// </summary>
    public static void Write(string formatString, object placeholderValue, ConsoleColor mainColor,
        ConsoleColor argumentColor, ConsoleColor background) =>
        WriteInColor(Console.Write, formatString, placeholderValue, mainColor, argumentColor, background);

    /// <summary>
    /// Writes the specified text-format ("text {0}") in given colors with line break.
    /// </summary>
    public static void WriteLine(string formatString, object placeholderValue, ConsoleColor mainColor,
        ConsoleColor argumentColor, ConsoleColor background) =>
        WriteInColor(Console.WriteLine, formatString, placeholderValue, mainColor, argumentColor, background);

    /// <summary>
    /// Writes the specified text-format ("text {0} {1} {2}") in given color without line break.
    /// </summary>
    public static void Write(string format, ConsoleColor color, params object[] args) =>
        WriteInColor(Console.Write, format, args, color);

    /// <summary>
    /// Writes the specified text-format ("text {0} {1} {2}") in given color with line break.
    /// </summary>
    public static void WriteLine(string format, ConsoleColor color, params object[] args) =>
        WriteInColor(Console.WriteLine, format, args, color);

    /// <summary>
    /// Writes the specified text-format ("text {0} {1} {2}") in given color with arguments colored differently without line break.
    /// </summary>
    public static void Write(string format, ConsoleColor mainColor, ConsoleColor argumentColor, params object[] args) =>
        WriteInColor(Console.Write, format, mainColor, argumentColor, args);

    /// <summary>
    /// Writes the specified text-format ("text {0} {1} {2}") in given color with arguments colored differently with line break.
    /// </summary>
    public static void WriteLine(string format, ConsoleColor mainColor, ConsoleColor argumentColor,
        params object[] args) =>
        WriteInColor(Console.WriteLine, format, mainColor, argumentColor, args);

    /// <summary>
    /// Clears the current entire line from any output.
    /// Sets Cursor at beginning of the line.
    /// </summary>
    public static void ClearLine()
    {
        var line = Console.CursorTop;
        Console.SetCursorPosition(0, line);
        Console.Write(new string(' ', Console.WindowWidth));
        Console.SetCursorPosition(0, line);
    }

    /// <summary>
    /// Clears the given line entirely from any output.
    /// Does nothing if line number is given incorrectly.
    /// Leaves cursor at initial position.
    /// To clear current line, see <see cref="ClearLine()"/>
    /// </summary>
    /// <param name="line">Vertical position of cursor (row on screen).</param>
    public static void ClearLine(int line)
    {
        var (cursorLeft, cursorTop) = (Console.CursorLeft, Console.CursorTop);
        try
        {
            Console.SetCursorPosition(0, line);
            Console.Write(new string(' ', Console.WindowWidth));
        }
        finally
        {
            Console.SetCursorPosition(cursorLeft, cursorTop);
        }
    }

    /// <summary>
    /// Sets the cursor to specified horizontal position in current line.
    /// </summary>
    /// <param name="horizontalPosition">number of horizontal console cell.</param>
    public static void CursorToPosition(int horizontalPosition) =>
        Console.SetCursorPosition(horizontalPosition, Console.CursorTop);

    /// <summary>
    /// Overwrites the content of current line.
    /// NOTE: Works only when previous output is done with Console.Write (not WriteLine)
    /// and previous output did not wrap over to new line.
    /// </summary>
    public static void WriteOver(string text)
    {
        ClearLine();
        Console.Write(text);
    }

    /// <summary>
    /// Overwrites the content of current line and shifts to new line.
    /// NOTE: Works only when previous output is done with Console.Write (not WriteLine)
    /// and previous output did not wrap over to new line.
    /// </summary>
    public static void WriteLineOver(string text)
    {
        ClearLine();
        Console.WriteLine(text);
    }

    /// <summary>
    /// Overwrites the content of current line.
    /// NOTE: Works only when previous output is done with Console.Write (not WriteLine)
    /// and previous output did not wrap over to new line.
    /// </summary>
    public static void WriteOver(string text, ConsoleColor color)
    {
        ClearLine();
        WriteInColor(Console.Write, text, color);
    }

    /// <summary>
    /// Overwrites the content of current line and shifts to new line.
    /// NOTE: Works only when previous output is done with Console.Write (not WriteLine)
    /// and previous output did not wrap over to new line.
    /// </summary>
    public static void WriteLineOver(string text, ConsoleColor color)
    {
        ClearLine();
        WriteInColor(Console.WriteLine, text, color);
    }

    /// <summary>
    /// Overwrites the content of current line.
    /// NOTE: Works only when previous output is done with Console.Write (not WriteLine)
    /// and previous output did not wrap over to new line.
    /// </summary>
    public static void WriteOver(string format, object arg0, ConsoleColor color)
    {
        var cursorPosition = Console.CursorLeft;
        Console.SetCursorPosition(0, Console.CursorTop);
        WriteInColor(Console.WriteLine, format, arg0, color);
    }

    #region Typed Write/WriteLine overrides
    public static void Write(bool value, ConsoleColor color) =>
        WriteInColor(Console.Write, value, color);
    public static void WriteLine(bool value, ConsoleColor color) =>
        WriteInColor(Console.WriteLine, value, color);

    public static void Write(char value, ConsoleColor color) =>
        WriteInColor(Console.Write, value, color);
    public static void WriteLine(char value, ConsoleColor color) =>
        WriteInColor(Console.WriteLine, value, color);

    public static void Write(char[] value, ConsoleColor color) =>
        WriteInColor(Console.Write, value, color);
    public static void WriteLine(char[] value, ConsoleColor color) =>
        WriteInColor(Console.WriteLine, value, color);

    public static void Write(int value, ConsoleColor color) =>
        WriteInColor(Console.Write, value, color);
    public static void WriteLine(int value, ConsoleColor color) =>
        WriteInColor(Console.WriteLine, value, color);

    public static void Write(long value, ConsoleColor color) =>
        WriteInColor(Console.Write, value, color);
    public static void WriteLine(long value, ConsoleColor color) =>
        WriteInColor(Console.WriteLine, value, color);

    public static void Write(decimal value, ConsoleColor color) =>
        WriteInColor(Console.Write, value, color);
    public static void WriteLine(decimal value, ConsoleColor color) =>
        WriteInColor(Console.WriteLine, value, color);

    public static void Write(double value, ConsoleColor color) =>
        WriteInColor(Console.Write, value, color);
    public static void WriteLine(double value, ConsoleColor color) =>
        WriteInColor(Console.WriteLine, value, color);

    public static void Write(float value, ConsoleColor color) =>
        WriteInColor(Console.Write, value, color);
    public static void WriteLine(float value, ConsoleColor color) =>
        WriteInColor(Console.WriteLine, value, color);

    public static void Write(object value, ConsoleColor color) =>
        WriteInColor(Console.Write, value, color);
    public static void WriteLine(object value, ConsoleColor color) =>
        WriteInColor(Console.WriteLine, value, color);

    public static void Write(uint value, ConsoleColor color) =>
        WriteInColor(Console.Write, value, color);
    public static void WriteLine(uint value, ConsoleColor color) =>
        WriteInColor(Console.WriteLine, value, color);

    public static void Write(ulong value, ConsoleColor color) =>
        WriteInColor(Console.Write, value, color);
    public static void WriteLine(ulong value, ConsoleColor color) =>
        WriteInColor(Console.WriteLine, value, color);
    #endregion

    private static void WriteInColor<T>(Action<T> writeOrWriteline, T outputValue, ConsoleColor color)
    {
        var oldForeground = Console.ForegroundColor;
        Console.ForegroundColor = color;
        writeOrWriteline.Invoke(outputValue);
        Console.ForegroundColor = oldForeground;
    }

    private static void WriteInColor<T>(T outputValue, ConsoleColor color, ConsoleColor background)
    {
        var oldForegroundColor = Console.ForegroundColor;
        var oldBackgroundColor = Console.BackgroundColor;
        Console.ForegroundColor = color;
        Console.BackgroundColor = background;
        Console.Write(outputValue);
        Console.ForegroundColor = oldForegroundColor;
        Console.BackgroundColor = oldBackgroundColor;
    }

    private static void WriteLineInColor<T>(T outputValue, ConsoleColor color, ConsoleColor background)
    {
        var oldForegroundColor = Console.ForegroundColor;
        var oldBackgroundColor = Console.BackgroundColor;
        Console.ForegroundColor = color;
        Console.BackgroundColor = background;
        Console.Write(outputValue);
        Console.ForegroundColor = oldForegroundColor;
        Console.BackgroundColor = oldBackgroundColor;
        Console.WriteLine();
    }

    private static void WriteInColor<T, U>(Action<T, U> writeOrWriteline, T target0, U target1, ConsoleColor color)
    {
        var oldForeground = Console.ForegroundColor;
        Console.ForegroundColor = color;
        writeOrWriteline.Invoke(target0, target1);
        Console.ForegroundColor = oldForeground;
    }

    private static void WriteInColor<T, U>(Action<T, U> writeOrWriteline, T target0, U target1, ConsoleColor color, ConsoleColor background)
    {
        var oldForegroundColor = Console.ForegroundColor;
        var oldBackgroundColor = Console.BackgroundColor;
        Console.ForegroundColor = color;
        Console.BackgroundColor = background;
        writeOrWriteline.Invoke(target0, target1);
        Console.ForegroundColor = oldForegroundColor;
        Console.BackgroundColor = oldBackgroundColor;
    }

    private static void WriteInColor(Action<string> writeOrWriteline, string formatString, ConsoleColor mainColor, ConsoleColor argumentColor, params object[] arguments)
    {
        var placeholders = ExtractPlaceholders(formatString);
        var placeholderValues = string.Format(string.Join("|||", placeholders), args: arguments)
            .Split(new[] { "|||" }, StringSplitOptions.None);
        var oldSystemColor = Console.ForegroundColor;
        OutputColoredFormatString(formatString, mainColor, argumentColor, placeholders, placeholderValues);
        writeOrWriteline.Invoke(string.Empty);
        Console.ForegroundColor = oldSystemColor;
    }

    private static void WriteInColor<T>(Action<string, T> writeOrWriteline, string formatString, T argument, ConsoleColor mainColor, ConsoleColor argumentColor)
    {
        var placeholders = ExtractPlaceholders(formatString);
        var placeholderValues = string.Format(string.Join("|||", placeholders), arg0: argument)
            .Split(new[] { "|||" }, StringSplitOptions.None);
        var oldSystemColor = Console.ForegroundColor;
        OutputColoredFormatString(formatString, mainColor, argumentColor, placeholders, placeholderValues);
        writeOrWriteline.Invoke(string.Empty, default);
        Console.ForegroundColor = oldSystemColor;
    }

    private static void WriteInColor(Action<string> writeOrWriteline, string formatString, ConsoleColor mainColor, ConsoleColor argumentColor, ConsoleColor background, params object[] arguments)
    {
        var placeholders = ExtractPlaceholders(formatString);
        var placeholderValues = string.Format(string.Join("|||", placeholders), args: arguments)
            .Split(new[] { "|||" }, StringSplitOptions.None);
        var oldForegroundColor = Console.ForegroundColor;
        var oldBackgroundColor = Console.BackgroundColor;
        OutputColoredFormatString(formatString, mainColor, argumentColor, placeholders, placeholderValues);
        writeOrWriteline.Invoke(string.Empty);
        Console.ForegroundColor = oldForegroundColor;
        Console.BackgroundColor = oldBackgroundColor;
    }

    private static void WriteInColor<T>(Action<string, T> writeOrWriteline, string formatString, T argument, ConsoleColor mainColor, ConsoleColor argumentColor, ConsoleColor background)
    {
        var placeholders = ExtractPlaceholders(formatString);
        var placeholderValues = string.Format(string.Join("|||", placeholders), arg0: argument)
            .Split(new[] { "|||" }, StringSplitOptions.None);
        var oldForegroundColor = Console.ForegroundColor;
        var oldBackgroundColor = Console.BackgroundColor;
        OutputColoredFormatString(formatString, mainColor, argumentColor, placeholders, placeholderValues);
        writeOrWriteline.Invoke(string.Empty, default);
        Console.ForegroundColor = oldForegroundColor;
        Console.BackgroundColor = oldBackgroundColor;
    }

    private static void OutputColoredFormatString(string formatString, ConsoleColor mainColor, ConsoleColor argumentColor, List<string> placeholders, string[] placeholderValues)
    {
        var transformedMainString = placeholders.Aggregate(formatString, (current, t) => current.Replace(t, $"|||{t}|||"));
        var mainStringParts = transformedMainString.Split(new[] { "|||" }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var mainStringPart in mainStringParts)
        {
            if (placeholders.Contains(mainStringPart))
            {
                WriteInColor(Console.Write, placeholderValues[placeholders.IndexOf(mainStringPart)], argumentColor);
            }
            else
            {
                WriteInColor(Console.Write, mainStringPart, mainColor);
            }
        }
    }

    private static List<string> ExtractPlaceholders(string formatString) =>
        new Regex(@"\{(\s*?.*?)*?\}")
            .Matches(formatString)
            .Cast<Match>()
            .Select(m => m.Value).ToList();
}
