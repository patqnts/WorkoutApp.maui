using WorkoutApp.MVVM.ViewModel;

namespace WorkoutApp.MVVM.View;

public partial class PlayWorkout : ContentPage
{
	public PlayWorkout(PlayWorkoutViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}