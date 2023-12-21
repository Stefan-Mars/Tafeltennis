using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace Tafeltennis
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Create instances of your pages
            StatsPage page1 = new StatsPage();
            StartPage page2 = new StartPage();

            // Set up the MainPage with a NavigationPage and initial content
            MainPage = new NavigationPage(page2)
            {
                BarBackgroundColor = Color.FromHex("#7c3aed"),
                BarTextColor = Color.White
            };

            // Create ToolbarItems


            ToolbarItem toolbarItem2 = new ToolbarItem
            {
                Text = "Stats",
                Command = new Command(() => (MainPage as NavigationPage)?.PushAsync(page1))
            };

            // Add ToolbarItems to the NavigationPage;
            (MainPage as NavigationPage)?.ToolbarItems.Add(toolbarItem2);


        }

        protected override void OnStart()
        {
        }


        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
