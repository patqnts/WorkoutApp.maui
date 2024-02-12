using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using WorkoutApp.Dal;
using WorkoutApp.MVVM.Model;
using WorkoutApp.MVVM.View;

namespace WorkoutApp.MVVM.ViewModel
{
    [QueryProperty(nameof(WorkoutTarget), "WorkoutTarget")]
    [QueryProperty(nameof(Workout), "Workout")]
    [QueryProperty("CurrentWorkout", "CurrentWorkout")]
    
    public partial class PlayWorkoutViewModel : ObservableObject
    {
        private readonly localdbDa localdb;

        public PlayWorkoutViewModel(localdbDa localdb)
        {
            this.localdb = localdb;
        }

        [ObservableProperty]
        bool isResting;

        [ObservableProperty]
        private WorkoutTarget workoutTarget;

        [ObservableProperty]
        private ObservableCollection<Workout> workout;

        [ObservableProperty]
        private Workout currentWorkout;

        [ObservableProperty]
        private int currentWorkoutIndex = 0;

        [ObservableProperty]
        private int currentSet = 1;

        [ObservableProperty]
        private int initialSeconds = 30;

        [ObservableProperty]
        private bool isRunning;

        [ObservableProperty]
        private TimeSpan elapsedTime = TimeSpan.FromMinutes(1);

        [RelayCommand]
        [Obsolete]
        private void StartPause()
        {
            IsRunning = !IsRunning;

            if (IsRunning)
            {
                IsResting = true;
                Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                {
                    if (IsRunning)
                    {
                        ElapsedTime = ElapsedTime.Add(TimeSpan.FromSeconds(-1));

                        if (ElapsedTime.TotalSeconds <= 0)
                        {
                            // Stop the timer when elapsed time reaches 0
                            Stop();
                        }
                    }

                    return IsRunning; // Repeat if the timer is still running
                });
            }
        }

        [RelayCommand]
        void Reset()
        {
            ElapsedTime = TimeSpan.FromSeconds(InitialSeconds);
            if (!IsRunning)
            {
                StartPause();
            }
        }

        [RelayCommand]
        async Task Stop()
        {
            ElapsedTime = TimeSpan.Zero;

            IsRunning = false;
            IsResting = false;

            if (CurrentWorkoutIndex < Workout.Count)
            {
                if (CurrentSet < CurrentWorkout.Sets)
                {
                    // Do something for the current set
                    CurrentSet++;
                }
                else
                {
                    if (CurrentWorkoutIndex < Workout.Count - 1)
                    {
                        // Move to the next workout
                        CurrentSet = 1;
                        CurrentWorkoutIndex++;                     
                        CurrentWorkout = Workout[CurrentWorkoutIndex];
                        if (string.IsNullOrEmpty(CurrentWorkout.Description))
                        {
                            CurrentWorkout.Description = "dotnet_bot.png";
                        }
                    }
                    else
                    {
                        await ExitPlay();
                    }
                }
            }          
        }

        [RelayCommand]
        void Finish()
        {
            IsResting = true;
            InitialSeconds = Workout[CurrentWorkoutIndex].RestTime;
            ElapsedTime = TimeSpan.FromSeconds(InitialSeconds);
            StartPause();
        }

        [RelayCommand]
        async Task ExitPlay()
        {
            //WorkoutTarget.Workouts = new(await localdb.GetWorkoutsById(WorkoutTarget.TargetId));

            

            //await Shell.Current.GoToAsync(nameof(WorkoutList), true, new Dictionary<string, object>
            //    {
            //        {"Workout", WorkoutTarget.Workouts },
            //        {"WorkoutTarget", WorkoutTarget}
            //    });
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        void ResetSession()
        {
            Stop();
            CurrentSet = 1;
            CurrentWorkoutIndex = 0;
            CurrentWorkout = Workout[CurrentWorkoutIndex];
        }

    }
}
