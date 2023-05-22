using Microsoft.Extensions.DependencyInjection;
using MidjourneyAPI.Dtos;
using MidjourneyAPI.Options;
using MidjourneyAPI.Utils;

namespace MidjourneyAPI.Test;

public class MidjourneyTest
{
    [Fact]
    public void TestImagine()
    {
        var service = new ServiceCollection();
        service.AddLogging();
        service.AddOptions<MidjourneyOption>().Configure(o =>
        {
            o.ServerId = "serverId";
            o.ChannelId = "channelId";
            o.DiscordToken = "token";
        });
        service.AddSingleton<DiscordCaller>();
        service.AddSingleton<Midjourney>();
        var provider = service.BuildServiceProvider();
        var midjourney = provider.GetService<Midjourney>();
        var loading = (MidjourneyMessageDto msg) =>
        {
            Console.WriteLine(msg.Progress);
            return Task.CompletedTask;
        };
        var result = midjourney.ImagineAsync("http://mmbiz.qpic.cn/mmbiz_jpg/WAYSdUKc8u6qsjXbBqWaVbdv8ZDjlTiaRuMar7dyTAXCYM5HodW3Zv1wonbXLHxVDxkIicFcTwFGjJQ0IhvNRX9w/0 Tech-inspired, nutritionally healthy, and featuring a blue color scheme product poster.",loading:loading).Result;
    }
}