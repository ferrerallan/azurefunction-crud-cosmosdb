using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos;
using AzureFunctionCosmosDB.Models;
using System.Collections.Generic;

namespace AzureFunctionCosmosDB.Functions
{
    public class CreateGuitar { 

        private CosmosClient cosmosClient;
        public CreateGuitar(CosmosClient cosmosClient)
        {
            this.cosmosClient = cosmosClient;
        }
  
        [FunctionName("create-guitar")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Guitar data = JsonConvert.DeserializeObject<Guitar>(requestBody);
            if (data == null)
            {
                return new BadRequestObjectResult($"Cannot parse body.");
            }

            var container = this.cosmosClient.GetContainer(Environment.GetEnvironmentVariable("DataBase"), "Guitars");

            try
            {
                var result = await container.CreateItemAsync<Guitar>(data, new PartitionKey(data.id));
                return new OkObjectResult(result.Resource.id);
            }
            catch (CosmosException cosmosException)
            {
                log.LogError("Creating item failed with error {0}", cosmosException.ToString());
                return new BadRequestObjectResult($"Failed to create item. Cosmos Status Code {cosmosException.StatusCode}, Sub Status Code {cosmosException.SubStatusCode}: {cosmosException.Message}.");
            }

            return new OkObjectResult("");
        }
    }
}
