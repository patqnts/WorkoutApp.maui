using WorkoutApp.MVVM.View;

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
        }
    }
}
