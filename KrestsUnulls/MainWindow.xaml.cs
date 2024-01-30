using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
namespace кресты_и_нули
{
    public partial class MainWindow : Window
    {
        private bool isPlayerX = true;
        private bool isDraw = false;
        private string[,] board = new string[3, 3];

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ResetGame();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            int row = Grid.GetRow(button);
            int column = Grid.GetColumn(button);

            if (!string.IsNullOrEmpty(board[row, column]))
                return;

            board[row, column] = isPlayerX ? "X" : "O";
            button.Content = board[row, column];

            if (CheckForWinner() || CheckForDraw())
            {
                ResetGame();
            }

            isPlayerX = !isPlayerX;

            if (!isPlayerX)
            {
                MakeComputerMove();
            }
        }

        private void MakeComputerMove()
        {
            Random random = new Random();
            int row, column;

            do
            {
                row = random.Next(0, 3);
                column = random.Next(0, 3);
            } while (!string.IsNullOrEmpty(board[row, column]));

            board[row, column] = "O";
            Button computerButton = (Button)FindName($"button{row}{column}");
            computerButton.Content = "O";

            if (CheckForWinner() || CheckForDraw())
            {
                ResetGame();
            }

            isPlayerX = !isPlayerX;
        }

        private void ResetGame()
        {
            isPlayerX = true;
            isDraw = false;
            board = new string[3, 3];

            foreach (var button in ((Grid)Content).Children)
            {
                if (button is Button)
                {
                    ((Button)button).Content = "";
                    ((Button)button).Foreground = Brushes.Black;
                }
            }
        }

        private bool CheckForWinner()
        {
            if (IsWinner("X"))
            {
                UpdateButtonColors("X", Brushes.Green);
                ShowGameResult("Хозяин крестиков ноликов");
                return true;
            }
            else if (IsWinner("O"))
            {
                UpdateButtonColors("O", Brushes.Red);
                ShowGameResult("Next time");
                return true;
            }

            return false;
        }

        private bool CheckForDraw()
        {
            isDraw = true;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (string.IsNullOrEmpty(board[i, j]))
                    {
                        isDraw = false;
                        break;
                    }
                }
                if (!isDraw)
                    break;
            }

            if (isDraw)
            {
                UpdateButtonColors("X", Brushes.Yellow);
                UpdateButtonColors("O", Brushes.Yellow);
                ShowGameResult("Потнейшая борьба");
                return true;
            }

            return false;
        }

        private bool IsWinner(string player)
        {
            for (int i = 0; i < 3; i++)
            {
                if (board[i, 0] == player && board[i, 1] == player && board[i, 2] == player)
                    return true;

                if (board[0, i] == player && board[1, i] == player && board[2, i] == player)
                    return true;
            }

            if (board[0, 0] == player && board[1, 1] == player && board[2, 2] == player)
                return true;
            if (board[0, 2] == player && board[1, 1] == player && board[2, 0] == player)
                return true;

            return false;
        }

        private void ShowGameResult(string result)
        {
            MessageBox.Show(result, "Игра окончена", MessageBoxButton.OK, MessageBoxImage.Information);
            ResetGame();
        }

        private void UpdateButtonColors(string player, Brush color)
        {
            foreach (var button in ((Grid)Content).Children)
            {
                if (button is Button && ((Button)button).Content.ToString() == player)
                {
                    ((Button)button).Foreground = color;
                }
            }
        }
    }
}