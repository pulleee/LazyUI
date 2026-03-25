using LazyUI.Panels;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace LazyUI.Layouts
{
    public class LazyLayout
    {
        public const string CONTENT_NAME = "Content";
        public const string INPUT_NAME = "Input";
        public const string OUTPUT_NAME = "Output";

        private Layout _root;

        private IRenderable? _contentPanel;
        private readonly OutputPanel _outputPanel;
        private readonly InputPanel _inputPanel;

        private bool _isCapturingKeys;
        
        public LazyLayout(GuiCommandDispatcher dispatcher)
        {
            // Panels
            _outputPanel = new OutputPanel(dispatcher.Console);
            _inputPanel = new InputPanel(dispatcher);

            // Wrap into caching renderables
            var output = new CachingRenderable(_outputPanel);
            var input = new CachingRenderable(_inputPanel);

            // Build layout
            _root = buildLayout(null, output, input);
        }

        public void Render()
        {
            AnsiConsole.Write(_root);
        }

        public void StartReadingKeys()
        {
            if (_isCapturingKeys) return;

            _isCapturingKeys = true;

            _ = Task.Run(() =>
            {
                var lastInputSize = _inputPanel.GetHeight();

                while (_isCapturingKeys)
                {
                    var key = Console.ReadKey(true);

                    bool ctrl = key.Modifiers.HasFlag(ConsoleModifiers.Control);
                    bool shift = key.Modifiers.HasFlag(ConsoleModifiers.Shift);

                    switch (key.Key)
                    {
                        case ConsoleKey.PageUp:
                            _outputPanel.ScrollUp(Console.WindowHeight);
                            break;
                        case ConsoleKey.PageDown:
                            _outputPanel.ScrollDown(Console.WindowHeight);
                            break;
                        case ConsoleKey.O when ctrl:
                            _root[OUTPUT_NAME].IsVisible = _root[OUTPUT_NAME].IsVisible ? false : true;
                            break;
                        default:
                            _inputPanel.HandleKey(key);

                            var inputSize = _inputPanel.GetHeight();

                            // Resize if text wraps in input panel
                            if (lastInputSize != inputSize)
                            {
                                _root[INPUT_NAME].Size(inputSize);
                                lastInputSize = inputSize;
                            }

                            break;
                    }
                }
            });
        }

        public void StopReadingKeys()
        {
            _isCapturingKeys = false;
        }

        public void SetContent(IRenderable content)
        {
            if (content is IHasDirtyState)
            {
                _contentPanel = new CachingRenderable(content);
            }
            else
            {
                _contentPanel = content;
            }

            _root[CONTENT_NAME].Update(_contentPanel);
        }

        public void ClearContent()
        {
            _contentPanel = null;
        }

        public void ShowSlot(string slotName)
        {
            _root[slotName].IsVisible = true;
        }

        public void HideSlot(string slotName)
        {
            _root[slotName].IsVisible = false;
        }

        public bool IsSlotVisible(string slotName) => _root[slotName].IsVisible;

        private Layout buildLayout(IRenderable content, IRenderable output, IRenderable input, bool showOutput = true)
        {
            var layout = new Layout("Root")
                .SplitRows(
                    new Layout(CONTENT_NAME).Ratio(1),
                    new Layout(OUTPUT_NAME).Ratio(1),
                    new Layout(INPUT_NAME).Size(1));

            layout[CONTENT_NAME].Update(content);
            layout[OUTPUT_NAME].Update(output);
            layout[INPUT_NAME].Update(input);

            layout[OUTPUT_NAME].IsVisible = showOutput;

            return layout;
        }
    }
}
