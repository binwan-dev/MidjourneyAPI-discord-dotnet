# MidjourneyAPI

midjourney api by discord

## Use for dotnet

1. download `MidjourneyAPI` package

   ```bash
   dotnet add package MidjourneyAPI --version 1.0.0-beta.2305181719
   ```

2. Reference to [Example]()

## Use for Binary

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

   ``` shell
   cd MidjourneyAPI/src/example/MidjourneyAPI.Example
   dotnet run 
   ```
