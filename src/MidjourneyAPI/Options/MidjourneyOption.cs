namespace MidjourneyAPI.Options;

public class MidjourneyOption:DiscordOption
{
    private static MidjourneyOption _defaultOption = new MidjourneyOption(); 
    
    public int MaxWait { get; set; } = 100;

    public int Limit { get; set; } = 50;

    public void SetOption(MidjourneyOption newOption)
    {
        if (_defaultOption.MaxWait != newOption.MaxWait)
        {
            MaxWait = newOption.MaxWait;
        }
    
        if (_defaultOption.Limit != newOption.Limit)
        {
            Limit = newOption.Limit;
        }

        base.SetOption(newOption);
    }

    public MidjourneyOption Clone()
    {
        var option = new MidjourneyOption();
        option.MaxWait = MaxWait;
        option.Limit = Limit;
        option.ServerId = ServerId;
        option.ChannelId = ChannelId;
        option.SalaiToken = SalaiToken;
        return option;
    }
}