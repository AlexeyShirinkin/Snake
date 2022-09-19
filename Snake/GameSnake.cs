namespace Snake;

internal class GameSnake
{
    private readonly List<Point> snakeBody;

    public GameSnake(Point head)
    {
        Speed = 350;
        CurrentAction = GameAction.Up;
        snakeBody = new List<Point> { head };
        InventoryItems = new Dictionary<Cell, int>
        {
            [Cell.Defence] = 0,
            [Cell.SharpTeeth] = 0,
            [Cell.MagicClock] = 0,
            [Cell.ReverseItem] = 0
        };
    }

    public Point Head => snakeBody[0];
    public Point Tail => snakeBody[Length - 1];
    public int Length => snakeBody.Count;
    public GameAction CurrentAction { get; set; }
    public Dictionary<Cell, int> InventoryItems { get; }
    public int TimelessTurns { get; private set; }
    public int Speed { get; }

    public Point this[int i] => snakeBody[i];

    public void IncreaseLength(Point point)
    {
        snakeBody.Add(point);
    }

    public bool CheckDefence()
    {
        if (InventoryItems[Cell.Defence] > 0)
        {
            InventoryItems[Cell.Defence]--;
            return true;
        }

        return false;
    }

    public bool CheckSharpTeeth(Point point)
    {
        if (InventoryItems[Cell.SharpTeeth] > 0)
        {
            InventoryItems[Cell.SharpTeeth]--;
            CutLength(point);
            return true;
        }

        return false;
    }

    public void ActivateMagicClock()
    {
        InventoryItems[Cell.MagicClock]--;
        TimelessTurns += 10;
    }

    public void Move(Point head)
    {
        for (var i = Length - 1; i > 0; --i)
            snakeBody[i] = snakeBody[i - 1];
        snakeBody[0] = head;

        if (TimelessTurns > 0)
            TimelessTurns--;
    }

    public void ReverseBody()
    {
        InventoryItems[Cell.ReverseItem]--;
        CurrentAction = GetNewAction();
        snakeBody.Reverse();

        if (TimelessTurns > 0)
            TimelessTurns--;
    }

    private GameAction GetNewAction()
    {
        if (Length <= 2)
            return GameActions.OppositeActions[CurrentAction];
        var deltaX = snakeBody[Length - 2].X - Tail.X;
        var deltaY = snakeBody[Length - 2].Y - Tail.Y;
        if (deltaX != 0)
            return deltaX == 1 || deltaX < -1
                ? GameAction.Left
                : GameAction.Right;
        return deltaY == 1 || deltaY < -1
            ? GameAction.Up
            : GameAction.Down;
    }

    private void CutLength(Point point)
    {
        while (Tail != point)
            snakeBody.RemoveAt(Length - 1);
        snakeBody.RemoveAt(Length - 1);
    }
}