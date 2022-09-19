using Timer = System.Windows.Forms.Timer;

namespace Snake
{
    internal class GameForm : Form
    {
        private Timer timer;
        private GameModel game;
        private Button buttonMenu;
        private Button buttonExit;
        private Queue<GameAction> pressedKeys;

        public GameForm()
        {
            Text = "Snake";
            MaximizeBox = false;
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            StartPosition = FormStartPosition.CenterScreen;
            CreateMenu();
        }

        private void CreateMenu()
        {
            var nameGame = CreateLabel(78, 20, "Snake");
            Controls.Add(nameGame);

            var menuButtons = new Button[GameLevels.CountLevels];
            for (var i = 0; i < menuButtons.Length; ++i)
            {
                menuButtons[i] = CreateButton(70, 50 * (2 + i), 150, i + 1 + " Level", Color.FromArgb(35, 204, 27));
                Controls.Add(menuButtons[i]);
                var j = i;
                menuButtons[i].Click += (sender, args) => CreateGame(nameGame, menuButtons, j);
            }
            BackColor = Color.FromArgb(200, 255, 179);
            Size = new Size(300, (menuButtons.Length + 3) * 50);
        }

        private void DeleteMenu(Label nameGame, Button[] menuButtons)
        {
            Controls.Remove(nameGame);
            foreach (var button in menuButtons)
                Controls.Remove(button);
        }

        private Label CreateLabel(int locationX, int locationY, string text)
        {
            return new Label
            {
                Location = new Point(locationX, locationY),
                Size = new Size(220, 50),
                Text = text,
                Font = new Font("Bauhaus 93", 34),
                ForeColor = Color.Green
            };
        }

        private Button CreateButton(int locationX, int locationY, int sizeWidth, string text, Color color)
        {
            return new Button
            {
                Location = new Point(locationX, locationY),
                Size = new Size(sizeWidth, 30),
                Text = text,
                Font = new Font("Bauhaus 93", 20),
                ForeColor = color
            };
        }

        private void CreateGame(Label nameGame, Button[] menuButtons, int levelNumber)
        {
            DeleteMenu(nameGame, menuButtons);

            BackColor = Color.White;
            game = new GameModel(levelNumber);
            pressedKeys = new Queue<GameAction>();
            Size = new Size(game.Map.WidthPixels + 15, game.Map.HeightPixels + 160);

            buttonMenu = GetGameButton(0, "Menu", Color.Green, (sender, args) => DeleteGame());
            buttonExit = GetGameButton(30, "Exit", Color.DarkRed, (sender, args) => Application.Exit());

            KeyUp += KeyPressed;
            timer = new Timer();
            timer.Interval = game.Snake.Speed;
            timer.Tick += HandleGame;
            timer.Start();
        }

        private Button GetGameButton(int deltaY, string text, Color color, EventHandler action)
        {
            var button = CreateButton(game.Map.WidthPixels - 100, game.Map.HeightPixels + deltaY, 100, text, color);
            button.Click += action;
            Controls.Add(button);
            button.Hide();
            return button;
        }

        private void DeleteGame()
        {
            game = null;
            pressedKeys = null;
            timer = null;
            KeyUp -= KeyPressed;
            Controls.Remove(buttonMenu);
            Controls.Remove(buttonExit);
            CreateMenu();
        }

        private void HandleGame(object sender, EventArgs e)
        {
            game.HandleGame(pressedKeys);
            if (game.GameOver)
            {
                timer.Stop();
                buttonMenu.Show();
                buttonExit.Show();
            }
            Invalidate();
        }

        private void KeyPressed(object sender, KeyEventArgs e)
        {
            if (GameActions.MarchingKeyAction.ContainsKey(e.KeyCode))
                pressedKeys.Enqueue(GameActions.MarchingKeyAction[e.KeyCode]);
        }

        private Image GetImage(int x, int y)
        {
            return game.Snake.Head == new Point(x, y)
                ? GameImages.headImages[game.Snake.CurrentAction]
                : GameImages.mapImages[game.Map[x, y]];
        }

        private void DrawMap(Graphics graphics)
        {
            for (var i = 0; i < game.Map.Width; ++i)
                for (var j = 0; j < game.Map.Height; ++j)
                {
                    var location = new Point(i * game.Map.SizeCell, j * game.Map.SizeCell);
                    graphics.DrawImage(GetImage(i, j), location);
                }
        }

        private void DrawInventory(Graphics graphics)
        {
            for (var i = 0; i < game.Snake.InventoryItems.Count; ++i)
            {
                var itemLocation = new Point(game.Map.SizeCell * (i / 2) * 3,
                                              game.Map.HeightPixels + game.Map.SizeCell * (i % 2 == 0 ? 2 : 3));
                graphics.DrawImage(GameImages.invetoryImages[(Cell)(i + 4)], itemLocation);
                graphics.DrawString(game.Snake.InventoryItems[(Cell)(i + 4)].ToString(),
                                    new Font("Bauhaus 93", 18), Brushes.Black,
                                    new Point(game.Map.SizeCell + itemLocation.X, itemLocation.Y));
            }
        }

        private void PrintPartInventory(Graphics graphics, string text, int deltaY)
        {
            graphics.DrawString(text, new Font("Bauhaus 93", 18), Brushes.Black,
                                new Point(0, game.Map.HeightPixels + deltaY));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (game == null)
                return;
            DrawMap(e.Graphics);
            DrawInventory(e.Graphics);
            PrintPartInventory(e.Graphics, "SCORE: " + game.Score, 0);
            PrintPartInventory(e.Graphics, "TIMELESS TURNS: " + game.Snake.TimelessTurns, 30);
            if (game.GameOver)
            {
                e.Graphics.DrawString("YOU DIED", new Font("Bauhaus 93", 34), Brushes.DarkRed,
                                new Point((game.Map.WidthPixels - 205) / 2, (game.Map.HeightPixels - 50) / 2));
            }
        }
    }
}