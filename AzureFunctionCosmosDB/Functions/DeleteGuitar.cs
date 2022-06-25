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
    public class DeleteGuitar
    { 

        private CosmosClient cosmosClient;
        public DeleteGuitar(CosmosClient cosmosClient)
        {
            this.cosmosClient = cosmosClient;
        }
  
        [FunctionName("delete-guitar")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "delete-guitar/{id}")] HttpRequest req,
            ILogger log, string id)
        {
            var container = this.cosmosClient.GetContainer(Environment.GetEnvironmentVariable("DataBase"), "Guitars");
            
            try
            {
                var result = await container.DeleteItemAsync<Guitar>(id, new PartitionKey(id));
                return new OkObjectResult("deleted with success");
            }
            catch (CosmosException cosmosException)
            {
                log.LogError("Creating item failed with error {0}", cosmosException.ToString());
                return new BadRequestObjectResult($"Failed to create item. Cosmos Status Code {cosmosException.StatusCode}, Sub Status Code {cosmosException.SubStatusCode}: {cosmosException.Message}.");
            }

        }
    }
}
