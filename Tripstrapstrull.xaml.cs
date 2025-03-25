namespace MauiApp1;

public partial class Tripstrapstrull : ContentPage
{
    private Grid grid;
    private Button newGameButton;
    private Button randomPlayerButton;
    private Button toggleBotButton;
    private Label statusLabel;
    private string currentPlayer = "X";
    private string[,] gameBoard = new string[3, 3];
    private bool gameOver = false;
    private bool isBotEnabled = true;
    private Random random = new Random();

    public Tripstrapstrull()
    {
        // Taustavärv
        BackgroundColor = Color.FromArgb("#1A1A2E");

        // Peamine konteiner
        var mainContainer = new StackLayout
        {
            Padding = new Thickness(20),
            Spacing = 20
        };

        // Mängulaua loomine
        grid = new Grid
        {
            BackgroundColor = Color.FromArgb("#16213E"),
            ColumnSpacing = 8,
            RowSpacing = 8,
            Padding = new Thickness(10),
            VerticalOptions = LayoutOptions.CenterAndExpand,
            HeightRequest = 350 // Fikseeritud kõrgus, et kõik nupud nähtavale jääks
        };

        for (int i = 0; i < 3; i++)
        {
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        }

        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                var cellButton = new Button
                {
                    FontSize = 60,
                    TextColor = Color.FromArgb("#E94560"),
                    BackgroundColor = Color.FromArgb("#0F3460"),
                    CornerRadius = 10,
                    BorderColor = Color.FromArgb("#E94560"),
                    BorderWidth = 2,
                    HeightRequest = 100 // Fikseeritud kõrgus lahtritele
                };
                cellButton.Clicked += CellButton_Clicked;
                grid.Children.Add(cellButton);

                Grid.SetRow(cellButton, row);
                Grid.SetColumn(cellButton, col);

                gameBoard[row, col] = string.Empty;
            }
        }

        // Juhtelementide loomine
        newGameButton = new Button
        {
            Text = "Uus mäng",
            BackgroundColor = Color.FromArgb("#E94560"),
            TextColor = Color.FromArgb("#FFFFFF"),
            FontSize = 16,
            CornerRadius = 20,
            HeightRequest = 50,
            WidthRequest = 150, // Fikseeritud laius
            Margin = new Thickness(5)
        };
        newGameButton.Clicked += NewGameButton_Clicked;

        toggleBotButton = new Button
        {
            Text = isBotEnabled ? "Lülita bot välja" : "Lülita bot sisse",
            BackgroundColor = Color.FromArgb("#533483"),
            TextColor = Color.FromArgb("#FFFFFF"),
            FontSize = 16,
            CornerRadius = 20,
            HeightRequest = 50,
            WidthRequest = 150, // Fikseeritud laius
            Margin = new Thickness(5)
        };
        toggleBotButton.Clicked += ToggleBotButton_Clicked;

        statusLabel = new Label
        {
            Text = "Mängija X kord",
            FontSize = 20,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            TextColor = Color.FromArgb("#E94560"),
            FontAttributes = FontAttributes.Bold
        };

        // Nuppude paigutus
        var buttonLayout = new StackLayout
        {
            Orientation = StackOrientation.Horizontal,
            HorizontalOptions = LayoutOptions.Center,
            Spacing = 10,
            Children = { newGameButton, randomPlayerButton, toggleBotButton }
        };

        // Veel üks konteiner nuppude jaoks, et paremini ekraanile mahuks
        var buttonsContainer = new ScrollView
        {
            Orientation = ScrollOrientation.Horizontal,
            Content = buttonLayout,
            HorizontalScrollBarVisibility = ScrollBarVisibility.Never
        };

        // Liidese kokkupanek
        mainContainer.Children.Add(new Label
        {
            Text = "Tripistripull",
            FontSize = 28,
            TextColor = Color.FromArgb("#E94560"),
            HorizontalOptions = LayoutOptions.Center,
            FontAttributes = FontAttributes.Bold
        });
        mainContainer.Children.Add(statusLabel);
        mainContainer.Children.Add(grid);
        mainContainer.Children.Add(buttonsContainer);

        Content = new ScrollView // Peamine keritav konteiner
        {
            Content = mainContainer
        };
    }

    private void ToggleBotButton_Clicked(object sender, EventArgs e)
    {
        isBotEnabled = !isBotEnabled;
        toggleBotButton.Text = isBotEnabled ? "Lülita bot välja" : "Lülita bot sisse";
    }

    private void CellButton_Clicked(object sender, EventArgs e)
    {
        if (gameOver) return;

        var button = sender as Button;
        var position = GetButtonPosition(button);
        int row = position.Item1;
        int col = position.Item2;

        if (gameBoard[row, col] == string.Empty)
        {
            button.Text = currentPlayer;
            button.TextColor = currentPlayer == "X" ? Color.FromArgb("#E94560") : Color.FromArgb("#00B4D8");
            gameBoard[row, col] = currentPlayer;

            if (CheckWinner())
            {
                statusLabel.Text = $"{currentPlayer} võitis!";
                gameOver = true;
                DisplayAlert("Mäng läbi", $"{currentPlayer} võitis! Kas soovid uuesti mängida?", "Jah", "Ei");
                return;
            }
            else if (IsBoardFull())
            {
                statusLabel.Text = "Viik!";
                gameOver = true;
                DisplayAlert("Mäng läbi", "Viik! Kas soovid uuesti mängida?", "Jah", "Ei");
                return;
            }

            currentPlayer = (currentPlayer == "X") ? "O" : "X";
            statusLabel.Text = $"Mängija {currentPlayer} kord";

            if (isBotEnabled && currentPlayer == "O" && !gameOver)
            {
                MakeBotMove();
            }
        }
    }

    private void NewGameButton_Clicked(object sender, EventArgs e)
    {
        StartNewGame();
    }

    private void StartNewGame()
    {
        gameOver = false;
        currentPlayer = "X";
        statusLabel.Text = "Mängija X kord";

        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                gameBoard[row, col] = string.Empty;

                foreach (var child in grid.Children)
                {
                    if (child is Button button && Grid.GetRow(button) == row && Grid.GetColumn(button) == col)
                    {
                        button.Text = string.Empty;
                        break;
                    }
                }
            }
        }
    }

    private void RandomPlayerButton_Clicked(object sender, EventArgs e)
    {
        currentPlayer = random.Next(0, 2) == 0 ? "X" : "O";
        statusLabel.Text = $"Mängija {currentPlayer} kord";

        if (isBotEnabled && currentPlayer == "O")
        {
            MakeBotMove();
        }
    }

    private void MakeBotMove()
    {
        if (gameOver || !isBotEnabled) return;

        List<Tuple<int, int>> emptyCells = new List<Tuple<int, int>>();

        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                if (gameBoard[row, col] == string.Empty)
                {
                    emptyCells.Add(new Tuple<int, int>(row, col));
                }
            }
        }

        if (emptyCells.Count == 0) return;

        var move = emptyCells[random.Next(emptyCells.Count)];
        int botRow = move.Item1;
        int botCol = move.Item2;

        foreach (var child in grid.Children)
        {
            if (child is Button button && Grid.GetRow(button) == botRow && Grid.GetColumn(button) == botCol)
            {
                button.Text = "O";
                button.TextColor = Color.FromArgb("#00B4D8");
                gameBoard[botRow, botCol] = "O";
                break;
            }
        }

        if (CheckWinner())
        {
            statusLabel.Text = "O võitis!";
            gameOver = true;
            DisplayAlert("Mäng läbi", "O võitis! Kas soovid uuesti mängida?", "Jah", "Ei");
            return;
        }
        else if (IsBoardFull())
        {
            statusLabel.Text = "Viik!";
            gameOver = true;
            DisplayAlert("Mäng läbi", "Viik! Kas soovid uuesti mängida?", "Jah", "Ei");
            return;
        }

        currentPlayer = "X";
        statusLabel.Text = "Mängija X kord";
    }

    private Tuple<int, int> GetButtonPosition(Button button)
    {
        int row = Grid.GetRow(button);
        int col = Grid.GetColumn(button);
        return Tuple.Create(row, col);
    }

    private bool CheckWinner()
    {
        for (int i = 0; i < 3; i++)
        {
            if (gameBoard[i, 0] == currentPlayer && gameBoard[i, 1] == currentPlayer && gameBoard[i, 2] == currentPlayer)
                return true;
            if (gameBoard[0, i] == currentPlayer && gameBoard[1, i] == currentPlayer && gameBoard[2, i] == currentPlayer)
                return true;
        }
        if (gameBoard[0, 0] == currentPlayer && gameBoard[1, 1] == currentPlayer && gameBoard[2, 2] == currentPlayer)
            return true;
        if (gameBoard[0, 2] == currentPlayer && gameBoard[1, 1] == currentPlayer && gameBoard[2, 0] == currentPlayer)
            return true;

        return false;
    }

    private bool IsBoardFull()
    {
        foreach (var cell in gameBoard)
        {
            if (string.IsNullOrEmpty(cell))
                return false;
        }
        return true;
    }
}