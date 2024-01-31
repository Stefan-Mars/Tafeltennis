using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Tafeltennis.Services;
using Xamarin.Forms;

namespace Tafeltennis.Models
{
    public class StatsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool isRefreshing;
        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set
            {
                if (isRefreshing != value)
                {
                    isRefreshing = value;
                    OnPropertyChanged(nameof(IsRefreshing));
                }
            }
        }

        private ObservableCollection<Player> players;
        public ObservableCollection<Player> Players
        {
            get { return players; }
            set
            {
                if (players != value)
                {
                    players = value;
                    OnPropertyChanged(nameof(Players));
                }
            }
        }

        private ICommand refreshCommand;
        public ICommand RefreshCommand
        {
            get
            {
                return refreshCommand ?? (refreshCommand = new Command(async () => await Refresh()));
            }
        }

        public StatsViewModel()
        {
            Players = new ObservableCollection<Player>();
            // You may want to load players when the ViewModel is instantiated
            // await LoadPlayers();
        }

        public async Task Refresh()
        {
            IsRefreshing = true;
            await LoadPlayers();
            IsRefreshing = false;
        }
        public async Task DeletePlayer(int playerId)
        {
            var playerService = new PlayerService();
            await playerService.DeletePlayerAsync(playerId);
            await LoadPlayers();
        }
        public async Task LoadPlayers()
        {
            try
            {
                var playerService = new PlayerService();
                List<Player> loadedPlayers = await playerService.GetAllPlayersAsync();

                Players.Clear(); // Clear the existing collection before adding new players

                foreach (var player in loadedPlayers)
                {
                    Players.Add(player);
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
