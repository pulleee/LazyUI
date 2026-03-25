using LazyUI;
using LazyUI.Sample.Commands;
using LazyUI.Sample.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;
using Spectre.Console.Cli;

namespace PTConsole
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateGuiCommandApp().RunAsync(args);
        }

        private static CommandApp CreateGuiCommandApp()
        {
            var services = new ServiceCollection();

            services.AddSingleton<GuiContext>();
            services.AddSingleton<GuiCommandDispatcher>(sp =>
            {
                var innerServices = new ServiceCollection();
                var capturingConsole = new CapturingConsole();

                innerServices.AddSingleton<IAnsiConsole>(capturingConsole);

                // Load GuiContext lazily in inner application
                innerServices.AddSingleton(_ => sp.GetRequiredService<GuiContext>());

                var innerApp = new CommandApp(new TypeRegistrar(innerServices));
                innerApp.Configure(config =>
                {
                    config.Settings.Console = capturingConsole;

                    config.AddCommand<OutputCommand>("output");
                    config.AddCommand<ClockCommand>("clock");
                    config.AddCommand<CubeCommand>("cube");
                });

                return new GuiCommandDispatcher(innerApp, capturingConsole);
            });

            var app = new CommandApp(new TypeRegistrar(services));
            app.Configure(config =>
            {
                config.AddCommand<GuiCommand>("gui");
            });

            return app;
        }
    }
}