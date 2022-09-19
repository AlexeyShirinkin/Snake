namespace Snake;

public enum GameAction
{
    Up,
    Down,
    Right,
    Left,
    Reverse,
    StopTime
}

internal static class GameActions
{
    public static readonly Dictionary<GameAction, GameAction> OppositeActions = new()
    {
        [GameAction.Up] = GameAction.Down,
        [GameAction.Down] = GameAction.Up,
        [GameAction.Left] = GameAction.Right,
        [GameAction.Right] = GameAction.Left
    };

    public static readonly Dictionary<Keys, GameAction> MarchingKeyAction = new()
    {
        [Keys.W] = GameAction.Up,
        [Keys.S] = GameAction.Down,
        [Keys.D] = GameAction.Right,
        [Keys.A] = GameAction.Left,
        [Keys.R] = GameAction.Reverse,
        [Keys.Q] = GameAction.StopTime
    };

    public static readonly Dictionary<GameAction, Point> MarchingActionPoint = new()
    {
        [GameAction.Up] = new Point(0, -1),
        [GameAction.Down] = new Point(0, 1),
        [GameAction.Left] = new Point(-1, 0),
        [GameAction.Right] = new Point(1, 0)
    };
}