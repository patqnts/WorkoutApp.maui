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
            try
            {
                var wo = await localdbDa.GetWorkoutsById(WorkoutTarget.TargetId);

                if(wo != null)
                {
                    Workout = new(wo);
                }
               
            }
            catch (Exception ex)
            {
                
            }
            
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
            var aw = await localdbDa.Create(new Workout
            {
                WorkoutTargetId = WorkoutTarget.TargetId,
                Index = Workout.Count + 1
            });


            Workout.Add(aw);
            //await LoadTarget();
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
                {"Workout", workout },
                {"ImageString", workout.Description }
            });
        }

        [RelayCommand]
        public async Task Play()
        {

            await ReorderItems();

            await Shell.Current.GoToAsync(nameof(PlayWorkout), true, new Dictionary<string, object>
            {
                {"WorkoutTarget", WorkoutTarget },
                {"Workout", Workout },
                {"CurrentWorkout", WorkoutTarget.Workouts.FirstOrDefault() }
            });
        }

        [RelayCommand]
        public async Task Back()
        {
            await Shell.Current.GoToAsync("..");
        }

        private async Task ReorderItems()
        {
            if (Workout.Count == 0) { return; }

            foreach (Workout workout in Workout)
            {
                workout.Index = Workout.IndexOf(workout);
                await localdbDa.Update(workout);
            }

            var wo = await localdbDa.GetWorkoutsById(WorkoutTarget.TargetId);
            
            Workout = new(wo);
        }
    }
}
