# MidjourneyAPI

midjourney api by discord

## Support feature

- [x] Imagine
- [ ] Scale 
- [ ] Select

## Use for Binary

wait pr

## Use for dotnet

1. Create console app

   ```shell
   mkdir Midjourney.Example.Console && cd Midjourney.Example.Console
   dotnet new console
   dotnet add package MidjourneyAPI --version 1.0.0-beta.2305181719
   dotnet add package Micorsoft.Extensions.Logging.Console
   dotnet add pacakge Micorsoft.Extensions.DependencyInjection
   ```

2. Edit `Program.cs`

   ```c#
   var services = new ServiceCollection();
   services.AddLogging(builder =>
   {
       builder.SetMinimumLevel(LogLevel.Debug);
       builder.AddConsole();
   });
   services.AddMidjourney(builder =>
   {
       // Note: Please set your custome info.
       builder.Option.DiscordToken = "<your discord authorization value>";
       builder.Option.ChannelId = "<your discord server id>";
       builder.Option.ServerId = "<your midjourney bot channelId in discord>";
   });
   var provider = services.BuildServiceProvider();

   var midjourney = provider.GetRequiredService<Midjourney>() ?? throw new ArgumentNullException(nameof(Midjourney));
   // Imagine
   await midjourney.ImagineAsync("Labrador", loading: (message) =>
   {
       Console.WriteLine(message.Progress);
       if (message.Progress == "done")
       {
           Console.WriteLine($"{message.Content} -> {message.Url}");
       }
       return Task.CompletedTask;
   });
   ```

3. Run your code for `dotnet run`

### How to get `DiscordToken` `ServerId` `ChannelId`
1. Get `DiscordToken`

   [How to get DiscordToken](https://www.androidauthority.com/get-discord-token-3149920/)


2. Get `ServerId`

   Press `F12`, Open your discord `Server profile`, Find `/profile` for `Networking` and view `payload`, select `guild_id` in `mutual_guilds`


3. Get `ChannelId`

   Press `F12`, Select click your channel in discord, Find `/messages` for `Networking` and view `payload`, select `channel_id` in data list

## Example

1. git clone `https://github.com/binwan-dev/MidjourneyAPI`

2. config `SalaiToken` `ServerId` `ChannelId` at `Program.cs` in `src/example/MidjourneyAPI.Example`

   ```csharp Program.cs
   service.AddOptions<MidjourneyOption>().Configure(o => 
   {
       o.SalaiToken = "<your discord authorization value>";
       o.ServerId = "<your discord server id>";
       o.ChannelId = "<your midjourney bot channelId in discord>";
   });
   ```

2. run `example`

``` bash
cd MidjourneyAPI/src/example/MidjourneyAPI.Example.Console
dotnet run 
```

## Thanks

[https://github.com/erictik/midjourney-api](https://github.com/erictik/midjourney-api)