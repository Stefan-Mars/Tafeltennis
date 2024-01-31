using System;
using Xamarin.Forms;

namespace Tafeltennis
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            StatsPage page1 = new StatsPage();
            StartPage page2 = new StartPage();
            ChartPage page3 = new ChartPage();

            var navigationPage = new NavigationPage(page2)
            {
                BarBackgroundColor = Color.FromHex("#7c3aed"),
                BarTextColor = Color.White
            };

            MainPage = navigationPage;


            ToolbarItem toolbarItem2 = new ToolbarItem
            {
                Text = "Stats" 
            };

            toolbarItem2.Command = new Command(async () =>
            {
                if (navigationPage.CurrentPage == page1)
                {
                    await navigationPage.PushAsync(page3);
                }
                else
                {
                    await navigationPage.PushAsync(page1);
                }
                toolbarItem2.Text = (navigationPage.CurrentPage == page1) ? "Chart" : "Stats";
            });

    
            navigationPage.ToolbarItems.Add(toolbarItem2);

            navigationPage.PropertyChanged += (sender, args) =>
            {
                toolbarItem2.Text = (navigationPage.CurrentPage == page1) ? "Chart" : "Stats";
            };
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
