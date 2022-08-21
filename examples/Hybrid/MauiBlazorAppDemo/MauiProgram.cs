using Microsoft.AspNetCore.Components.WebView.Maui;
using MauiBlazorAppDemo.Data;

using Microsoft.Extensions.Logging;

namespace MauiBlazorAppDemo;

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
			});

		builder.Services.AddMauiBlazorWebView();
        
		#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();

        builder.Services.AddLogging(logging =>
        {
            logging.AddDebug();
        });
        #endif

        builder.Services.AddSingleton<WeatherForecastService>();

		return builder.Build();
	}
}
