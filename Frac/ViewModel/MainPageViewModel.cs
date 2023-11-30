using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Frac.Pages;

namespace Frac.ViewModel
{
    [QueryProperty("End","End")]
    public partial class MainPageViewModel : ObservableObject
    {
        [ObservableProperty]
        string _end;
        [RelayCommand]
        async Task OnStartAsync()
            =>await Shell.Current.GoToAsync(nameof(GamePage));

        [RelayCommand]
        public void SelectLevel(int level) 
            => DrawingHelper.StartingLevel = level;

        [RelayCommand]
        private void SelectLayers(int layers) 
            => DrawingHelper.StartingLayers = layers;
    }
}
