namespace PowerplantCodingChallenge.Models;

public class ProductionPlanPayload
{
    /// <summary>
    /// The load is the amount of energy (MWh) that need to be generated during one hour
    /// </summary>
    public float Load { get; set; }
    /// <summary>
    /// Costs for the PowerPlants 
    /// </summary>
    public Fuel? Fuels { get; set; }
    /// <summary>
    /// List of available PowerPlants
    /// </summary>
    public List<Powerplant> Powerplants { get; set; } = new List<Powerplant>();
}