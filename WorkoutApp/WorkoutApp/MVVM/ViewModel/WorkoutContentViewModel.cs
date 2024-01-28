using CommunityToolkit.Mvvm.ComponentModel;
using WorkoutApp.MVVM.Model;
using CommunityToolkit.Mvvm.Input;
using WorkoutApp.Dal;
using System.Collections.ObjectModel;

namespace WorkoutApp.MVVM.ViewModel
{
    [QueryProperty(nameof(Workout), "Workout")]
    public partial class WorkoutContentViewModel : ObservableObject
    {
        private readonly localdbDa localdbDa;

        public WorkoutContentViewModel(localdbDa localdbDa)
        {
            this.localdbDa = localdbDa;
        }

        [ObservableProperty]
        private Workout workout;

        [RelayCommand]
         async Task SaveInfo()
        {
            if (Workout != null)
            {
                await localdbDa.Update(Workout);

                ObservableCollection<Workout> OCWorkouts = new(await localdbDa.GetWorkoutsById(Workout.WorkoutTargetId));
                await Shell.Current.GoToAsync("..", true, new Dictionary<string, object>
                {
                    {"Workout", OCWorkouts }
                });
            }
         }
    }
}
