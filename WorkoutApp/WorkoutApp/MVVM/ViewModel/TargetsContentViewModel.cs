using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using WorkoutApp.Dal;
using WorkoutApp.MVVM.Model;
using WorkoutApp.MVVM.View;

namespace WorkoutApp.MVVM.ViewModel
{
    [QueryProperty(nameof(WorkoutTarget), "WorkoutTarget")]
    public partial class TargetsContentViewModel : ObservableObject
    {
        private readonly localdbDa localdbDa;
        public TargetsContentViewModel(localdbDa localdb) 
        {
            this.localdbDa = localdb;
            LoadTargets();
        }

        [ObservableProperty]
        ObservableCollection<WorkoutTarget> workoutTarget = new();

        public async Task LoadTargets()
        {
            var loadedTargets = await localdbDa.GetTargetWorkouts();

            WorkoutTarget.Clear();

            WorkoutTarget = new(loadedTargets);
        }
        async Task ClearTargets()
        {
            var loadedTargets = await localdbDa.GetTargetWorkouts();

            foreach (var target in loadedTargets)
            {
                WorkoutTarget.Remove(target);
            }

            WorkoutTarget.Clear();
        }

        [RelayCommand]
        async Task AddTarget(WorkoutTarget wt)
        {
            bool isEdit = false;
            bool isAdd = false;
            if (wt != null)
            {
                isEdit = true;
                wt = await localdbDa.GetTargetWorkoutById(wt.TargetId);
                await Shell.Current.GoToAsync(nameof(AddTarget), true, new Dictionary<string, object>
                {
                    {"WorkoutTarget", wt },
                    {"IsEdit", isEdit }
                });
            }
            else
            {
                await Shell.Current.GoToAsync(nameof(AddTarget), true, new Dictionary<string, object>
                {
                    {"IsAdd", isAdd }
                });  
            } 
        }

        [RelayCommand]
        async Task DeleteTarget(WorkoutTarget wt)
        {
            if (wt == null) return;

            bool userConfirmed = await Application.Current.MainPage.DisplayAlert("Confirm Deletion", "Are you sure you want to delete this target?", "Yes", "No");

            if (!userConfirmed)
                return;

            List<Workout> wo = await localdbDa.GetWorkoutsById(wt.TargetId);

            if (wo.Count < 1)
            {
                foreach (Workout workout in wo)
                {
                    await this.localdbDa.Delete(workout);
                }
            }

            await this.localdbDa.DeleteTarget(wt);
            WorkoutTarget.Remove(wt);
        }    

        [RelayCommand]
        private async Task SelectTargetWorkout(WorkoutTarget wt)
        {         
            try 
            { 
                if (wt == null)
                {
                    return;
                }

                wt.Workouts = new(await localdbDa.GetWorkoutsById(wt.TargetId));
                
                await Shell.Current.GoToAsync(nameof(WorkoutList), true, new Dictionary<string, object>
                {
                    {"Workout", wt.Workouts },
                    {"WorkoutTarget", wt }
                });  
            }
            catch(Exception ex) 
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message , "OK");
            }
        }
    }
}
