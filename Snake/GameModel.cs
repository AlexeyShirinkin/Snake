namespace Snake;

internal class GameModel
{
    public GameModel(int levelNumber = 1)
    {
        Map = GameLevels.GetMap(levelNumber);
        Snake = new GameSnake(GameLevels.InitialSnakePosition);
        DrawItem(Cell.Food);
    }

    public GameMap Map { get; }
    public GameSnake Snake { get; }
    public int Score { get; private set; }
    public bool GameOver { get; private set; }

    public void HandleGame(Queue<GameAction> pressedKeys)
    {
        ClearIncorrectActions(pressedKeys);

        if (Snake.TimelessTurns > 0 && pressedKeys.Count == 0)
            return;
        var nextAction = pressedKeys.Count > 0
            ? pressedKeys.Dequeue()
            : Snake.CurrentAction;
        if (GameActions.OppositeActions.ContainsKey(nextAction))
            HandlePointForward(GetPointForward(nextAction));
        else HandleExtraKeys(nextAction);
    }

    private void DrawSnake()
    {
        Map.ClearCells(Cell.Snake);
        for (var i = 0; i < Snake.Length; ++i)
            Map[Snake[i].X, Snake[i].Y] = Cell.Snake;
    }

    private void DrawItem(Cell cell)
    {
        var emptyPoints = Map.GetEmptyPoints();
        if (emptyPoints.Count == 0)
            return;
        var randomIndex = new Random().Next(0, emptyPoints.Count - 1);
        var item = emptyPoints[randomIndex];
        Map[item.X, item.Y] = cell;
    }

    private void MoveSnake(Point toPoint)
    {
        Snake.Move(toPoint);
        DrawSnake();
    }

    private void IncreaseSnake(Point toPoint)
    {
        Snake.IncreaseLength(toPoint);
        DrawSnake();
    }

    private void ReverseSnake()
    {
        Snake.ReverseBody();
        DrawSnake();
    }

    private void ChangeMap(Point toPoint, bool isIncrease)
    {
        var tail = Snake.Tail;
        MoveSnake(toPoint);
        if (isIncrease)
        {
            Score++;
            IncreaseSnake(tail);
            DrawOtherItems();
        }
    }

    private Point GetPointForward(GameAction action)
    {
        Snake.CurrentAction = action;
        var pointChange = GameActions.MarchingActionPoint[action];
        var pointForward = new Point(Snake.Head.X + pointChange.X, Snake.Head.Y + pointChange.Y);
        return HandleBorders(pointForward);
    }

    private void ClearIncorrectActions(Queue<GameAction> pressedKeys)
    {
        while (pressedKeys.Count > 0)
            if ((pressedKeys.Peek() == Snake.CurrentAction && Snake.TimelessTurns <= 0)
                || pressedKeys.Peek() == GameActions.OppositeActions[Snake.CurrentAction])
                pressedKeys.Dequeue();
            else if (pressedKeys.Peek() == GameAction.Reverse && Snake.InventoryItems[Cell.ReverseItem] <= 0)
                pressedKeys.Dequeue();
            else if (pressedKeys.Peek() == GameAction.StopTime && Snake.InventoryItems[Cell.MagicClock] <= 0)
                pressedKeys.Dequeue();
            else break;
    }

    private void HandleExtraKeys(GameAction currentAction)
    {
        if (currentAction == GameAction.Reverse)
            ReverseSnake();
        if (currentAction == GameAction.StopTime)
            Snake.ActivateMagicClock();
    }

    private void HandlePointForward(Point pointForward)
    {
        if (Map[pointForward] == Cell.Wall)
            GameOver = !Snake.CheckDefence();
        if (Map[pointForward] == Cell.Snake)
            GameOver = !Snake.CheckSharpTeeth(pointForward);
        if (Snake.InventoryItems.ContainsKey(Map[pointForward]))
            Snake.InventoryItems[Map[pointForward]]++;

        if (!GameOver)
            ChangeMap(pointForward, Map[pointForward] == Cell.Food);
    }

    private Point HandleBorders(Point pointForward)
    {
        if (pointForward.X < 0)
            return pointForward with { X = Map.Width - 1 };
        if (pointForward.X >= Map.Width)
            return pointForward with { X = 0 };
        if (pointForward.Y < 0)
            return pointForward with { Y = Map.Height - 1 };
        if (pointForward.Y >= Map.Height)
            return pointForward with { Y = 0 };
        return pointForward;
    }

    private void DrawOtherItems()
    {
        DrawItem(Cell.Food);
        if (Score % 5 == 0)
        {
            Thread.Sleep(30);
            var randomItem = new Random().Next((int)Cell.Defence, (int)Cell.ReverseItem + 1);
            DrawItem((Cell)randomItem);
        }
    }
}