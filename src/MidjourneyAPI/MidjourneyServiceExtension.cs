namespace Microsoft.Extensions.DependencyInjection;

public static class MidjourneyServiceExtension
{
    public static IServiceCollection AddMidjourney(this IServiceCollection services, Action<MidjourneyServiceBuilder> builderAction)
    {
        var builder = new MidjourneyServiceBuilder();
        builderAction.Invoke(builder);
        builder.CheckConfig();
        services.AddSingleton(builder);

        services.AddSingleton<IConfigureOptions<MidjourneyOption>>(
            new ConfigureOptions<MidjourneyOption>(o => o.SetOption(builder.Option)));
        services.AddOptions<MidjourneyOption>("MidjourneyOption");
        services.AddSingleton<DiscordCaller>();
        services.AddSingleton<Midjourney>();

        return services;
    }
}
