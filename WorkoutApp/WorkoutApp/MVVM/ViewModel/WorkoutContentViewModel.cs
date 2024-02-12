using CommunityToolkit.Mvvm.ComponentModel;
using WorkoutApp.MVVM.Model;
using CommunityToolkit.Mvvm.Input;
using WorkoutApp.Dal;
using System.Collections.ObjectModel;
using System.IO;
using System.Diagnostics;

namespace WorkoutApp.MVVM.ViewModel
{
    [QueryProperty(nameof(Workout), "Workout")]
    [QueryProperty("ImageString", "ImageString")]
    
    public partial class WorkoutContentViewModel : ObservableObject
    {
        private readonly localdbDa localdbDa;
       

        public WorkoutContentViewModel(localdbDa localdbDa)
        {
            this.localdbDa = localdbDa;
        
        }

        [ObservableProperty]
        private Workout workout;

        
        private string imageString;

        public string ImageString
        {
            get => imageString;
            set
            {
                SetProperty(ref imageString, value);
                ImageSource = ImageSource.FromFile(imageString);
            }
        }

        [ObservableProperty]
        ImageSource imageSource = "dotnet_bot.png";


        [RelayCommand]
         async Task SaveInfo()
        {
            if (Workout != null)
            {
                await localdbDa.Update(Workout);

                // Delete the image file associated with the workout
                if (!string.IsNullOrEmpty(Workout.Description))
                {
                    DeleteImageFile(Workout.Description);
                }

                ObservableCollection<Workout> OCWorkouts = new(await localdbDa.GetWorkoutsById(Workout.WorkoutTargetId));
                await Shell.Current.GoToAsync("..", true, new Dictionary<string, object>
                {
                    {"Workout", OCWorkouts }
                });
            }
         }


        [RelayCommand]
        async Task UploadImage()
        {
            try
            {
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Please pick a photo",
                    FileTypes = FilePickerFileType.Images
                });

                if (result == null)
                {
                    return;
                }

                var stream = await result.OpenReadAsync();

                // Save the image to the app's persistent path
                var imagePath = SaveImageToLocalFile(stream, result.FileName);

                // Update the DisplayedImageSource property
                ImageSource = ImageSource.FromFile(imagePath);

                // Update the model's ImageSource property with the image path
                Workout.Description = imagePath;
            }
            catch (Exception ex)
            {
                // Handle exception
            }
        }


        private string SaveImageToLocalFile(Stream stream, string fileName)
        {
            // Get the app's persistent path on Android
            string persistentPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

            // Combine the persistent path with a subfolder for images
            string imageFolderPath = Path.Combine(persistentPath, "images");

            // Ensure the directory exists, create if not
            Directory.CreateDirectory(imageFolderPath);

            // Save the stream to a local file in the images subfolder
            var localFilePath = Path.Combine(imageFolderPath, fileName);

            using (var fileStream = File.OpenWrite(localFilePath))
            {
                stream.CopyTo(fileStream);
            }
            Debug.WriteLine($"appdatadir Directory: {FileSystem.AppDataDirectory}");
            Debug.WriteLine($"special Directory: {Environment.SpecialFolder.Personal}");
            return localFilePath;
        }



        private void DeleteImageFile(string imagePath)
        {
            try
            {
                // Delete the image file from local storage
                if (File.Exists(imagePath))
                {
                    File.Delete(imagePath);
                }
            }
            catch (Exception ex)
            {
                // Handle exception if needed
            }
        }

    }
}

