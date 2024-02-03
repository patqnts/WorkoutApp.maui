using System.Collections.ObjectModel;
using SQLite;

namespace WorkoutApp.MVVM.Model
{
    public class WorkoutTarget
    {
        [PrimaryKey]
        [AutoIncrement]
        [Column("target_id")]
        public int TargetId { get; set; }

        [Column("target_name")]
        public string Name { get; set; } = string.Empty;

        [Ignore]
        public ObservableCollection<Workout> Workouts { get; set; }

        [Column("rest_intervals")]
        public int RestIntervals { get; set; } = -1;
    }
}
