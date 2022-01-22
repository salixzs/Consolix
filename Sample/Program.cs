using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Salix.Extensions;

namespace Demo;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        // Set Console application default color scheme (clears console)
        Consolix.SetColorScheme(ConsoleColorScheme.Campbell);

        IHost? host = Host.CreateDefaultBuilder(args)
            .ConfigureLogging(logging => logging.ClearProviders())
            .UseConsoleLifetime()
            .ConfigureServices(SetupContainer)
            .Build();

        try
        {
            var consoleOperationHandler = host.Services.GetRequiredService<ConsoleOperationHandler>();
            consoleOperationHandler.PrepareOperation(args);
            if (args.Contains("--h") || args.Contains("--help"))
            {
                consoleOperationHandler.OutputHelp(
                    typeof(Program).Assembly.GetName().Name,
                    "Shows usage of Consolix extensions for console applications.");
                return 0;
            }

            if (args.Contains("--v") || args.Contains("--version"))
            {
                Consolix.WriteLine("Version: {0}", typeof(Program).Assembly.GetName().Version, ConsoleColor.Gray, ConsoleColor.Cyan);
                return 0;
            }

            if (consoleOperationHandler.SelectedOperation is { IsReady: true })
            {
                // Here operation is called to do its work.
                return await consoleOperationHandler.SelectedOperation.DoWork();
            }

            // Fallback to displaying Help.
            consoleOperationHandler.OutputHelp(
                typeof(Program).Assembly.GetName().Name,
                "Shows usage of Consolix extensions for console applications.");
            return -1;
        }
        catch (Exception ex)
        {
            Consolix.WriteLine(ex.Message, ConsoleColor.Red);
            return -1;
        }
    }

    private static void SetupContainer(HostBuilderContext context, IServiceCollection services)
    {
        // <--- These are demo commands
        services.AddTransient<IConsoleOperation, ColorDemo>();
        services.AddTransient<IConsoleOperation, OutputDemo>();
        services.AddTransient<IConsoleOperation, InputDemo>();
        // --> Demo commands define ends here

        services.AddTransient<ConsoleOperationHandler>();
    }
}
