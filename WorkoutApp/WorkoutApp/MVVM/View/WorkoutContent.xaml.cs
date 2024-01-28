using WorkoutApp.MVVM.ViewModel;

namespace WorkoutApp.MVVM.View;

public partial class WorkoutContent : ContentPage
{
	public WorkoutContent(WorkoutContentViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}