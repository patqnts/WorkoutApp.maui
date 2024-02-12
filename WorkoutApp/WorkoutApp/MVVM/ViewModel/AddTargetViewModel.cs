using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;
using WorkoutApp.Dal;
using WorkoutApp.MVVM.Model;
using WorkoutApp.MVVM.View;

namespace WorkoutApp.MVVM.ViewModel
{
    [QueryProperty(nameof(WorkoutTarget), "WorkoutTarget")]
    [QueryProperty("IsEdit", "IsEdit")]
    [QueryProperty("IsAdd", "IsAdd")]
    public partial class AddTargetViewModel : ObservableObject
    {
        private readonly localdbDa localdb;
        public AddTargetViewModel(localdbDa localdb)
        {
            this.localdb = localdb;
        }

        [ObservableProperty]
        private WorkoutTarget workoutTarget = new WorkoutTarget()
        {
            Name = "Workout Target Name",
        };

        [ObservableProperty]
        private bool isEdit;
        
        [ObservableProperty]
        private bool isAdd;

        public ICommand ToggleCommand => new Command(ToggleMethod);

        [RelayCommand]
        private async Task SaveTarget()
        {

            WorkoutTarget.DateModified = DateTime.Now;
            await localdb.CreateTarget(WorkoutTarget);

            await GoBack();
        }

        [RelayCommand]
        private async Task EditTarget()
        {
            List<Workout> wo = await localdb.GetWorkoutsById(WorkoutTarget.TargetId);
             
            if(wo.Any())
            {
                WorkoutTarget.RestIntervals = wo.Count;
            }

            WorkoutTarget.DateModified = DateTime.Now;
            await localdb.UpdateTarget(WorkoutTarget);

            await GoBack();
        }

        [RelayCommand]
        private async Task Cancel()
        {
            await Shell.Current.GoToAsync("..");
        }

        private void ToggleMethod()
        {
            if (IsEdit)
            {
                // Execute EditTargetCommand logic
                EditTargetCommand.Execute(null);
            }
            else
            {
                // Execute SaveTargetCommand logic
                SaveTargetCommand.Execute(null);
            }
            // Add any additional logic as needed
        }

        async Task GoBack()
        {
            ObservableCollection<WorkoutTarget> wo = new(await this.localdb.GetTargetWorkouts());

            await Shell.Current.GoToAsync("..", true, new Dictionary<string, object>
            {
                {"WorkoutTarget", wo }
            });
        }
    }
}
