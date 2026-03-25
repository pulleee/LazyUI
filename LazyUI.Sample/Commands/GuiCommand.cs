using LazyUI.Sample.Panels;
using Spectre.Console.Cli;

namespace LazyUI.Sample.Commands;

public sealed class GuiCommand : AsyncCommand<GuiCommand.Settings>
{
    public sealed class Settings : CommandSettings
    {
    }

    private readonly GuiContext _guiContext;

    public GuiCommand(GuiContext guiContext)
    {
        _guiContext = guiContext;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        _guiContext.SetContent(new SplashPanel());

        await _guiContext.Draw();

        return 0;
    }
}
