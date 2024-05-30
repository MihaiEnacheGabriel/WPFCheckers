using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using MVPTema2EnacheMihai.Commands;
using MVPTema2EnacheMihai.Models;
using MVPTema2EnacheMihai.Views;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace MVPTema2EnacheMihai.ViewModels
{
    public class MainMenuViewModel
    {
        private CheckerBoardViewModel _checkerBoardViewModel;

        public ICommand NewGameCommand { get; }
        public ICommand LoadGameCommand { get; }
        public ICommand ShowAboutInfoCommand { get; }
        public ICommand StatisticsViewCommand { get; }

        public MainMenuViewModel()
        {
            _checkerBoardViewModel = new CheckerBoardViewModel();
            NewGameCommand = new NewGameCommand(NewGame);
            LoadGameCommand = new LoadGameCommand(LoadGame);
            ShowAboutInfoCommand = new RelayCommand(ShowAboutInfo);
            StatisticsViewCommand = new RelayCommand(OpenStatisticsView);
        }

        private void NewGame()
        {
            var gameLogic = new GameLogic();
            var boardView = new Board();
            boardView.DataContext = new CheckerBoardViewModel();
            boardView.Closed += (sender, args) =>
            {
                // Close the MainMenu view after the Board window is closed
                var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
                window.Close();
            };
            boardView.Show();
        }

        private void LoadGame()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                if (_checkerBoardViewModel != null && File.Exists(filePath))
                {
                    string gameState = File.ReadAllText(filePath);
                    GameLogic loadedGameLogic = GameLogic.LoadState(gameState);
                    _checkerBoardViewModel.UpdateGameLogic(loadedGameLogic);
                }
                else
                {
                    Console.WriteLine($"File not found or CheckerBoardViewModel not initialized: {filePath}");
                }
            }
        }
        private void OpenStatisticsView()
        {
            // Create an instance of StatisticsViewModel
            var statisticsViewModel = new StatisticsViewModel();

            // Create an instance of StatisticsView
            var statisticsView = new StatisticsView();

            // Set the DataContext of StatisticsView to the StatisticsViewModel instance
            statisticsView.DataContext = statisticsViewModel;

            // Show the StatisticsView window
            statisticsView.Show();
        }

        private void ShowAboutInfo()
        {
            var aboutWindow = new AboutWindow();
            aboutWindow.ShowDialog();
        }
    }

}
