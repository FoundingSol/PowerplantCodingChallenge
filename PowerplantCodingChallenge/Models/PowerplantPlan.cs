namespace PowerplantCodingChallenge.Models;

public class PowerplantPlan
{
    public PowerplantPlan() { }
    public PowerplantPlan (string? name, float p)
    {
        Name = name;
        P = p;
    }
    /// <summary>
    /// Name of the PowerPlant
    /// </summary>
    public string? Name { get; set; }
    /// <summary>
    /// Power Produced (multiple of 0.1 Mw)
    /// </summary>
    public float P { get; set; }
}
