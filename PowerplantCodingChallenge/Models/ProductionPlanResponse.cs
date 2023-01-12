namespace PowerplantCodingChallenge.Models;

public class ProductionPlanResponse
{
    /// <summary>
    /// List of how much power each powerplant should deliver
    /// </summary>
    public List<PowerplantPlan> PowerplantPlans { get; set; } = new List<PowerplantPlan>();
}
