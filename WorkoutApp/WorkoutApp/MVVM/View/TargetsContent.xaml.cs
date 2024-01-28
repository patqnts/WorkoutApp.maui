using WorkoutApp.MVVM.ViewModel;

namespace WorkoutApp.MVVM.View;

public partial class TargetsContent : ContentPage
{
	public TargetsContent(TargetsContentViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}