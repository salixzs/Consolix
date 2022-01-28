using System.Diagnostics;

namespace Salix.Extensions;

/// <summary>
/// Draws a spinning/growing/swapping busy indicator with optional message and execution time for lengthy operations.
/// </summary>
public class Spinner : IDisposable
{
    private readonly Thread _spinnerThread; // Separate thread to run spinner itself
    private readonly SpinnerType _spinnerType;
    private bool _active; // Flag, indicating whether spinner is in use
    private int _counter; // Decides which symbol for spinner to output
    private readonly int _delay; // Delays between spinner spins (milliseconds)
    private readonly ConsoleColor _color; // Color for outputting spinner
    private string _message = ""; // Optional message to show with spinner

    private readonly bool _showTime; // To show or not the elapsed time
    private readonly Stopwatch _timer;

    private bool _drawProgressBar = false;
    private int _totalSteps; // ProgressBar - how many steps the work will take to complete
    private int _currentStep = 0;

    private readonly string[] _sequence;
    private readonly string[] _sequenceSlash = new string[] { "/", "-", "\\", "|" };
    private readonly string[] _sequenceCircle = new string[] { ".", "o", "O", "o" };
    private readonly string[] _sequenceCross = new string[] { "+", "x" };
    private readonly string[] _sequenceCube = new string[] { "▄", "▀" };
    private readonly string[] _sequenceGrowDots = new string[] { ".   ", "..  ", "... ", "...." };
    private readonly string[] _sequenceGrowArrow = new string[] { "=>   ", "==>  ", "===> ", "====>" };

    /// <summary>
    /// Optional message to output current operation status with spinner.
    /// </summary>
    public string Message
    {
        get => _message;
        set => _message = value.Length > 40 ? value.Substring(0, 37) + "..." : value;
    }

    /// <summary>
    /// With progress bar usage will set current step to calculate and display its completion percentage.
    /// </summary>
    public int CurrentStep
    {
        get => _currentStep;
        set => _currentStep = value > _totalSteps ? _totalSteps : value;
    }

    /// <summary>
    /// Holds elapsed time since spinner start.
    /// Can use to summarize all used time during entire process.
    /// Read resulting time right after calling one of <see cref="Stop()"/> methods.
    /// </summary>
    public TimeSpan ElapsedTime => _timer.Elapsed;

    /// <summary>
    /// Usable with ProgressBar. Sets both status message and current step to display.
    /// </summary>
    /// <param name="message">Long operation status message.</param>
    /// <param name="step">Current step in process to calculate progress bar percentage.</param>
    public void SetProgressMessage(string message, int step)
    {
        this.Message = message;
        this.CurrentStep = step;
    }

    /// <summary>
    /// Creates a Busy indicator as spinner with some optional parameters.
    /// </summary>
    /// <param name="type">Type of the spinner. Default - lines (\ | / -).</param>
    /// <param name="delay">Delay between spinner changes (milliseconds). Default - 200 (1/4 sec).</param>
    /// <param name="showTime">Whether to show operation length time ([mm:ss]. Default - do not show.</param>
    /// <param name="color">Specify Color to use for spinner and [optional] message(s). Default - default color (Gray).</param>
    public Spinner(SpinnerType type = SpinnerType.Lines, int delay = 300, bool showTime = false, ConsoleColor color = ConsoleColor.Gray)
    {
        _spinnerType = type;
        _delay = delay;
        _showTime = showTime;
        _color = color;
        _timer = new Stopwatch();
        _sequence = type switch
        {
            SpinnerType.Circles => _sequenceCircle,
            SpinnerType.Cross => _sequenceCross,
            SpinnerType.Cubes => _sequenceCube,
            SpinnerType.GrowingDots => _sequenceGrowDots,
            SpinnerType.GrowingArrow => _sequenceGrowArrow,
            SpinnerType.Lines => _sequenceSlash,
            _ => _sequenceSlash
        };
        _spinnerThread = new Thread(this.Spin);
    }

    /// <summary>
    /// Puts a spinner inside small progress bar.
    /// Only usable with non-growing spinners (Lines, Circles, Cubes, Cross).
    /// </summary>
    /// <param name="totalSteps"></param>
    public void AddProgressBar(int totalSteps)
    {
        if (_active)
        {
            return;
        }

        if (_spinnerType is SpinnerType.GrowingDots or SpinnerType.GrowingArrow)
        {
            throw new NotSupportedException("Progress bar is not supported with growing spinner types");
        }

        _drawProgressBar = true;
        _totalSteps = totalSteps;
    }

