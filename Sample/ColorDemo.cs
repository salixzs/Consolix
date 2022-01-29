using System.Globalization;
using Salix.Extensions;

namespace Demo;

public class ColorDemo : IConsoleOperation
{
    [ConsoleOption(
        "scheme",
        "Color scheme to use: Default, Vintage, Campbell, HalfDark or Raspberry",
        "s")]
    public string Scheme { get; set; }

    public string OperationName => "colors";

    public string HelpText =>
        "Command to show color scheme variations on console output.";

    public Task<int> DoWork()
    {
        switch (this.Scheme.ToUpper(CultureInfo.InvariantCulture))
        {
            case "VINTAGE":
                Consolix.SetColorScheme(ConsoleColorScheme.Vintage);
                break;
            case "CAMPBELL":
                Consolix.SetColorScheme(ConsoleColorScheme.Campbell);
                break;
            case "HALFDARK":
                Consolix.SetColorScheme(ConsoleColorScheme.HalfDark);
                break;
            case "RASPBERRY":
                Consolix.SetColorScheme(ConsoleColorScheme.Raspberry);
                break;
            default:
                Consolix.SetColorScheme(ConsoleColorScheme.Default);
                break;
        }

        this.DrawColorGrid();

        return Task.FromResult(0);
    }

    private void DrawColorGrid()
    {
        Consolix.WriteLine("Background | Foreground colors", ConsoleColor.Gray, ConsoleColor.Black);
        Consolix.WriteLine("---------------------------------------------------------------------", ConsoleColor.Gray, ConsoleColor.Black);
        foreach (var background in DarkColors)
        {
            Consolix.Write("Dark       | ", ConsoleColor.Gray, ConsoleColor.Black);
            foreach (var darkColor in DarkColors)
            {
                Consolix.Write($" [{darkColor.Name.Replace("Dark ", string.Empty).Substring(0, 3)}] ", darkColor.Color, background.Color);
            }

            Console.WriteLine();
            Consolix.Write($"{background.Name.Replace("Dark ", string.Empty),-11}| ", ConsoleColor.Gray, ConsoleColor.Black);
            foreach (var lightColor in LightColors)
            {
                Consolix.Write($" [{lightColor.Name[..3]}] ", lightColor.Color, background.Color);
            }
            Console.WriteLine();
            Consolix.WriteLine("---------------------------------------------------------------------", ConsoleColor.Gray, ConsoleColor.Black);
        }

        Consolix.WriteLine();
        Consolix.WriteLine("Green text indicating some success", ConsoleColor.Green);
        Consolix.WriteLine("This is problem message", ConsoleColor.Red);
        Consolix.WriteLine("This is fatal problem message", ConsoleColor.Yellow, ConsoleColor.Red);
        Consolix.WriteLine("This is String.Format prompt with: {0}", "value", ConsoleColor.DarkCyan, ConsoleColor.Cyan);
        Consolix.WriteLine("This is {0} with {1} placeholders in {2}", ConsoleColor.DarkMagenta, ConsoleColor.Magenta, "string.Format", "multiple", "it");
    }

    public bool IsReady
    {
        get
        {
            if (string.IsNullOrEmpty(this.Scheme))
            {
                return false;
            }

            if (!new HashSet<string> { "DEFAULT", "VINTAGE", "CAMPBELL", "RASPBERRY", "HALFDARK" }
                    .Contains(this.Scheme.ToUpper(CultureInfo.InvariantCulture)))
            {
                return false;
            }

            return true;
        }
    }

    private List<ColorSet> DarkColors = new List<ColorSet>
    {
        new ColorSet("Black", ConsoleColor.Black),
        new ColorSet("Dark Red", ConsoleColor.DarkRed),
        new ColorSet("Dark Green", ConsoleColor.DarkGreen),
        new ColorSet("Dark Yellow", ConsoleColor.DarkYellow),
        new ColorSet("Dark Blue", ConsoleColor.DarkBlue),
        new ColorSet("Dark Magenta", ConsoleColor.DarkMagenta),
        new ColorSet("Dark Cyan", ConsoleColor.DarkCyan),
        new ColorSet("Dark Gray", ConsoleColor.DarkGray),
    };

    private List<ColorSet> LightColors = new List<ColorSet>
    {
        new ColorSet("Gray", ConsoleColor.Gray),
        new ColorSet("Red", ConsoleColor.Red),
        new ColorSet("Green", ConsoleColor.Green),
        new ColorSet("Yellow", ConsoleColor.Yellow),
        new ColorSet("Blue", ConsoleColor.Blue),
        new ColorSet("Magenta", ConsoleColor.Magenta),
        new ColorSet("Cyan", ConsoleColor.Cyan),
        new ColorSet("White", ConsoleColor.White),
    };

    private class ColorSet
    {
        public ColorSet(string name, ConsoleColor color)
        {
            this.Name = name;
            this.Color = color;
        }

        public string Name { get; set; }
        public ConsoleColor Color { get; set; }
    }
}
