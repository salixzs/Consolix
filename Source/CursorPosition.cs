using System.Runtime.CompilerServices;

namespace Salix.Extensions;

/// <summary>
/// Class to operate with Cursor position on screen.
/// </summary>
public class CursorPosition
{
    /// <summary>
    /// Creates Cursor position object. When <c>true</c> is given for parameter - saves current cursor coordinates.
    /// </summary>
    /// <param name="storeCurrentPosition">When <c>true</c> - stores current cursor position. Otherwise - top/left corner (default: 0, 0)</param>
    public static CursorPosition New(bool storeCurrentPosition = false) => new(storeCurrentPosition);

    /// <summary>
    /// Creates Cursor position object with saved current cursor coordinates.
    /// </summary>
    public static CursorPosition Current() => new(true);

    /// <summary>
    /// Creates Cursor position object with given coordinates.
    /// </summary>
    /// <param name="top">Top coordinate for cursor position.</param>
    /// <param name="left">Left coordinate for cursor position.</param>
    public static CursorPosition New(int top, int left) => new (top, left);

    /// <summary>
    /// Creates Cursor position object with given coordinates.
    /// </summary>
    /// <param name="top">Top coordinate for cursor position.</param>
    /// <param name="left">Left coordinate for cursor position.</param>
    public CursorPosition(int top, int left)
    {
        this.Top = top;
        this.Left = left;
    }

    /// <summary>
    /// Creates Cursor position object. When <c>true</c> is given for parameter - saves current cursor coordinates.
    /// </summary>
    /// <param name="storeCurrentPosition">When <c>true</c> - stores current cursor position. Otherwise - top/left corner (default: 0, 0)</param>
    public CursorPosition(bool storeCurrentPosition = false)
    {
        if (storeCurrentPosition)
        {
            this.Top = Console.CursorTop;
            this.Left = Console.CursorLeft;
        }
    }

    /// <summary>
    /// Top part of cursor coordinates.
    /// </summary>
    public int Top { get; set; }

    /// <summary>
    /// Left part of cursor coordinates.
    /// </summary>
    public int Left { get; set; }

    /// <summary>
    /// Stores current cursor position into this object.
    /// </summary>
    public void StoreCurrentPosition()
    {
        this.Top = Console.CursorTop;
        this.Left = Console.CursorLeft;
    }

    /// <summary>
    /// Moves cursor to position onto coordinates stored into this object.
    /// </summary>
    public void MoveTo() => Console.SetCursorPosition(this.Left, this.Top);
}
