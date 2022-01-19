using System.Runtime.InteropServices;

namespace Consolix
{
#pragma warning disable IDE1006 // Naming Styles
    public partial class Consolix
    {
        /// <summary>
        /// Sets the console color scheme/theme to one of predefined schemes.
        /// </summary>
        /// <param name="scheme">The color scheme/theme/style.</param>
        public static void SetColorScheme(ConsoleColorScheme scheme)
        {
            if (scheme == ConsoleColorScheme.Default)
            {
                return;
            }

            var colorMapper = new ColorMapper();
            switch (scheme)
            {
                case ConsoleColorScheme.HalfDark:
                    ColorMapper.SetBatchBufferColors(OneHalfDarkScheme);
                    break;
                case ConsoleColorScheme.Campbell:
                    ColorMapper.SetBatchBufferColors(CampbellScheme);
                    break;
                case ConsoleColorScheme.Vintage:
                    ColorMapper.SetBatchBufferColors(VintageScheme);
                    break;
                case ConsoleColorScheme.Raspberry:
                    ColorMapper.SetBatchBufferColors(RaspberryScheme);
                    break;
                case ConsoleColorScheme.Default:
                default:
                    break;
            }

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Clear(); // To redraw screen with consistent background
        }

        /// <summary>
        /// Sets the color scheme to custom color scheme (theme/style).
        /// Define it:
        /// <code>
        /// new()
        /// {
        ///     { "darkBlue", new COLORREF(r, g, b) },
        ///     { "darkGreen", new COLORREF(r, g, b) },
        ///     { "darkCyan", new COLORREF(r, g, b) },
        ///     { "darkRed", new COLORREF(r, g, b) },
        ///     { "darkMagenta", new COLORREF(r, g, b) },
        ///     { "darkYellow", new COLORREF(r, g, b) },
        ///     { "blue", new COLORREF(r, g, b) },
        ///     { "green", new COLORREF(r, g, b) },
        ///     { "cyan", new COLORREF(r, g, b) },
        ///     { "red", new COLORREF(r, g, b) },
        ///     { "magenta", new COLORREF(r, g, b) },
        ///     { "yellow", new COLORREF(r, g, b) },
        ///     { "black", new COLORREF(r, g, b) },
        ///     { "darkGray", new COLORREF(r, g, b) },
        ///     { "gray", new COLORREF(r, g, b) },
        ///     { "white", new COLORREF(r, g, b) },
        /// }
        /// </code>
        /// </summary>
        /// <param name="colorTheme">The custom color scheme (style).</param>
        public static void SetColorScheme(Dictionary<string, COLORREF> colorTheme)
        {
            var colorMapper = new ColorMapper();
            ColorMapper.SetBatchBufferColors(colorTheme);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Clear(); // To redraw screen with consistent background
        }

        private static readonly Dictionary<string, COLORREF> OneHalfDarkScheme = new()
        {
            { "black", new COLORREF(40, 44, 52) },
            { "darkBlue", new COLORREF(97, 175, 239) },
            { "darkGreen", new COLORREF(152, 195, 121) },
            { "darkCyan", new COLORREF(101, 188, 199) },
            { "darkRed", new COLORREF(224, 108, 117) },
            { "darkMagenta", new COLORREF(198, 120, 221) },
            { "darkYellow", new COLORREF(229, 192, 123) },
            { "gray", new COLORREF(220, 223, 228) },
            { "darkGray", new COLORREF(0, 0, 0) },
            { "blue", new COLORREF(97, 175, 239) },
            { "green", new COLORREF(152, 195, 121) },
            { "cyan", new COLORREF(101, 188, 199) },
            { "red", new COLORREF(224, 108, 117) },
            { "magenta", new COLORREF(198, 120, 221) },
            { "yellow", new COLORREF(229, 192, 123) },
            { "white", new COLORREF(248, 249, 250) },
        };

        private static readonly Dictionary<string, COLORREF> CampbellScheme = new()
        {
            { "black", new COLORREF(12, 12, 12) },
            { "darkBlue", new COLORREF(0, 55, 218) },
            { "darkGreen", new COLORREF(19, 161, 14) },
            { "darkCyan", new COLORREF(58, 150, 221) },
            { "darkRed", new COLORREF(197, 15, 31) },
            { "darkMagenta", new COLORREF(136, 23, 152) },
            { "darkYellow", new COLORREF(193, 156, 0) },
            { "gray", new COLORREF(166, 166, 166) },
            { "darkGray", new COLORREF(78, 86, 102) },
            { "blue", new COLORREF(59, 120, 255) },
            { "green", new COLORREF(22, 198, 12) },
            { "cyan", new COLORREF(97, 214, 214) },
            { "red", new COLORREF(231, 72, 86) },
            { "magenta", new COLORREF(180, 0, 158) },
            { "yellow", new COLORREF(249, 241, 165) },
            { "white", new COLORREF(242, 242, 242) },
        };

