using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Tafeltennis.Services;
using System.Numerics;


namespace Tafeltennis
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StatsPage : ContentPage
    {
        public StatsPage()
        {
            InitializeComponent();
            LoadPlayers();
        }


        private async void LoadPlayers()
        {
            try
            {
                var playerService = new PlayerService();
                List<Player> players = await playerService.GetAllPlayersAsync();


                for (int i = playerTable.Children.Count - 1; i >= 0; i--)
                {
                    if (Grid.GetRow(playerTable.Children[i]) != 0)
                    {
                        playerTable.Children.RemoveAt(i);
                    }
                }

                int startingRow = 1;



                foreach (var player in players)
                {
                    Label nameLabel = new Label { Text = player.Name };
                    Label winsLabel = new Label { Text = player.Wins };
                    Button deleteButton = new Button {
                        ImageSource = ImageSource.FromFile("delete.png"),
                        CommandParameter = player.Id,
                        BackgroundColor = Color.Transparent,
                        HeightRequest = 40,
                        WidthRequest = 30,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center,
                    };
                    deleteButton.Clicked += async (sender, e) => await DeletePlayerButton_Clicked(sender, e);



                    Grid.SetColumn(nameLabel, 0);
                    Grid.SetRow(nameLabel, startingRow);

                    Grid.SetColumn(winsLabel, 1);
                    Grid.SetRow(winsLabel, startingRow);

                    Grid.SetColumn(deleteButton, 2);
                    Grid.SetRow(deleteButton, startingRow);


                    playerTable.Children.Add(nameLabel);
                    playerTable.Children.Add(winsLabel);
                    playerTable.Children.Add(deleteButton);
                    startingRow++;
                }
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
            var newPlayer = new Player() { Id = "", Name = inputValue, Wins = "0", };
            await playerService.CreatePlayerAsync(newPlayer);
            PlayerName.Text = string.Empty;
            LoadPlayers();


        }
        
        private async Task DeletePlayer(int playerId)
        {
            var playerService = new PlayerService();
            await playerService.DeletePlayerAsync(playerId);
            LoadPlayers();
        }



        private async void RefreshButton_Clicked(object sender, EventArgs e)
        {
            await myImage.RotateTo(360, 1000);
            LoadPlayers();
            await myImage.RotateTo(0, 0);
        }
        private async void CreatePlayerButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(PlayerName.Text))
                {
                    await MakePlayer();
                    await ShowFlashMessage("Player created succesfully");
                }
                else
                {
                    await DisplayAlert("Error", "Input field cannot be null", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
                if (ex.InnerException != null)
                {
                    await DisplayAlert("Error", ex.Message + ex.InnerException, "OK");

                }
            }

        }
       
        private async Task DeletePlayerButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (sender is Button button)
                {
                    int playerId = Convert.ToInt32(button.CommandParameter);
                    await DeletePlayer(playerId);
                    await ShowFlashMessage("Player deleted succesfully");



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
        private async Task ShowFlashMessage(string message)
        {
            Label flashLabel = new Label
            {
                Text = message,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                BackgroundColor = Color.LightGray,
                TextColor = Color.Black,
                FontSize = 20
            };
            Layout.Children.Add(flashLabel);
            await Task.Delay(2000);
            Layout.Children.Remove(flashLabel);
        }

    }
    



}