using System;
using System.Drawing;
using System.Threading;
using System.Collections.Generic;

namespace Snake
{
    class GameModel
    {
        public GameMap Map { get; }
        public GameSnake Snake { get; }
        public int Score { get; private set; }
        public bool GameOver { get; private set; }

        public GameModel(int levelNumber = 1)
        {
            Map = GameLevels.GetMap(levelNumber);
            Snake = new GameSnake(GameLevels.InitialSnakePosition);
            DrawItem(Cell.Food);
        }

        public void HandleGame(Queue<GameAction> pressedKeys)
        {
            ClearIncorrectActions(pressedKeys);

            if (Snake.TimelessTurns > 0 && pressedKeys.Count == 0)
                return;
            var nextAction = pressedKeys.Count > 0
                             ? pressedKeys.Dequeue()
                             : Snake.CurrentAction;
            if (GameActions.OppositeActions.ContainsKey(nextAction))
                HandlePointFoward(GetPointFoward(nextAction));
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

        private Point GetPointFoward(GameAction action)
        {
            Snake.CurrentAction = action;
            var pointChange = GameActions.MarchingActionPoint[action];
            var pointFoward = new Point(Snake.Head.X + pointChange.X, Snake.Head.Y + pointChange.Y);
            return HandleBorders(pointFoward);
        }

        private void ClearIncorrectActions(Queue<GameAction> pressedKeys)
        {
            while (pressedKeys.Count > 0)
            {
                if ((pressedKeys.Peek() == Snake.CurrentAction && Snake.TimelessTurns <= 0)
                    || pressedKeys.Peek() == GameActions.OppositeActions[Snake.CurrentAction])
                    pressedKeys.Dequeue();
                else if (pressedKeys.Peek() == GameAction.Reverse && Snake.InventoryItems[Cell.ReverseItem] <= 0)
                    pressedKeys.Dequeue();
                else if (pressedKeys.Peek() == GameAction.StopTime && Snake.InventoryItems[Cell.MagicClock] <= 0)
                    pressedKeys.Dequeue();
                else break;
            }
        }

        private void HandleExtraKeys(GameAction currentAction)
        {
            if (currentAction == GameAction.Reverse)
                ReverseSnake();
            if (currentAction == GameAction.StopTime)
                Snake.ActivateMagicClock();
        }

        private void HandlePointFoward(Point pointFoward)
        {
            if (Map[pointFoward] == Cell.Wall)
                GameOver = !Snake.CheckDefence();
            if (Map[pointFoward] == Cell.Snake)
                GameOver = !Snake.CheckSharpTeeth(pointFoward);
            if (Snake.InventoryItems.ContainsKey(Map[pointFoward]))
                Snake.InventoryItems[Map[pointFoward]]++;

            if (!GameOver)
                ChangeMap(pointFoward, Map[pointFoward] == Cell.Food);
        }

        private Point HandleBorders(Point pointFoward)
        {
            if (pointFoward.X < 0)
                return new Point(Map.Width - 1, pointFoward.Y);
            if (pointFoward.X >= Map.Width)
                return new Point(0, pointFoward.Y);
            if (pointFoward.Y < 0)
                return new Point(pointFoward.X, Map.Height - 1);
            if (pointFoward.Y >= Map.Height)
                return new Point(pointFoward.X, 0);
            return pointFoward;
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
}
