using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using WorkoutApp.Dal;
using WorkoutApp.MVVM.View;
using WorkoutApp.MVVM.View.Content;
using WorkoutApp.MVVM.ViewModel;
using Xe.AcrylicView;

namespace WorkoutApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseAcrylicView()
                .UseMauiCommunityToolkit()
                .ConfigureLifecycleEvents(events =>
                {
#if ANDROID
                events.AddAndroid(android => android.OnCreate((activity, bundle) => MakeStatusBarTranslucent(activity)));

                static void MakeStatusBarTranslucent(Android.App.Activity activity)
                {
                    activity.Window.SetFlags(Android.Views.WindowManagerFlags.LayoutNoLimits, Android.Views.WindowManagerFlags.LayoutNoLimits);

                    activity.Window.ClearFlags(Android.Views.WindowManagerFlags.TranslucentStatus);

                    activity.Window.SetStatusBarColor(Android.Graphics.Color.Transparent);
                }
#endif
                })
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("AnekLatin_Condensed-SemiBold.ttf", "AnekSemibold");
                    fonts.AddFont("AnekLatin-Bold.ttf", "AnekBold");
                });

            builder.Services.AddSingleton<localdbDa>();
            
            builder.Services.AddSingleton<TargetsContent>();
            builder.Services.AddSingleton<TargetsContentViewModel>();

            builder.Services.AddTransient<AddTargetViewModel>();
            builder.Services.AddTransient<AddTarget>();

            builder.Services.AddTransient<WorkoutListViewModel>();
            builder.Services.AddTransient<WorkoutList>();

            builder.Services.AddTransient<WorkoutContentViewModel>();
            builder.Services.AddTransient<WorkoutContent>();

            builder.Services.AddTransient<PlayWorkout>();
            builder.Services.AddTransient<PlayWorkoutViewModel>();
#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
