using System.Text;

namespace Salix.Extensions;

/// <summary>
/// User input helper extensions.
/// </summary>
public static partial class Consolix
{
    /// <summary>
    /// Waits for the user to press the ENTER (RETURN) key.
    /// </summary>
    public static void ReadEnter() => ReadKeys(ConsoleKey.Enter);

    /// <summary>
    /// Waits for the user to press the ESC (escape) key.
    /// </summary>
    public static void ReadEscape() => ReadKeys(ConsoleKey.Escape);

    /// <summary>
    /// Waits for the user to press either Y or N (case insensitive).
    /// </summary>
    /// <returns>TRUE -if Y was pressed, FALSE - if N was pressed.</returns>
    public static bool ReadYesNo() => ReadKeys(true, 'y', 'n').ToString().ToUpperInvariant() == "Y";

    /// <summary>
    /// Waits for the user to press any key on the keyboard. Displays the character representing
    /// the pressed key in the console window.
    /// </summary>
    /// <returns>Information about the pressed key.</returns>
    public static ConsoleKeyInfo ReadKey() => Console.ReadKey(intercept: true);

    /// <summary>
    /// Waits for any of a specified set of <paramref name="keys"/> to be pressed by the user.
    /// </summary>
    /// <param name="keys">An array of characters representing the allowed set of characters.</param>
    /// <returns>The character pressed by the user.</returns>
    public static char ReadKeys(params char[] keys)
    {
        char key;
        do
        {
            key = Console.ReadKey(intercept: true).KeyChar;
        } while (Array.IndexOf(keys, key) < 0);
        return key;
    }

    /// <summary>
    /// Waits for any of a specified set of <paramref name="keys"/> to be pressed by the user.
    /// </summary>
    /// <param name="ignoreCase">Indicates whether to the keys pressed are case sensitive.</param>
    /// <param name="keys">An array of characters representing the allowed set of characters.</param>
    /// <returns>The character pressed by the user.</returns>
    public static char ReadKeys(bool ignoreCase, params char[] keys)
    {
        var casedKeys = ignoreCase ? keys.Select(char.ToUpperInvariant).ToArray() : keys;
        int keyIndex;
        do
        {
            var key = char.ToUpperInvariant(System.Console.ReadKey(intercept: true).KeyChar);
            keyIndex = Array.IndexOf(casedKeys, key);
        }
        while (keyIndex < 0);
        return keys[keyIndex];
    }

    /// <summary>
    /// Waits for any of a specified set of <paramref name="keys"/> to be pressed by the user.
    /// </summary>
    /// <param name="keys">
    /// An array of <see cref="ConsoleKey"/> objects representing the allowed set of keys.
    /// </param>
    /// <returns>The key pressed by the user.</returns>
    public static ConsoleKey ReadKeys(params ConsoleKey[] keys)
    {
        ConsoleKey key;
        do
        {
            key = Console.ReadKey(intercept: true).Key;
        } while (Array.IndexOf(keys, key) < 0);
        return key;
    }

    /// <summary>
    /// Reads the line from console input masking pressed characters with asterix.
    /// </summary>
    public static string ReadLineSecret()
    {
        var keyInfo = Console.ReadKey(intercept: true);
        var secret = new StringBuilder();
        while (keyInfo.Key != ConsoleKey.Enter)
        {
            secret.Append(keyInfo.KeyChar);
            Console.Write("*");
            keyInfo = Console.ReadKey(intercept: true);
        }

        Console.WriteLine();
        return secret.ToString();
    }
}
