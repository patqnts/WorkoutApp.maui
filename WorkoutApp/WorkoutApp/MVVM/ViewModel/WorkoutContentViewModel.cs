using CommunityToolkit.Mvvm.ComponentModel;
using WorkoutApp.MVVM.Model;
using CommunityToolkit.Mvvm.Input;
using WorkoutApp.Dal;
using System.Collections.ObjectModel;
using System.IO;

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

                // Save the image to local storage
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
            // Save the stream to a local file
            var localFilePath = Path.Combine(FileSystem.CacheDirectory, fileName);

            using (var fileStream = File.OpenWrite(localFilePath))
            {
                stream.CopyTo(fileStream);
            }

            return localFilePath;
        }
    }
}

