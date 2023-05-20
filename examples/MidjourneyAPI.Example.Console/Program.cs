// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MidjourneyAPI;

var services = new ServiceCollection();
services.AddLogging(builder =>
{
    builder.SetMinimumLevel(LogLevel.Debug);
    builder.AddConsole();
});
services.AddMidjourney(builder =>
{
    builder.Option.DiscordToken = "<your discord authorization value>";
    builder.Option.ChannelId = "<your discord server id>";
    builder.Option.ServerId = "<your midjourney bot channelId in discord>";
});
var provider = services.BuildServiceProvider();

var midjourney = provider.GetRequiredService<Midjourney>() ?? throw new ArgumentNullException(nameof(Midjourney));
await midjourney.ImagineAsync("Labrador", loading: (message) =>
{
    Console.WriteLine(message.Progress);
    if (message.Progress == "done")
    {
        Console.WriteLine($"{message.Content} -> {message.Url}");
    }
    return Task.CompletedTask;
});
