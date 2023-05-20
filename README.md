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
       builder.Option.SalaiToken = "<your discord authorization value>";
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

## Example

1. git clone `https://github.com/binwan-dev/MidjourneyAPI`

2. run `example`

``` bash
cd MidjourneyAPI/src/example/MidjourneyAPI.Example.Console
dotnet run 
```

## Thanks

[https://github.com/erictik/midjourney-api](https://github.com/erictik/midjourney-api)