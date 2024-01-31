using Android.Content.Res;
using Android.Media;
using System;
using Tafeltennis.Droid;
using Tafeltennis.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(SoundPlayerService))]

namespace Tafeltennis.Droid
{
    public class SoundPlayerService : ISoundPlayerService
    {
        public void Play(string soundFileName)
        {
            try
            {
                var assetManager = Android.App.Application.Context.Assets;
                using (var assetStream = assetManager.OpenFd(soundFileName))
                {
                    var player = new MediaPlayer();
                    player.SetDataSource(assetStream.FileDescriptor, assetStream.StartOffset, assetStream.Length);
                    player.Prepare();
                    player.Start();
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error playing sound: {ex}");
            }
        }
    }
}
