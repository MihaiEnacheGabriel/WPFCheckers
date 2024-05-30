using System;
using System.ComponentModel;
using System.IO;
using MVPTema2EnacheMihai.Models;
using Newtonsoft.Json;

namespace MVPTema2EnacheMihai.ViewModels
{
    public class StatisticsViewModel : INotifyPropertyChanged
    {
        private WinsData _winsData;

        public int PlayerOneWins
        {
            get { return _winsData.PlayerOneWins; }
            set
            {
                if (_winsData.PlayerOneWins != value)
                {
                    _winsData.PlayerOneWins = value;
                    OnPropertyChanged();
                }
            }
        }

        public int PlayerTwoWins
        {
            get { return _winsData.PlayerTwoWins; }
            set
            {
                if (_winsData.PlayerTwoWins != value)
                {
                    _winsData.PlayerTwoWins = value;
                    OnPropertyChanged();
                }
            }
        }

        public StatisticsViewModel()
        {
            LoadWins();
        }

        private void LoadWins()
        {
            try
            {
                string json = File.ReadAllText("wins.json");
                _winsData = JsonConvert.DeserializeObject<WinsData>(json);
                OnPropertyChanged(nameof(PlayerOneWins));
                OnPropertyChanged(nameof(PlayerTwoWins));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while loading wins data: {ex.Message}");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
