using LazyUI.Sample.Panels;
using Spectre.Console.Cli;

namespace LazyUI.Sample.Commands;

public class CubeCommand : Command<CubeCommand.Settings>
{
    public sealed class Settings : CommandSettings
    {
    }

    private readonly GuiContext _guiContext;

    public CubeCommand(GuiContext guiContext)
    {
        _guiContext = guiContext;
    }

    public override int Execute(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        var cubePanel = new CubePanel();
        _guiContext.SetContent(cubePanel);

        return 0;
    }
}
