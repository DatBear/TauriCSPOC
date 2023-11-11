using System.Net;
using Microsoft.Extensions.Logging;
using nativemaui.Websockets;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace nativemaui;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder()
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

        JsonConvert.DefaultSettings = () => new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
			ContractResolver = new CamelCasePropertyNamesContractResolver(),
        };

        var loader = new AssemblyLoader();
		loader.Load(Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "../../../../../../nativeexternal/bin/Debug/net7.0/nativeexternal.dll"));

        var server = new Server(IPAddress.Any, 4051, loader);
        server.Start();


        return builder.Build();
	}
}
