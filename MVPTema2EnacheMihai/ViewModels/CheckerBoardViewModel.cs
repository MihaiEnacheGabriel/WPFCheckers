using MVPTema2EnacheMihai.Commands;
using MVPTema2EnacheMihai.Models;
using MVPTema2EnacheMihai.Views;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Text;
namespace MVPTema2EnacheMihai.ViewModels
{
    public class CheckerBoardViewModel : INotifyPropertyChanged
    {
        private GameLogic _gameLogic;
        private WinsManager _winsManager;


        public CheckerBoardViewModel()
        {
            _gameLogic = new GameLogic();
            _winsManager = new WinsManager(); // Initialize WinsManager first
            _gameLogic.PropertyChanged += GameLogic_PropertyChanged;
            CellClickedCommand = new RelayCommand<CheckerCell>(_gameLogic.CellClicked);
            SaveGameCommand = new SaveGameCommand(SaveGame);

        }

       /* public CheckerBoardViewModel(GameLogic gameLogic)
        {
            _gameLogic = gameLogic;
            _gameLogic.PropertyChanged += GameLogic_PropertyChanged;
            CellClickedCommand = new RelayCommand<CheckerCell>(_gameLogic.CellClicked);
            SaveGameCommand = new SaveGameCommand(SaveGame);
        }*/

        private void GameLogic_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(GameLogic.PlayerOnePiecesCount) || e.PropertyName == nameof(GameLogic.PlayerTwoPiecesCount))
            {
                // Check for win condition and update winner
                string winner = _gameLogic.CheckForWinCondition();
                if (!string.IsNullOrEmpty(winner))
                {
                    ShowWinnerPopup(winner);
                    // Increment player wins when there's a winner
                    if (winner == "Player One (Red) wins!")
                        _winsManager.IncrementPlayerOneWins();
                    else if (winner == "Player Two (Black) wins!")
                        _winsManager.IncrementPlayerTwoWins();
                }
            }

            OnPropertyChanged(e.PropertyName);
        }

        public ObservableCollection<CheckerCell> Cells => _gameLogic.Cells;
        public bool IsPlayerOneTurn => _gameLogic.IsPlayerOneTurn;
        public int PlayerOnePiecesCount => _gameLogic.PlayerOnePiecesCount;
        public int PlayerTwoPiecesCount => _gameLogic.PlayerTwoPiecesCount;
        public ICommand CellClickedCommand { get; }
        public ICommand SaveGameCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ShowWinnerPopup(string winner)
        {
            var winnerPopup = new WinnerPopupWindow(winner);
            // Subscribe to the RestartRequested event
            ((WinnerPopupViewModel)winnerPopup.DataContext).RestartRequested += (sender, e) => RestartGame();
            winnerPopup.ShowDialog();
        }

        public void RestartGame()
        {
            _gameLogic = new GameLogic();
            OnPropertyChanged(nameof(Cells));
            OnPropertyChanged(nameof(IsPlayerOneTurn));
            OnPropertyChanged(nameof(PlayerOnePiecesCount));
            OnPropertyChanged(nameof(PlayerTwoPiecesCount));
        }



        public void SaveGame(string filePath)
        {
            try
            {
                string gameStateToSave = _gameLogic.SaveState();

                filePath = Path.ChangeExtension(filePath, ".json");

                File.WriteAllText(filePath, gameStateToSave, Encoding.UTF8);

                Console.WriteLine($"Game state saved to file: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while saving the game: {ex.Message}");
            }
        }


        public void UpdateGameLogic(GameLogic gameLogic)
        {
            _gameLogic = gameLogic;
            OnPropertyChanged(nameof(Cells));
            OnPropertyChanged(nameof(IsPlayerOneTurn));
            OnPropertyChanged(nameof(PlayerOnePiecesCount));
            OnPropertyChanged(nameof(PlayerTwoPiecesCount));
        }


    }
}