    /// <summary>
    /// Starts the spinner.
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
    /// Starts a spinner with initial message.
    /// </summary>
    /// <param name="message"></param>
    public void Start(string message)
    {
        this.Message = message;
        this.Start();
    }

    /// <summary>
    /// Stops the spinner, clears spinner.
    /// </summary>
    public void Stop()
    {
        this.StopProcess();
        Consolix.ClearLine();
    }

    /// <summary>
    /// Stops the spinner and displays given end message in its place.
    /// It will add elapsed time at the end of message.
    /// </summary>
    /// <param name="endMessage">Message to show when process ends (work complete).</param>
    /// <param name="color">Specify end message color. Default = default color (gray).</param>
    public void Stop(string endMessage, ConsoleColor color = ConsoleColor.Gray)
    {
        this.StopProcess();
        Consolix.WriteLineOver(endMessage + " [" + _timer.Elapsed.ToString(@"mm\:ss\.f") + "]", color);
    }

    /// <summary>
    /// Common code for stopping spinner - all internal properties reset and stopping.
    /// </summary>
    private void StopProcess()
    {
        _active = false;
        Console.CursorVisible = true;
        this.Message = "";
        _timer.Stop();
    }

    private void Spin()
    {
        while (_active)
        {
            if (_drawProgressBar)
            {
                this.DrawProgress();
            }
            else
            {
                this.Draw();
            }

            Thread.Sleep(_delay);
        }
    }

    /// <summary>
    /// Draws the busy/spinner indicator of the chosen type.
    /// </summary>
    private void Draw()
    {
        Consolix.ClearLine();
        if (!string.IsNullOrWhiteSpace(this.Message) && _spinnerType is SpinnerType.GrowingArrow or SpinnerType.GrowingDots)
        {
            Consolix.Write(this.Message + " ", _color);
        }

        if (_showTime && _spinnerType is SpinnerType.GrowingArrow or SpinnerType.GrowingDots)
        {
            Consolix.Write($"[{_timer.Elapsed:mm\\:ss}] ", _color);
        }

        Consolix.Write(_sequence[_counter], _color);

        if (_showTime && _spinnerType != SpinnerType.GrowingArrow && _spinnerType != SpinnerType.GrowingDots)
        {
            Consolix.Write($" [{_timer.Elapsed:mm\\:ss}]", _color);
        }

        if (!string.IsNullOrWhiteSpace(this.Message) && _spinnerType != SpinnerType.GrowingArrow && _spinnerType != SpinnerType.GrowingDots)
        {
            Consolix.Write(" " + this.Message, _color);
        }

        _counter++;
        if (_counter >= _sequence.Length)
        {
            _counter = 0;
        }
    }

    /// <summary>
    /// Draws the busy/spinner indicator inside progress bar.
    /// </summary>
    private void DrawProgress()
    {
        Consolix.ClearLine();

        if (_showTime && _spinnerType is SpinnerType.GrowingArrow or SpinnerType.GrowingDots)
        {
            Consolix.Write($"[{_timer.Elapsed:mm\\:ss}] ", _color);
        }

        var progressPercentage = (decimal)this.CurrentStep / _totalSteps * 100;
        var fillCharacterCount = (int)Math.Round(0.1M * progressPercentage);
        if (fillCharacterCount == 10)
        {
            fillCharacterCount = 9;
        }

        // DEBUG: Consolix.Write($" (Curr:{this.CurrentStep}; Tot:{_totalSteps}; %:{progressPercentage}; #:{fillCharacterCount}) ");

        Consolix.Write("[" + new string('#', fillCharacterCount), _color);
        Consolix.Write(_sequence[_counter], _color);
        if (fillCharacterCount < 9)
        {
            Consolix.Write(new string('-', 9 - fillCharacterCount) + "] ", _color);
        }
        else
        {
            Consolix.Write("] ", _color);
        }

        Consolix.Write($"{progressPercentage:0.#}%", _color);

        if (!string.IsNullOrWhiteSpace(this.Message))
        {
            Consolix.Write($" : {this.Message}", _color);
        }

        _counter++;
        if (_counter >= _sequence.Length)
        {
            _counter = 0;
        }
    }

    /// <summary>
    /// Stops the spinner and Disposes its instance.
    /// </summary>
    public void Dispose() => this.Stop();
}
