using Frac.GameObjects;
using Frac.Pages;
using Frac.Scenes;
using Frac.ViewModel;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using Orbit.Engine;
using SharpHook;

namespace Frac
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
            .UseOrbitEngine()
#if ANDROID
            .ConfigureLifecycleEvents(events => events.AddAndroid(android => android.OnCreate((activity, bundle) => MakeStatusBarTranslucent(activity))))
#endif
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .Services
            .AddTransient<DrawingSurface>()
            .AddSingleton<GamePage>()
            .AddTransient<MainScene>()
            .AddSingleton<GamePageViewModel>()
            .AddSingleton<MainPage>()
            .AddSingleton<MainPageViewModel>()
            .AddTransient<EndGamePage>()
            .AddTransient<EndGamePageViewModel>()
            .AddSingleton<IDeviceDisplay>(DeviceDisplay.Current);

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
#if ANDROID
        static void MakeStatusBarTranslucent(Android.App.Activity activity)
        {
            activity.Window.SetFlags(Android.Views.WindowManagerFlags.LayoutNoLimits, Android.Views.WindowManagerFlags.LayoutNoLimits);

            activity.Window.ClearFlags(Android.Views.WindowManagerFlags.TranslucentStatus);

            activity.Window.SetStatusBarColor(Android.Graphics.Color.Transparent);
        }
#endif
    }

}