        private static readonly Dictionary<string, COLORREF> VintageScheme = new()
        {
            { "black", new COLORREF(0, 0, 0) },
            { "darkBlue", new COLORREF(32, 32, 180) },
            { "darkGreen", new COLORREF(0, 128, 0) },
            { "darkCyan", new COLORREF(0, 128, 128) },
            { "darkRed", new COLORREF(180, 32, 32) },
            { "darkMagenta", new COLORREF(160, 32, 160) },
            { "darkYellow", new COLORREF(128, 128, 0) },
            { "gray", new COLORREF(166, 166, 166) },
            { "darkGray", new COLORREF(78, 86, 102) },
            { "blue", new COLORREF(32, 32, 255) },
            { "green", new COLORREF(0, 255, 0) },
            { "cyan", new COLORREF(0, 255, 255) },
            { "red", new COLORREF(255, 0, 0) },
            { "magenta", new COLORREF(255, 32, 255) },
            { "yellow", new COLORREF(255, 255, 32) },
            { "white", new COLORREF(255, 255, 255) },
        };

        private static readonly Dictionary<string, COLORREF> RaspberryScheme = new()
        {
            { "black", new COLORREF(42, 2, 15) },
            { "darkBlue", new COLORREF(1, 112, 197) },
            { "darkGreen", new COLORREF(118, 171, 35) },
            { "darkCyan", new COLORREF(63, 141, 131) },
            { "darkRed", new COLORREF(189, 9, 64) },
            { "darkMagenta", new COLORREF(125, 73, 143) },
            { "darkYellow", new COLORREF(224, 222, 72) },
            { "gray", new COLORREF(177, 182, 189) },
            { "darkGray", new COLORREF(40, 42, 46) },
            { "blue", new COLORREF(128, 200, 255) },
            { "green", new COLORREF(181, 214, 128) },
            { "cyan", new COLORREF(138, 190, 183) },
            { "red", new COLORREF(189, 109, 133) },
            { "magenta", new COLORREF(172, 121, 187) },
            { "yellow", new COLORREF(255, 253, 118) },
            { "white", new COLORREF(255, 255, 253) },
        };
    }

    /// <summary>
    /// Enumeration to select and use specific color scheme for console application.
    /// </summary>
    public enum ConsoleColorScheme
    {
        /// <summary>
        /// Default Scheme - no changes. NOTE: No need to call method with default scheme.
        /// </summary>
        Default = 0,

        /// <summary>
        /// Black background with bright colors.
        /// https://docs.microsoft.com/en-us/windows/terminal/customize-settings/color-schemes#vintage
        /// </summary>
        Vintage = 1,

        /// <summary>
        /// Black background with mild colors.
        /// https://docs.microsoft.com/en-us/windows/terminal/customize-settings/color-schemes#campbell
        /// </summary>
        Campbell = 2,

        /// <summary>
        /// Black (light) as background with same color for dark and bright versions, except gray/white.
        /// Foreground (default) is Gray.
        /// https://docs.microsoft.com/en-us/windows/terminal/customize-settings/color-schemes#one-half-dark
        /// </summary>
        HalfDark = 3,

        /// <summary>
        /// Ubuntu styled - Red-ish background.
        /// Foreground (default) is Gray.
        /// https://docs.microsoft.com/en-us/windows/terminal/customize-settings/color-schemes#one-half-dark
        /// </summary>
        Raspberry = 4,
    }

