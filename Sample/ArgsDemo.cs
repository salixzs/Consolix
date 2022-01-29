using System.Globalization;
using Salix.Extensions;

namespace Demo
{
    public class ArgsDemo : IConsoleOperation
    {
        public string OperationName => "args";
        public string HelpText => "Demo of values passing to command properties via command-line args.";

        [ConsoleOption(
            "readycheck",
            "Mandatory boolean parameter, which must be specified for command to run",
            "rc")]
        public bool ReadyCheck { get; set; } = false;

        [ConsoleOption(
            "text",
            "Optional argument to specify some text",
            "t")]
        public string? TextArg { get; set; }

        [ConsoleOption(
            "number",
            "Optional argument to specify some integer",
            "n")]
        public int? NumberArg { get; set; }

        [ConsoleOption(
            "date",
            "Optional argument to specify some date",
            "d")]
        public DateTime? DateArg { get; set; }

        [ConsoleOption(
            "enum",
            "Optional argument as Enum to specify its value (Tango, Salsa, Waltz, Modern)",
            "e")]
        public Dances? EnumArg { get; set; }

        public Task<int> DoWork()
        {
            if (string.IsNullOrEmpty(this.TextArg))
            {
                Consolix.WriteLine("Text was not given.");
            }
            else
            {
                Consolix.WriteLine("Given text: {0}", ConsoleColor.Gray, ConsoleColor.White, this.TextArg);
            }

            if (this.NumberArg == null)
            {
                Consolix.WriteLine("Number was not given.");
            }
            else
            {
                Consolix.WriteLine("Given number: {0}", ConsoleColor.Gray, ConsoleColor.White, this.NumberArg);
            }

            if (this.DateArg == null)
            {
                Consolix.WriteLine("Date was not given.");
            }
            else
            {
                Consolix.WriteLine("Given date: {0}", ConsoleColor.Gray, ConsoleColor.White, this.DateArg.Value.ToString("D", new CultureInfo("en-US")));
            }

            if (this.EnumArg == null)
            {
                Consolix.WriteLine("Enum was not given.");
            }
            else
            {
                Consolix.WriteLine("Given enum: {0}", ConsoleColor.Gray, ConsoleColor.White, this.EnumArg.ToString());
            }

            return Task.FromResult(0);
        }

        public bool IsReady => this.ReadyCheck;
    }
}
