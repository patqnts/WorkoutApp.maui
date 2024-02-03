using WorkoutApp.MVVM.View;
using WorkoutApp.MVVM.View.Content;

namespace WorkoutApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(TargetsContent), typeof(TargetsContent));
            Routing.RegisterRoute(nameof(WorkoutList), typeof(WorkoutList));        
            Routing.RegisterRoute(nameof(WorkoutContent), typeof(WorkoutContent));
            Routing.RegisterRoute(nameof(AddTarget), typeof(AddTarget));
            Routing.RegisterRoute(nameof(PlayWorkout), typeof(PlayWorkout));
        }

        //protected override bool OnBackButtonPressed()
        //{
        //    return false;           
        //}
    }
}
