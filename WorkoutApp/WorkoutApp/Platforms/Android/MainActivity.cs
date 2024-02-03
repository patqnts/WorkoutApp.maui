using Android.App;
using Android.Content.PM;
using Android.OS;

namespace WorkoutApp
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            // Set status bar color
            Window.SetStatusBarColor(Android.Graphics.Color.ParseColor("#23272A"));

            // Set navigation bar color
            Window.SetNavigationBarColor(Android.Graphics.Color.ParseColor("#23272A"));

            base.OnCreate(savedInstanceState);
        }

    }
}
