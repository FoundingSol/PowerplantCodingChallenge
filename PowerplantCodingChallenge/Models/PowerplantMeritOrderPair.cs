namespace PowerplantCodingChallenge.Models;

public class PowerplantMeritOrderPair
{
    public PowerplantMeritOrderPair (Powerplant powerplant, float meritOrder)
    {
        Powerplant = powerplant;
        MeritOrder = meritOrder;
        MinCost = meritOrder * powerplant.PMin;
        MaxCost = meritOrder * powerplant.PMax;
    }
    public Powerplant Powerplant { get; set; }
    public float MinCost { get; set; }
    public float MaxCost { get; set; }
    public float MeritOrder { get; set; }
}
