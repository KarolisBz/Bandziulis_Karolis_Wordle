using Microsoft.Extensions.Logging;

namespace Wordle_Karolis_G00417529
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    // adding custom fonts
                    fonts.AddFont("Heavenly Christmas - Personal Use.otf", "HeavenlyChristmas");
                    fonts.AddFont("Playful Christmas-Personal Use.otf", "PlayfulChristmas");
                    fonts.AddFont("ChristmasLightsIndoor.ttf", "ChristmasLightsIndoor");
                    fonts.AddFont("Merry Deer ttf.ttf", "MerryDeer");
                    fonts.AddFont("Little Santa Personal Use Only.otf", "LittleSanta");
                });

#if DEBUG
		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}