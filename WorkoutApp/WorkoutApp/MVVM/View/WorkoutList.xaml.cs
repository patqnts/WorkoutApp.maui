using WorkoutApp.MVVM.ViewModel;

namespace WorkoutApp.MVVM.View;

public partial class WorkoutList : ContentPage
{
	public WorkoutList(WorkoutListViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}