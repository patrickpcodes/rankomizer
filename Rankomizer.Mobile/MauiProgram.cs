using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Rankomizer.Mobile.ViewModels;
using Syncfusion.Maui.Toolkit.Hosting;
using System.Text.Json.Serialization;
using System.Text.Json;
using Rankomizer.Domain.DTOs;

namespace Rankomizer.Mobile;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureSyncfusionToolkit()
			.ConfigureMauiHandlers(handlers =>
			{
			})
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFont("SegoeUI-Semibold.ttf", "SegoeSemibold");
				fonts.AddFont("FluentSystemIcons-Regular.ttf", FluentUI.FontFamily);
			});

#if DEBUG
		builder.Logging.AddDebug();
		builder.Services.AddLogging(configure => configure.AddDebug());
#endif
        //builder.Services.AddSingleton( new JsonSerializerOptions
        //{
        //    Converters = { new JsonStringEnumConverter( JsonNamingPolicy.CamelCase ), new RosterItemStatusJsonConverter() }
        //} );
        var jsonOptions = new JsonSerializerOptions();
        jsonOptions.Converters.Add( new RosterItemStatusJsonConverter() );
        jsonOptions.Converters.Add( new ItemTypeJsonConverter() );

        builder.Services.AddSingleton( jsonOptions );

        builder.Services.AddSingleton<ProjectRepository>();
		builder.Services.AddSingleton<TaskRepository>();
		builder.Services.AddSingleton<CategoryRepository>();
		builder.Services.AddSingleton<TagRepository>();
		builder.Services.AddSingleton<SeedDataService>();
		builder.Services.AddSingleton<ModalErrorHandler>();
		builder.Services.AddSingleton<MainPageModel>();
		builder.Services.AddSingleton<ProjectListPageModel>();
		builder.Services.AddSingleton<ManageMetaPageModel>();

		builder.Services.AddTransientWithShellRoute<ProjectDetailPage, ProjectDetailPageModel>("project");
		builder.Services.AddTransientWithShellRoute<TaskDetailPage, TaskDetailPageModel>("task");
        
        //HttpClient is a singleton to only have one accros the app
        //TODO Manage the cookies
        builder.Services.AddSingleton<HttpClient>();
        builder.Services.AddSingleton<GauntletApiService>();

        builder.Services.AddSingleton<GauntletViewModel>();



        return builder.Build();
	}
}
