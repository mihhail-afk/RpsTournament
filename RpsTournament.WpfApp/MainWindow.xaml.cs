using System;
using System.Collections.Generic;
using System.Resources;
using System.Windows;
using RpsTournament.Core;

namespace RpsTournament.WpfApp
{
    public partial class MainWindow : Window
    {
        private readonly List<GameRound> _rounds = new List<GameRound>();
        private ResourceManager? _resourceManager;
        private int _playerScore = 0;
        private int _computerScore = 0;
        private int _currentRound = 1;

        public MainWindow()
        {
            InitializeComponent();

            try
            {
                _resourceManager = new ResourceManager("RpsTournament.WpfApp.Resources", typeof(MainWindow).Assembly);
            }
            catch
            {
                _resourceManager = null;
            }

            MoveComboBox.ItemsSource = new List<Move>
            {
                Move.Rock,
                Move.Paper,
                Move.Scissors
            };
        }

        private void PlayRoundButton_Click(object sender, RoutedEventArgs e)
        {
            string playerName = PlayerNameTextBox.Text;

            // Nime kontroll
            if (!GameLogic.ValidatePlayerName(playerName))
            {
                string errorMsg = GetString("InvalidNameError", "Mängija nimi peab olema 2 kuni 30 märki pikk!");
                MessageBox.Show(errorMsg, "Viga", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Käigu kontroll
            if (MoveComboBox.SelectedItem == null || (Move)MoveComboBox.SelectedItem == Move.None)
            {
                string errorMsg = GetString("NoMoveSelectedError", "Palun vali käik enne käigu tegemist!");
                MessageBox.Show(errorMsg, "Viga", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Move playerMove = (Move)MoveComboBox.SelectedItem;
            Move computerMove = GameLogic.GetRandomComputerMove();
            RoundResult result = GameLogic.EvaluateRound(playerMove, computerMove);

            string resultText = "";
            if (result == RoundResult.PlayerWin)
            {
                _playerScore++;
                resultText = "Võit";
            }
            else if (result == RoundResult.ComputerWin)
            {
                _computerScore++;
                resultText = "Kaotus";
            }
            else
            {
                resultText = "Viik";
            }

            GameRound round = new GameRound
            {
                RoundNumber = _currentRound,
                PlayerName = playerName,
                PlayerMove = playerMove,
                ComputerMove = computerMove,
                Result = result,
                ResultText = resultText
            };

            _rounds.Add(round);
            RoundsDataGrid.ItemsSource = null;
            RoundsDataGrid.ItemsSource = _rounds;

            ScoreTextBlock.Text = $"Skoor: {playerName} {_playerScore} - {_computerScore} Arvuti";

            if (_currentRound == 5)
            {
                PlayRoundButton.IsEnabled = false;
                string winnerText = _playerScore > _computerScore
                    ? $"Turniiri võitis {playerName}!"
                    : (_computerScore > _playerScore ? "Turniiri võitis Arvuti!" : "Turniir lõppes viigiga!");

                MessageBox.Show($"Turniir on lõppenud!\n\n{winnerText}", "Lõpptulemus", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                _currentRound++;
            }
        }

        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            _rounds.Clear();
            _playerScore = 0;
            _computerScore = 0;
            _currentRound = 1;

            RoundsDataGrid.ItemsSource = null;
            ScoreTextBlock.Text = "Skoor: Mängija 0 - 0 Arvuti";
            PlayRoundButton.IsEnabled = true;
            MoveComboBox.SelectedIndex = -1;
        }

        private string GetString(string key, string fallback)
        {
            if (_resourceManager == null) return fallback;
            try
            {
                string? val = _resourceManager.GetString(key);
                return !string.IsNullOrEmpty(val) ? val : fallback;
            }
            catch
            {
                return fallback;
            }
        }
    }
}