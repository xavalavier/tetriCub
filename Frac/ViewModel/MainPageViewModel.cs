using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Frac.Pages;

namespace Frac.ViewModel
{
    public partial class MainPageViewModel : ObservableObject
    {
        [RelayCommand]
        async Task OnStartAsync()
            =>await Shell.Current.GoToAsync(nameof(GamePage));

        [RelayCommand]
        public void SelectLevel(int level) 
            => DrawingHelper.Level = level;
        [RelayCommand]
        private void SelectLayers(int layers) 
            => DrawingHelper.StartingLayers = layers;
    }
}
