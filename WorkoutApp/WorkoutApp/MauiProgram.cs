﻿using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using WorkoutApp.Dal;
using WorkoutApp.MVVM.View;
using WorkoutApp.MVVM.View.Content;
using WorkoutApp.MVVM.ViewModel;

namespace WorkoutApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            builder.Services.AddSingleton<localdbDa>();

            builder.Services.AddTransient<TargetsContent>();
            builder.Services.AddTransient<TargetsContentViewModel>();

            builder.Services.AddTransient<AddTargetViewModel>();
            builder.Services.AddTransient<AddTarget>();

            builder.Services.AddTransient<WorkoutListViewModel>();
            builder.Services.AddTransient<WorkoutList>();

            builder.Services.AddTransient<WorkoutContentViewModel>();
            builder.Services.AddTransient<WorkoutContent>();
#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
