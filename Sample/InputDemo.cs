using Salix.Extensions;

namespace Demo;

public class InputDemo : IConsoleOperation
{
    public string OperationName => "input";

    public string HelpText =>
        "Method to show possibilities of input extensions.";

    public Task<int> DoWork()
    {
        var items = new HashSet<string>
        {
            "Enter password",
            "Wait for Enter",
            "Wait for Escape",
            "Wait for y/n",
            "Wait for specific keys",
            "Wait for special keys",
            "Exit output demo"
        };

        bool shouldExit = false;
        while (!shouldExit)
        {
            int selectedItem = Consolix.MenuSelectSingle(items);
            Consolix.WriteLine("Selected item index: {0}", selectedItem, ConsoleColor.DarkCyan, ConsoleColor.Cyan);

            switch (selectedItem)
            {
                case 0:
                    Consolix.WriteLine();
                    Consolix.Write("Enter password: ");
                    var psw = Consolix.ReadLineSecret();
                    Consolix.WriteLine($"Password: {psw}");
                    Consolix.WriteLine();
                    break;
                case 1:
                    Consolix.WriteLine();
                    Consolix.WriteLine("Press Enter (all other keys should not respond): ");
                    Consolix.ReadEnter();
                    Consolix.WriteLine();
                    break;
                case 2:
                    Consolix.WriteLine();
                    Consolix.WriteLine("Press Escape (all other keys should not respond): ");
                    Consolix.ReadEscape();
                    Consolix.WriteLine();
                    break;
                case 3:
                    Consolix.WriteLine();
                    Consolix.WriteLine("Press 'y' or 'n' (all other keys should not respond): ");
                    var yn = Consolix.ReadYesNo();
                    Consolix.WriteLine($"Pressed {(yn ? "Yes" : "No")}");
                    Consolix.WriteLine();
                    break;
                case 4:
                    Consolix.WriteLine();
                    Consolix.WriteLine("Press 'f' or 'j' (all other keys should not respond): ");
                    var fj = Consolix.ReadKeys(true, 'f', 'j');
                    Consolix.WriteLine($"Pressed '{fj}'");
                    Consolix.WriteLine();
                    break;
                case 5:
                    Consolix.WriteLine();
                    Consolix.WriteLine("Press 'Backspace' or 'Delete' (all other keys should not respond): ");
                    var ctrl = Consolix.ReadKeys(ConsoleKey.Backspace, ConsoleKey.Delete);
                    Consolix.WriteLine($"Pressed '{ctrl}'");
                    Consolix.WriteLine();
                    break;
                default:
                    shouldExit = true;
                    break;
            }
        }

        return Task.FromResult(0);
    }

    public bool IsReady => true;
}
