using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using WorkoutApp.Dal;
using WorkoutApp.MVVM.Model;
using WorkoutApp.MVVM.View;

namespace WorkoutApp.MVVM.ViewModel
{
    public partial class TargetsContentViewModel : ObservableObject
    {
        private readonly localdbDa localdbDa;
        public TargetsContentViewModel(localdbDa localdb) 
        {
            this.localdbDa = localdb;
            LoadTargets();
        }

        [ObservableProperty]
        ObservableCollection<WorkoutTarget> workoutTargets = new();
        public async Task LoadTargets()
        {
            var loadedTargets = await localdbDa.GetTargetWorkouts();

            WorkoutTargets.Clear();

            foreach (var target in loadedTargets)
            {
                WorkoutTargets.Add(target);
            }
        }
        async Task ClearTargets()
        {
            var loadedTargets = await localdbDa.GetTargetWorkouts();

            foreach (var target in loadedTargets)
            {
                WorkoutTargets.Remove(target);
            }

            WorkoutTargets.Clear();
        }

        [RelayCommand]
        async Task AddTarget()
        {     
            await AddTargetTest();
        }

        [RelayCommand]
        async Task DeleteTarget(WorkoutTarget wt)
        {
            if (wt == null) return;

            List<Workout> wo = await localdbDa.GetWorkoutsById(wt.TargetId);

            if (wo.Count < 1)
            {
                foreach (Workout workout in wo)
                {
                    await this.localdbDa.Delete(workout);
                }
            }

            await this.localdbDa.DeleteTarget(wt);
            WorkoutTargets.Remove(wt);
        }

        private async Task AddTargetTest()
        {
            Random r = new Random();
            var addedtarget = await localdbDa.CreateTarget(new WorkoutTarget
            {
                Name = $"This is pat workout {r.Next(1,500)}",
                RestIntervals = 60
            });
            WorkoutTargets.Add(addedtarget);

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
