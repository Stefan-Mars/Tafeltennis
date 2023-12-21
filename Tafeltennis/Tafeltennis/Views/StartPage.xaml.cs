using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Tafeltennis.Services;

namespace Tafeltennis
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartPage : ContentPage
    {
        private int selectedPlayerId;
        private int selectedPlayerId2;
        public StartPage()
        {
            InitializeComponent();
            LoadPlayers();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadPlayers(); 
        }
        private async void SubmitButton_Clicked(object sender, EventArgs e)
        {
            if (selectedPlayerId != 0 && selectedPlayerId2 != 0)
            {
                await Navigation.PushAsync(new GamePage(selectedPlayerId, selectedPlayerId2));
            }
            else
            {
                await DisplayAlert("Error", "Player cannot be null", "OK");
            }
        }
        private async void LoadPlayers()
        {
            try
            {
                
                var playerService = new PlayerService();
                List<Player> players = await playerService.GetAllPlayersAsync();

               

                List<string> playerNames = new List<string>();
                Grid grid = new Grid();
                grid.Children.Clear();
                grid.RowDefinitions.Clear();
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                Dictionary<string, int> playerDictionary = new Dictionary<string, int>();

                for (int i = 0; i < players.Count; i++)
                {
                    playerNames.Add(players[i].Name);
                    playerDictionary.Add(players[i].Name, int.Parse(players[i].Id)); 
                }

                
                Picker picker = new Picker
                {
                    Title = "Kies een speler",
                    BackgroundColor = Color.FromHex("#2563eb"),
                    TextColor = Color.White,
                    HorizontalTextAlignment = TextAlignment.Center,
                    FontSize = 40,
                };


                Picker picker2 = new Picker
                {
                    Title = "Kies een speler",
                    BackgroundColor = Color.FromHex("#dc2626"),
                    TextColor = Color.White,
                    HorizontalTextAlignment = TextAlignment.Center,
                    FontSize = 40,
                };
                picker.Items.Clear();
                picker2.Items.Clear();
                foreach (var playerName in playerNames)
                {
                    picker.Items.Add(playerName);
                    picker2.Items.Add(playerName);
                }


                Button button = new Button
                {
                    BackgroundColor = Color.White,
                    Text = "Confirm",
                };
                button.Clicked += SubmitButton_Clicked;

                picker.SelectedIndexChanged += (sender, args) =>
                {
                    if (picker.SelectedIndex != -1)
                    {
                        string selectedPlayerName = playerNames[picker.SelectedIndex];
                        selectedPlayerId = playerDictionary[selectedPlayerName];
                    }
                };

                picker2.SelectedIndexChanged += (sender, args) =>
                {
                    if (picker2.SelectedIndex != -1)
                    {
                        string selectedPlayerName = playerNames[picker2.SelectedIndex];
                        selectedPlayerId2 = playerDictionary[selectedPlayerName];
                    }
                };

                Grid.SetRow(picker, 0);
                Grid.SetRow(button, 1);
                Grid.SetRow(picker2, 2);

                grid.Children.Add(picker);
                grid.Children.Add(button);
                grid.Children.Add(picker2);

                Content = grid;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
        
    }
}