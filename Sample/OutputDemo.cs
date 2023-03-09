using Salix.Extensions;

namespace Demo;

public class OutputDemo : IConsoleOperation
{
    public string OperationName => "output";

    public string HelpText =>
        "Command to show possibilities of output extensions.";

    public Task<int> DoWork()
    {
        var items = new HashSet<string>
        {
            "Output yellow text",
            "Output yellow text on red background",
            "Output interpolated bi-color text",
            "Spinner - Long process busy indicator",
            "Progress bar - stepped process visualization",
            "Exit output demo"
        };

        bool shouldExit = false;
        while (!shouldExit)
        {
            int selectedItem = Consolix.MenuSelectSingle(items);
            Consolix.WriteLine("Selected item index: {0}", selectedItem, ConsoleColor.Gray, ConsoleColor.Cyan);

            switch (selectedItem)
            {
                case 0:
                    OutputColoredText();
                    break;
                case 1:
                    OutputColoredTextWithBackground();
                    break;
                case 2:
                    OutputInterpolatedText();
                    break;
                case 3:
                    OutputSpinner();
                    break;
                case 4:
                    OutputProgress();
                    break;
                default:
                    shouldExit = true;
                    break;
            }
        }

        return Task.FromResult(0);
    }

    private static void OutputColoredText()
    {
        Consolix.WriteLine();
        Consolix.WriteLine("This text output is colored yellow.", ConsoleColor.Yellow);
        Consolix.WriteLine("Used method: ");
        Consolix.Write("Consolix", ConsoleColor.Blue);
        Consolix.Write(".");
        Consolix.Write("WriteLine(", ConsoleColor.DarkCyan);
        Consolix.Write("\"This text output is colored yellow.\"", ConsoleColor.DarkRed);
        Consolix.Write(", ");
        Consolix.Write("ConsoleColor", ConsoleColor.DarkCyan);
        Consolix.Write(".");
        Consolix.Write("Yellow", ConsoleColor.DarkYellow);
        Consolix.Write(");");
        Consolix.WriteLine();
        Consolix.WriteLine();
    }

    private static void OutputColoredTextWithBackground()
    {
        Consolix.WriteLine();
        Consolix.WriteLine("This is emphasized problem!", ConsoleColor.Yellow, ConsoleColor.Red);
        Consolix.WriteLine("Used method: ");
        Consolix.Write("Consolix", ConsoleColor.Blue);
        Consolix.Write(".");
        Consolix.Write("WriteLine(", ConsoleColor.DarkCyan);
        Consolix.Write("\"This is emphasized problem!\"", ConsoleColor.DarkRed);
        Consolix.Write(", ");
        Consolix.Write("ConsoleColor", ConsoleColor.DarkCyan);
        Consolix.Write(".");
        Consolix.Write("Yellow", ConsoleColor.DarkYellow);
        Consolix.Write(", ");
        Consolix.Write("ConsoleColor", ConsoleColor.DarkCyan);
        Consolix.Write(".");
        Consolix.Write("Red", ConsoleColor.DarkRed);
        Consolix.Write(");");
        Consolix.WriteLine();
        Consolix.WriteLine();
    }

    private static void OutputInterpolatedText()
    {
        Consolix.WriteLine();
        Consolix.WriteLine("Children can count from {0} to {1} in {2} seconds.", ConsoleColor.DarkCyan, ConsoleColor.Cyan, 1, 100, 45);
        Consolix.WriteLine("Used method: ");
        Consolix.Write("Consolix", ConsoleColor.Blue);
        Consolix.Write(".");
        Consolix.Write("WriteLine(", ConsoleColor.DarkCyan);
        Consolix.Write("\"Children can count from {0} to {1} in {2} seconds.\"", ConsoleColor.DarkRed);
        Consolix.Write(", ");
        Consolix.Write("ConsoleColor", ConsoleColor.DarkCyan);
        Consolix.Write(".");
        Consolix.Write("DarkCyan", ConsoleColor.DarkCyan);
        Consolix.Write(", ");
        Consolix.Write("ConsoleColor", ConsoleColor.DarkCyan);
        Consolix.Write(".");
        Consolix.Write("Cyan", ConsoleColor.Cyan);
        Consolix.Write(", 1,100,45);");
        Consolix.WriteLine();
        Consolix.WriteLine();
    }

    private static void OutputSpinner()
    {
        using var spinner = new Spinner(SpinnerType.Cross, showTime: true, color: ConsoleColor.Green);
        var stepCounter = 0;
        spinner.AddProgressBar(4); // Draw also progress bar with total of 4 steps in process.

        spinner.Start("Process initialization");
        Thread.Sleep(3123); // These mimics some lengthy process
        spinner.SetProgressMessage("Opening connections", ++stepCounter);
        Thread.Sleep(2132);
        spinner.SetProgressMessage("Doing Work", ++stepCounter);
        Thread.Sleep(5174);
        spinner.SetProgressMessage("Teardown/Cleanup", ++stepCounter);
        Thread.Sleep(2163);

        spinner.Stop("Spinner work is finished.", ConsoleColor.Yellow);
    }

    private static void OutputProgress()
    {
        using var progressBar = new ProgressBar(69, color: ConsoleColor.Green);

        var progressBarLine = CursorPosition.Current();
        Consolix.WriteLine();
        Consolix.Write("Current iteration: ", ConsoleColor.Gray);
        var iterationPosition = CursorPosition.Current();
        progressBarLine.MoveTo();

        progressBar.Start();
        for (int stepCounter = 0; stepCounter < 69; stepCounter++)
        {
            progressBar.CurrentStep = stepCounter;
            Consolix.Write(stepCounter, ConsoleColor.Cyan, iterationPosition);
            Thread.Sleep(Random.Shared.Next(10, 500));
        }

        progressBar.Stop();
        Consolix.WriteLine("Progress bar process ended in {0} sec.", progressBar.ElapsedTime.ToString(@"ss\.fff"), ConsoleColor.DarkYellow, ConsoleColor.Yellow);
    }

    public bool IsReady => true;
}
