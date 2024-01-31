using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Tafeltennis.Services;
using Tafeltennis.Models;

namespace Tafeltennis
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StatsPage : ContentPage
    {
        private readonly StatsViewModel viewModel;

        public StatsPage()
        {
            InitializeComponent();
            viewModel = new StatsViewModel();
            BindingContext = viewModel;
            InitializePage();
        }
        private async void GoToChartButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ChartPage());
        }


        private async void InitializePage()
        {
            await LoadPlayers();
        }

        private async Task LoadPlayers()
        {
            try
            {
                await viewModel.LoadPlayers();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async Task MakePlayer()
        {
            string inputValue = PlayerName.Text;
            var playerService = new PlayerService();
            var newPlayer = new Player() { Id = "", Name = inputValue, Wins = "0" };
            await playerService.CreatePlayerAsync(newPlayer);
            PlayerName.Text = string.Empty;
            await LoadPlayers();
        }

        private async Task DeletePlayer(int playerId)
        {
            await viewModel.DeletePlayer(playerId);
        }

        private async void RefreshView_Refreshing(object sender, EventArgs e)
        {
            await viewModel.Refresh();
            refreshView.IsRefreshing = false;
        }

        private async void CreatePlayerButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(PlayerName.Text))
                {
                    await MakePlayer();
                }
                else
                {
                    await DisplayAlert("Error", "Input field cannot be null", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void DeletePlayerButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (sender is Button button)
                {
                    int playerId = Convert.ToInt32(button.CommandParameter);
                    await DeletePlayer(playerId);
                }
                else
                {
                    await DisplayAlert("Message", "Player couldn't be deleted", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

       
    }

}
