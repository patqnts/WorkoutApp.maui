using SQLite;

namespace WorkoutApp.MVVM.Model
{
    public class Workout
    {
        [PrimaryKey]
        [AutoIncrement]
        [Column("id")]
        public int Id { get; set; }

        [Column("workout_name")]
        public string Name { get; set; } = "Workout Name";

        [Column("workout_target_id")]
        public int WorkoutTargetId { get; set; }

        [Column("workout_description")]
        public string Description { get; set; } = "This workout has no description";

        [Column("imgsrc")]
        public string ImageSource { get; set; } = string.Empty;

        [Column("reps")]
        public int Reps { get; set; } = 8;

        [Column("sets")]
        public int Sets { get; set; } = 4;
    }
}