    /// <summary>
    /// Code taken from https://github.com/tomakita/Colorful.Console .
    /// Exposes methods used for mapping System.Drawing.Colors to System.ConsoleColors.
    /// Based on code that was originally written by Alex Shvedov, and that was then modified by MercuryP.
    /// </summary>
    internal sealed class ColorMapper
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct COORD
        {
            internal short X;
            internal short Y;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct SMALL_RECT
        {
            internal short Left;
            internal short Top;
            internal short Right;
            internal short Bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct CONSOLE_SCREEN_BUFFER_INFO_EX
        {
            internal int cbSize;
            internal COORD dwSize;
            internal COORD dwCursorPosition;
            internal ushort wAttributes;
            internal SMALL_RECT srWindow;
            internal COORD dwMaximumWindowSize;
            internal ushort wPopupAttributes;
            internal bool bFullscreenSupported;
            internal COLORREF black;
            internal COLORREF darkBlue;
            internal COLORREF darkGreen;
            internal COLORREF darkCyan;
            internal COLORREF darkRed;
            internal COLORREF darkMagenta;
            internal COLORREF darkYellow;
            internal COLORREF gray;
            internal COLORREF darkGray;
            internal COLORREF blue;
            internal COLORREF green;
            internal COLORREF cyan;
            internal COLORREF red;
            internal COLORREF magenta;
            internal COLORREF yellow;
            internal COLORREF white;
        }

        private const int STD_OUTPUT_HANDLE = -11;                               // per WinBase.h
        private static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);    // per WinBase.h

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool GetConsoleScreenBufferInfoEx(IntPtr hConsoleOutput, ref CONSOLE_SCREEN_BUFFER_INFO_EX csbe);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleScreenBufferInfoEx(IntPtr hConsoleOutput, ref CONSOLE_SCREEN_BUFFER_INFO_EX csbe);

        /// <summary>
        /// Gets a collection of all 16 colors in the console buffer.
        /// </summary>
        /// <returns>Returns all 16 COLORREFs in the console buffer as a dictionary keyed by the COLORREF's alias
        /// in the buffer's ColorTable.</returns>
        public Dictionary<string, COLORREF> GetBufferColors()
        {
            var colors = new Dictionary<string, COLORREF>();
            IntPtr hConsoleOutput = GetStdHandle(STD_OUTPUT_HANDLE);    // 7
            CONSOLE_SCREEN_BUFFER_INFO_EX csbe = GetBufferInfo(hConsoleOutput);

            colors.Add("black", csbe.black);
            colors.Add("darkBlue", csbe.darkBlue);
            colors.Add("darkGreen", csbe.darkGreen);
            colors.Add("darkCyan", csbe.darkCyan);
            colors.Add("darkRed", csbe.darkRed);
            colors.Add("darkMagenta", csbe.darkMagenta);
            colors.Add("darkYellow", csbe.darkYellow);
            colors.Add("gray", csbe.gray);
            colors.Add("darkGray", csbe.darkGray);
            colors.Add("blue", csbe.blue);
            colors.Add("green", csbe.green);
            colors.Add("cyan", csbe.cyan);
            colors.Add("red", csbe.red);
            colors.Add("magenta", csbe.magenta);
            colors.Add("yellow", csbe.yellow);
            colors.Add("white", csbe.white);

            return colors;
        }

        /// <summary>
        /// Sets all 16 colors in the console buffer using colors supplied in a dictionary.
        /// </summary>
        /// <param name="colors">A dictionary containing COLORREFs keyed by the COLORREF's alias in the buffer's 
        /// ColorTable.</param>
        public static void SetBatchBufferColors(Dictionary<string, COLORREF> colors)
        {
            IntPtr hConsoleOutput = GetStdHandle(STD_OUTPUT_HANDLE); // 7
            CONSOLE_SCREEN_BUFFER_INFO_EX csbe = GetBufferInfo(hConsoleOutput);

            csbe.black = colors["black"];
            csbe.darkBlue = colors["darkBlue"];
            csbe.darkGreen = colors["darkGreen"];
            csbe.darkCyan = colors["darkCyan"];
            csbe.darkRed = colors["darkRed"];
            csbe.darkMagenta = colors["darkMagenta"];
            csbe.darkYellow = colors["darkYellow"];
            csbe.gray = colors["gray"];
            csbe.darkGray = colors["darkGray"];
            csbe.blue = colors["blue"];
            csbe.green = colors["green"];
            csbe.cyan = colors["cyan"];
            csbe.red = colors["red"];
            csbe.magenta = colors["magenta"];
            csbe.yellow = colors["yellow"];
            csbe.white = colors["white"];

            SetBufferInfo(hConsoleOutput, csbe);
        }

        private static CONSOLE_SCREEN_BUFFER_INFO_EX GetBufferInfo(IntPtr hConsoleOutput)
        {
            var csbe = new CONSOLE_SCREEN_BUFFER_INFO_EX();
            csbe.cbSize = Marshal.SizeOf(csbe); // 96 = 0x60
            if (hConsoleOutput == INVALID_HANDLE_VALUE)
            {
                throw CreateException(Marshal.GetLastWin32Error());
            }

            bool brc = GetConsoleScreenBufferInfoEx(hConsoleOutput, ref csbe);
            if (!brc)
            {
                throw CreateException(Marshal.GetLastWin32Error());
            }

            return csbe;
        }

        private static void SetBufferInfo(IntPtr hConsoleOutput, CONSOLE_SCREEN_BUFFER_INFO_EX csbe)
        {
            csbe.srWindow.Bottom++;
            csbe.srWindow.Right++;

            bool brc = SetConsoleScreenBufferInfoEx(hConsoleOutput, ref csbe);
            if (!brc)
            {
                throw CreateException(Marshal.GetLastWin32Error());
            }
        }

        private static Exception CreateException(int errorCode) =>
            errorCode == 6 //ERROR_INVALID_HANDLE
                ? new Exception("Color conversion failed because a handle to the actual windows console was not found.")
                : new Exception(string.Format("Color conversion failed with system error code {0}!", errorCode));
    }

    /// <summary>
    /// A Win32 COLORREF, used to specify an RGB color.  See MSDN for more information:
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/dd183449(v=vs.85).aspx
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct COLORREF
    {
        private readonly uint _colorDWORD;

        internal COLORREF(uint r, uint g, uint b) => _colorDWORD = r + (g << 8) + (b << 16);

        /// <summary>
        /// COLOREF string representation.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => _colorDWORD.ToString();
    }
#pragma warning restore IDE1006 // Naming Styles
}
