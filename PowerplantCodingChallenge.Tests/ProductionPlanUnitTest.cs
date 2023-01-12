using System.IO;
using System.Text.Json;

namespace PowerplantCodingChallenge.Tests
{
    public class ProductionPlanTests
    {
        private readonly List<ProductionPlanPayload> payloads = new List<ProductionPlanPayload>();

        [SetUp]
        public async Task Setup()
        {
            List<string> inputFiles = new List<string>();

            inputFiles.Add(@"..\..\..\payload1.json");
            inputFiles.Add(@"..\..\..\payload2.json");
            inputFiles.Add(@"..\..\..\payload3.json");

            foreach (var file in inputFiles)
            {
                using FileStream openStream = File.OpenRead(file);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };

                ProductionPlanPayload? payload = await JsonSerializer.DeserializeAsync<ProductionPlanPayload>(openStream, options);
                if (payload != null)
                {
                    payloads.Add(payload);
                }
            }
        }

        [Test]
        public void TestProductionPlanController()
        {
            ProductionPlanController productionPlanController = new ProductionPlanController();

            foreach (ProductionPlanPayload payload in payloads)
            {
                ProductionPlanResponse response = productionPlanController.ProductionPlan(payload);

                Assert.IsNotNull(response.PowerplantPlans);
                Assert.IsNotEmpty(response.PowerplantPlans);

                foreach (var plant in response.PowerplantPlans)
                {
                    Assert.IsNotNull(plant.P);
                    Assert.IsNotNull(plant.Name);
                }
            }
        }
    }
}