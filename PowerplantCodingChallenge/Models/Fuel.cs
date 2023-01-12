using System.Text.Json.Serialization;

namespace PowerplantCodingChallenge.Models;

public class Fuel
{
    [JsonPropertyName("gas(euro/MWh)")]
    public float Gas { get; set; }
    [JsonPropertyName("kerosine(euro/MWh)")]
    public float Kerosine { get; set; }
    [JsonPropertyName("co2(euro/ton)")]
    public float CO2 { get; set; }
    [JsonPropertyName("wind(%)")]
    public float Wind { get; set; }
}
