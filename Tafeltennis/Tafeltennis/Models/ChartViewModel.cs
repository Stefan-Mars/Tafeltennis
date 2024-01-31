using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using OxyPlot;
using OxyPlot.Series;
using Tafeltennis.Services;
using Xamarin.Forms;

namespace Tafeltennis.Models
{
    public class ChartViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Player> players;
        private PlotModel plotModel;

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

        public PlotModel PlotModel
        {
            get { return plotModel; }
            set
            {
                if (plotModel != value)
                {
                    plotModel = value;
                    OnPropertyChanged(nameof(PlotModel));
                }
            }
        }

        public ChartViewModel()
        {
            Players = new ObservableCollection<Player>();
            // You may want to load players when the ViewModel is instantiated
            LoadPlayers();
        }

        public async Task LoadPlayers()
        {
            try
            {
                var playerService = new PlayerService();
                List<Player> loadedPlayers = await playerService.GetAllPlayersAsync();

                Players.Clear();

                foreach (var player in loadedPlayers)
                {
                    Players.Add(player);
                }

                // Filter out players with 0 wins
                var nonZeroWinsPlayers = Players.Where(player => double.TryParse(player.Wins, out double wins) && wins != 0);

                var slices = nonZeroWinsPlayers.Select(player =>
                {
                    double xAxisValue;
                    if (int.TryParse(player.Name, out int xValue))
                    {
                        xAxisValue = xValue;
                    }
                    else
                    {
                        xAxisValue = 0;
                    }

                    double yAxisValue;
                    if (double.TryParse(player.Wins, out double yValue))
                    {
                        yAxisValue = yValue;
                    }
                    else
                    {
                        yAxisValue = 0;
                    }

                    return new PieSlice(player.Name, yAxisValue);
                });

                var model = new PlotModel
                {
                    Title = "Player Wins",
                };

                var pieSeries = new PieSeries
                {
                    Slices = slices.ToList(),
                    InnerDiameter = 0.2,
                };

                model.Series.Add(pieSeries);

                PlotModel = model;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
