using LazyUI.Layouts;
using Spectre.Console.Rendering;

namespace LazyUI;

public class GuiContext
{
    private LazyLayout _layout;

    private int _lastWidth;
    private int _lastHeight;

    private readonly GuiCommandDispatcher _dispatcher;

    public GuiContext(GuiCommandDispatcher dispatcher)
    {
        _dispatcher = dispatcher;

        _layout = new LazyLayout(dispatcher);
    }

    public void SetContent(IRenderable content) => _layout.SetContent(content);
    public void ShowSlot(string name) => _layout.ShowSlot(name);
    public void HideSlot(string name) => _layout.HideSlot(name);
    public bool IsSlotVisible(string name) => _layout.IsSlotVisible(name);

    public async Task Draw()
    {
        Console.Write(AnsiUtil.USE_ALTERNATE_SCREEN_BUFFER);
        Console.Write(AnsiUtil.SET_CURSOR_DEFAULT);
        Console.CursorVisible = false;
        Console.TreatControlCAsInput = true;

        try
        {
            _lastWidth = Console.WindowWidth;
            _lastHeight = Console.WindowHeight;

            // Force initial render
            _layout.Render();
            _layout.StartReadingKeys();

            while (!_dispatcher.ExitRequested)
            {
                var w = Console.WindowWidth;
                var h = Console.WindowHeight;
                bool resized = w != _lastWidth || h != _lastHeight;

                if (resized)
                {
                    _lastWidth = w;
                    _lastHeight = h;
                }

                Console.Write(AnsiUtil.USE_SPECIAL_RENDERING_MODE + AnsiUtil.SET_CURSOR_DEFAULT);
                _layout.Render();
                Console.Write(AnsiUtil.CLEAR_FROM_CURSOR_TO_END + AnsiUtil.END_SPECIAL_RENDERING_MODE);

                await Task.Delay(7);
            }
        }
        finally
        {
            _layout.StopReadingKeys();

            Console.TreatControlCAsInput = false;
            Console.Write(AnsiUtil.END_ALTERNATE_SCREEN_BUFFER);
            Console.CursorVisible = true;
        }
    }
}
