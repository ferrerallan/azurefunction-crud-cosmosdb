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
    public class ListProgressiveGuitars { 

        private CosmosClient cosmosClient;
        public ListProgressiveGuitars(CosmosClient cosmosClient)
        {
            this.cosmosClient = cosmosClient;
        }
  
        [FunctionName("list-progressive-guitars")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {

            var container = this.cosmosClient.GetContainer(Environment.GetEnvironmentVariable("DataBase"), "Guitars");

            var sqlQueryText = "SELECT * FROM Guitars where Guitars.NumberOfStrings = udf.ProgressiveGuitars()";

            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            FeedIterator<Guitar> queryResultSetIterator = container.GetItemQueryIterator<Guitar>(queryDefinition);

            List<Guitar> mylist = new List<Guitar>();

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<Guitar> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (Guitar x in currentResultSet)
                {
                    mylist.Add(x);
                    log.LogInformation(x.GetInfo());
                }
            }

            return new OkObjectResult(mylist);
        }
    }
}
