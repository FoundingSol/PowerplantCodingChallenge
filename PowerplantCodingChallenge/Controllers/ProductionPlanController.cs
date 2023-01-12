using Microsoft.AspNetCore.Mvc;
using PowerplantCodingChallenge.Models;
using System.Text.Json;

namespace PowerplantCodingChallenge.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductionPlanController : ControllerBase
    {
        [HttpPost(Name = "productionplan")]
        public ProductionPlanResponse ProductionPlan(ProductionPlanPayload jsonPayload)
        {
            // based on the cost of the fuels of each powerplant, the merit-order can be determined which is the starting point for deciding which powerplants should be switched on and how much power they will deliver.
            ProductionPlanResponse productionPlanResponse = new ProductionPlanResponse();

            List<PowerplantMeritOrderPair> meritOrderPlant = CalculateMeritOder(jsonPayload);
            meritOrderPlant = meritOrderPlant.OrderBy(x => x.MeritOrder).ToList();

            float availableDemand = jsonPayload.Load;
            for (int i = 0; i < meritOrderPlant.Count; i++)
            {
                Powerplant powerPlant = meritOrderPlant[i].Powerplant;
                PowerplantPlan powerplantPlan = new PowerplantPlan();

                float powerSupply = 0f;

                if (availableDemand > 0)
                { 
                    switch (powerPlant.Type) 
                    { 
                        case "windturbine":
                            powerSupply = powerPlant.PMax / (100 / jsonPayload.Fuels!.Wind);
                            break;
                        case "gasfired" or "turbojet":
                            if (availableDemand < powerPlant.PMin)
                            {
                                // if demand is less than PMin check for smaller powerplant if cheaper
                                var minCost = meritOrderPlant[i].MinCost;
                                bool foundCheaper = false;

                                for (int j = i + 1; j < meritOrderPlant.Count && !foundCheaper; j++)
                                {
                                    var comparisonPlant = meritOrderPlant[j];
                                    if (availableDemand >= comparisonPlant.Powerplant.PMin && availableDemand <= comparisonPlant.Powerplant.PMax)
                                    {
                                        var cost = meritOrderPlant[j].MeritOrder * availableDemand;
                                        if (cost < minCost)
                                        {
                                            foundCheaper = true;
                                            productionPlanResponse.PowerplantPlans.Add(new PowerplantPlan(comparisonPlant.Powerplant.Name, availableDemand));
                                            meritOrderPlant.Remove(comparisonPlant);
                                            availableDemand = powerSupply = 0;
                                        }
                                    }
                                }

                                if (foundCheaper)
                                {
                                    break;
                                }
                            }

                            powerSupply = Math.Min(powerPlant.PMax, availableDemand);
                            powerSupply = Math.Max(powerSupply, powerPlant.PMin);
                            
                            break;
                        default:
                            break;
                    }

                    availableDemand -= powerSupply;
                }

                powerplantPlan.Name = powerPlant.Name;
                powerplantPlan.P = powerSupply;

                productionPlanResponse.PowerplantPlans.Add(powerplantPlan);
            }

            return productionPlanResponse;
        }

        private List<PowerplantMeritOrderPair> CalculateMeritOder(ProductionPlanPayload jsonPayload)
        {
            List<PowerplantMeritOrderPair> meritOrderScores = new List<PowerplantMeritOrderPair>();
            foreach (Powerplant powerplant in jsonPayload.Powerplants)
            {
                // the cost of generating 1 MWh is x euros
                float meritOrder = 0;
                switch (powerplant.Type)
                {
                    case "gasfired":
                        meritOrder = jsonPayload.Fuels!.Gas / powerplant.Efficiency;

                        // each MWh generated creates 0.3 ton of CO2
                        float co2Cost = 0.3f * jsonPayload.Fuels!.CO2;
                        meritOrder += co2Cost;

                        meritOrderScores.Add(new PowerplantMeritOrderPair(powerplant, meritOrder));
                        break;
                    case "turbojet":
                        meritOrder = jsonPayload.Fuels!.Kerosine / powerplant.Efficiency;
                        meritOrderScores.Add(new PowerplantMeritOrderPair(powerplant, meritOrder));
                        break;
                    case "windturbine":
                        //Wind-turbines do not consume 'fuel' and thus are considered to generate power at zero price.
                        meritOrder = 0;
                        meritOrderScores.Add(new PowerplantMeritOrderPair(powerplant, meritOrder));
                        break;
                    default:
                        meritOrder = float.MaxValue;
                        meritOrderScores.Add(new PowerplantMeritOrderPair(powerplant, meritOrder));
                        break;
                }
            }

            return meritOrderScores;
        }
    }
}