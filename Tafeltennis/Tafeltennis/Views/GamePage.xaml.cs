using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tafeltennis.Services;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.IO;



namespace Tafeltennis
{
    
    public partial class GamePage : ContentPage
    {
        int blue = 0;
        int red = 0;

        int blueWinCount = 0;
        int redWinCount = 0;

        private readonly PlayerService playerService = new PlayerService();
        private Player player1;
        private Player player2;

        private readonly int selectedPlayerId;
        private readonly int selectedPlayerId2;
        public GamePage(int selectedPlayerId, int selectedPlayerId2)
        {
            InitializeComponent();
            this.selectedPlayerId = selectedPlayerId;
            this.selectedPlayerId2 = selectedPlayerId2;

            InitializePage();


        }
        private async void InitializePage()
        {

            player1 = await playerService.GetPlayerByIdAsync(selectedPlayerId);
            player2 = await playerService.GetPlayerByIdAsync(selectedPlayerId2);

            UpdateButtonText();
        }

        private async void OnButtonClicked(object sender, EventArgs e)
        {
            ClearButton.IsEnabled = false;
            BlueButton.IsEnabled = false;
            RedButton.IsEnabled = false;
            if (sender != BlueButton)
            {
                if (sender == RedButton)
                {
                    red++;
                    ((Button)sender).Text = $"{player2.Name} ({redWinCount}) {Environment.NewLine} {red}";
                }
            }
            else
            {
                blue++;
                ((Button)sender).Text = $"{player1.Name} ({blueWinCount}) {Environment.NewLine} {blue}";
            }
            if ((blue >= 11 && Math.Abs(blue - red) >= 2) || (red >= 11 && Math.Abs(red - blue) >= 2))
            {
                
                await ShowConfetti();
                
                if (sender != BlueButton)
                {
                    if (sender == RedButton)
                    {
                        redWinCount++;
                        int playerId2 = int.Parse(player2.Id);
                        await UpdatePlayer(playerId2);



                    }
                }
                else
                {
                    blueWinCount++;
                    int playerId1 = int.Parse(player1.Id);
                    await UpdatePlayer(playerId1); ;
                }
                Clear(this, EventArgs.Empty);




            }
            DependencyService.Get<ISoundPlayerService>().Play("boop.mp3");
            ClearButton.IsEnabled = true;
            BlueButton.IsEnabled = true;
            RedButton.IsEnabled = true;


        }
       

        private async Task ShowConfetti()
        {
            DependencyService.Get<ISoundPlayerService>().Play("yay.mp3");
            Random random = new Random();
            int confettiCount = 100; 
            Grid confettiContainer = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                InputTransparent = true 
            };

            ConfettiContainer.Children.Add(confettiContainer);

            List<BoxView> confettiList = new List<BoxView>();

            for (int i = 0; i < confettiCount; i++)
            {
                BoxView confettiLine = new BoxView
                {
                    Color = Color.FromRgb(random.Next(256), random.Next(256), random.Next(256)),

                    HeightRequest = 1,
                    WidthRequest = 1
                };

                int row = random.Next(60);
                int column = random.Next(30);
                confettiContainer.Children.Add(confettiLine, column, row);
                confettiList.Add(confettiLine);
            }

            await Task.WhenAll(confettiList.Select(confettiLine => confettiLine.TranslateTo(random.Next((int)(-confettiContainer.Width), (int)(confettiContainer.Width * 2)), confettiContainer.Height, 2000, Easing.Linear)));

            await Task.WhenAll(confettiList.Select(confettiLine => confettiLine.FadeTo(0, 2000)));
            foreach (var confettiLine in confettiList)
            {
                confettiContainer.Children.Remove(confettiLine);
            }
            ConfettiContainer.Children.Remove(confettiContainer);
        }

        private void Clear(object sender, EventArgs e)
        {
            blue = 0;
            red = 0;
            UpdateButtonText();
        }
        private void UpdateButtonText()
        {
            BlueButton.Text = $"{player1.Name} ({blueWinCount}) {Environment.NewLine} {blue}";
            RedButton.Text = $"{player2.Name} ({redWinCount}) {Environment.NewLine} {red}";
        }
        private async Task UpdatePlayer(int playerId)
        {
            var playerService = new PlayerService();
            Player player = await playerService.GetPlayerByIdAsync(playerId);

            if (int.TryParse(player.Wins, out int currentWins))
            {
                int updatedWins = currentWins + 1;
                await playerService.UpdatePlayerAsync(playerId, updatedWins.ToString());
            }
            else
            {
                throw new Exception("Invalid Wins value for the player");
            }
        }

    }
}
