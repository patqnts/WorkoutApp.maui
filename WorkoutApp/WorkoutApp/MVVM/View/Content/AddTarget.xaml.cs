using WorkoutApp.MVVM.ViewModel;

namespace WorkoutApp.MVVM.View.Content;

public partial class AddTarget : ContentPage
{
	public AddTarget(AddTargetViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}