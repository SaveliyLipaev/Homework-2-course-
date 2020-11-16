module MakeGameToLearnOOP.ILogger

/// Interface for loggers used in game to log game state
/// Returns current step
type ILogger =
    abstract LogState: bool array -> int