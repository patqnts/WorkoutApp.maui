using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using WorkoutApp.Dal;
using WorkoutApp.MVVM.Model;
using WorkoutApp.MVVM.View;

namespace WorkoutApp.MVVM.ViewModel
{
    [QueryProperty(nameof(Workout), "Workout")]
    [QueryProperty(nameof(WorkoutTarget), "WorkoutTarget")]
    public partial class WorkoutListViewModel : ObservableObject
    {
        private readonly localdbDa localdbDa;
        public WorkoutListViewModel(localdbDa localdb) 
        {
            localdbDa = localdb;
            LoadTarget();
        }

        [ObservableProperty]
        private ObservableCollection<Workout> workout;

        [ObservableProperty]
        private WorkoutTarget workoutTarget;

        private async Task LoadTarget()
        {
            var wo = await localdbDa.GetWorkoutsById(WorkoutTarget.TargetId);

            Workout = new(wo);
        }


        [RelayCommand]
        async Task DeleteWorkout(Workout workout)
        {           
            if(workout == null) { return; }
            await localdbDa.Delete(workout);
            Workout.Remove(workout);
        }

        async void ClearDB()
        {
            foreach (Workout workout in Workout) 
            { await localdbDa.Delete(workout); }
            
        }

        [RelayCommand]
        async Task AddWorkout()
        {
            await localdbDa.Create(new Workout
            {
                WorkoutTargetId = WorkoutTarget.TargetId,
            });

            await LoadTarget();
        }

        [RelayCommand]
        public async Task SelectWorkout(Workout workout)
        {
            if (workout == null)
            {
                return;
            }

            await Shell.Current.GoToAsync(nameof(WorkoutContent), true, new Dictionary<string, object>
            {
                {"Workout", workout }
            });
        }
    }
}
