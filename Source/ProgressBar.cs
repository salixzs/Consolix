using System.Diagnostics;

namespace Salix.Extensions;

/// <summary>
/// Draws a Progress Bar on screen in 80 character width with percentage and elapsed time display.
/// WIll overwrite current line text, so invoke it on a new line.
/// </summary>
public class ProgressBar : IDisposable
{
    private readonly Thread _spinnerThread; // Separate thread to run spinner itself
    private bool _active; // Flag, indicating whether spinner is in use
    private readonly ConsoleColor _color; // Color for outputting spinner
    private readonly Stopwatch _timer;

    /// <summary>
    /// With progress bar usage will set current step to calculate and display its completion percentage.
    /// </summary>
    public int CurrentStep
    {
        get => _currentStep;
        set => _currentStep = value > _totalSteps ? _totalSteps : value;
    }

    private int _totalSteps; // ProgressBar - how many steps the work will take to complete
    private int _currentStep = 0;

    /// <summary>
    /// Holds elapsed time since progress bar drawing start.
    /// Can use to summarize all used time during entire process.
    /// Read resulting time right after calling one of <see cref="Stop()"/> methods.
    /// </summary>
    public TimeSpan ElapsedTime => _timer.Elapsed;

    /// <summary>
    /// Creates a Progress bar.
    /// </summary>
    /// <param name="color">Specify Color to use for spinner and [optional] message(s). Default - default color (Gray).</param>
    public ProgressBar(int totalStepCount, ConsoleColor color = ConsoleColor.Gray)
    {
        if (totalStepCount < 2)
        {
            throw new ArgumentException("Progress Bar created with total steps less than 2 (Pointless).");
        }

        _totalSteps = totalStepCount;
        _color = color;
        _timer = new Stopwatch();
        _spinnerThread = new Thread(this.Step);
    }

    private void Step()
    {
        while (_active)
        {
            this.Draw();
            Thread.Sleep(1000);
        }
    }

    /// <summary>
    /// Starts the Progress bar drawing.
    /// </summary>
    public void Start()
    {
        if (_active)
        {
            return;
        }

        _active = true;
        Console.CursorVisible = false;
        _timer.Restart();

        if (!_spinnerThread.IsAlive)
        {
            _spinnerThread.Start();
        }
    }

    /// <summary>
    /// Stops the progress bar drawing and clears it from screen.
    /// </summary>
    public void Stop()
    {
        _active = false;
        Console.CursorVisible = true;
        _timer.Stop();
        Consolix.ClearLine();
    }

    /// <summary>
    /// Draws the Progress Bar on screen.
    /// </summary>
    private void Draw()
    {
        // 1                               33            47
        // |###################------------| 25% (00:12) |--------------------------------|
        var progressPercentage = (decimal)this.CurrentStep / _totalSteps * 100;
        var fillCharacterCount = (int)Math.Round(0.78M * progressPercentage);

        Consolix.ClearLine();
        Consolix.Write("[" + new string('#', fillCharacterCount), _color);
        if (fillCharacterCount < 78)
        {
            Consolix.Write(new string('-', 78 - fillCharacterCount) + "]", _color);
        }
        else
        {
            Consolix.Write("] ", _color);
        }

        Consolix.CursorToPosition(33);
        Consolix.Write($"| {progressPercentage:00}% ({_timer.Elapsed.ToString(@"mm\:ss")}) |", _color);
    }

    public void Dispose() => this.Stop();
}
