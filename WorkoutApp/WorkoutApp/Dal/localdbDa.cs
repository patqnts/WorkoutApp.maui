using SQLite;
using WorkoutApp.MVVM.Model;

namespace WorkoutApp.Dal
{
    public class localdbDa
    {
        private const string DB_NAME = "workout_local_db.db3";
        private readonly SQLiteAsyncConnection connection;

        public localdbDa()
        {
            connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, DB_NAME));

            connection.CreateTableAsync<Workout>();
            connection.CreateTableAsync<WorkoutTarget>();
        }
        //TARGETS
        public async Task<List<WorkoutTarget>> GetTargetWorkouts()
        {
            return await connection.Table<WorkoutTarget>().ToListAsync();
        }

        public async Task<WorkoutTarget> GetTargetWorkoutById(int id)
        {
            return await connection.Table<WorkoutTarget>().Where(x => x.TargetId == id).FirstOrDefaultAsync();
        }

        public async Task<WorkoutTarget> CreateTarget(WorkoutTarget target)
        {
            await connection.InsertAsync(target);
            return await connection.Table<WorkoutTarget>().Where(x => x.TargetId == target.TargetId).FirstOrDefaultAsync();
        }

        public async Task UpdateTarget(WorkoutTarget target)
        {
            await connection.UpdateAsync(target);
        }

        public async Task DeleteTarget(WorkoutTarget target)
        {
            await connection.DeleteAsync(target);
        }

        //WORKOUTS
        public async Task<List<Workout>> GetWorkouts()
        {
            return await connection.Table<Workout>().ToListAsync();
        }

        public async Task<List<Workout>>GetWorkoutsById(int id)
        {
            return await connection.Table<Workout>().Where(x => x.WorkoutTargetId == id).ToListAsync();
        }

        public async Task<Workout> Create(Workout workout)
        {
            await connection.InsertAsync(workout);
            return await connection.Table<Workout>().Where(x => x.Id == workout.Id).FirstOrDefaultAsync();
        }

        public async Task Update(Workout workout)
        {
            await connection.UpdateAsync(workout);
        }

        public async Task Delete(Workout workout)
        {
            await connection.DeleteAsync(workout);
        }

    }
}
