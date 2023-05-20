namespace Microsoft.Extensions.DependencyInjection;

public class MidjourneyServiceBuilder
{
    public MidjourneyOption Option { get; set; } = new();

    public void CheckConfig()
    {
        Option.CheckConfig();
    }
}
