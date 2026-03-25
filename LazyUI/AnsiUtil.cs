namespace LazyUI
{
    public class AnsiUtil
    {
        /// <summary>
        /// Default ANSI Escape Code 
        /// </summary>
        public const string ESCAPE_CODE = "\x1b";

        /// <summary>
        /// Uses the alternate screen buffer
        /// </summary>
        public const string USE_ALTERNATE_SCREEN_BUFFER = ESCAPE_CODE + "[?1049h";

        /// <summary>
        /// Ends the alternate screen buffer
        /// </summary>
        public const string END_ALTERNATE_SCREEN_BUFFER = ESCAPE_CODE + "[?1049l";

        /// <summary>
        /// Clears the terminal screen
        /// </summary>
        public const string CLEAR_SCREEN = ESCAPE_CODE + "[2J";

        /// <summary>
        /// Sets cursor to the default position (1,1)
        /// </summary>
        public const string SET_CURSOR_DEFAULT = ESCAPE_CODE + "[H";

        /// <summary>
        /// Uses special rendering mode (used in iTerm2, Kitty)
        /// </summary>
        public const string USE_SPECIAL_RENDERING_MODE = ESCAPE_CODE + "[?2026h";

        /// <summary>
        /// Stops using special rendering mode (used in iTerm2, Kitty)
        /// </summary>
        public const string END_SPECIAL_RENDERING_MODE = ESCAPE_CODE + "[?2026l";

        /// <summary>
        /// Clears the screen from cursor position to the end of the terminal
        /// </summary>
        public const string CLEAR_FROM_CURSOR_TO_END = ESCAPE_CODE + "[0J";
    }
}
