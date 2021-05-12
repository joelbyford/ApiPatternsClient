using System; 
using Microsoft.Azure.Cosmos;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace ApiPatternsClient
{

    // Started from https://docs.microsoft.com/en-us/dotnet/api/overview/azure/cosmosdb
    // Examples pulled from: https://docs.microsoft.com/en-us/azure/cosmos-db/create-sql-api-dotnet#code-examples
    public class CosmosSqlHandler
    {
        public static async Task<string> insertData(string cosConnString)
        {
            // Using temporary CosmosClient here as an example.  In real code this will be created ONCE for the entire class 
            // (and reused in all functions).
            CosmosClient cosmosClient = new CosmosClient(cosConnString);
            string dbName = "sample-db";
            string collectionName = "sample-collection";
            
            Container container = cosmosClient.GetContainer(dbName, collectionName);
            //make a sample to add
            SampleSqlData sample = new SampleSqlData("TestSqlName", DateTime.Now);

            try
            {
                // Create an item in the container. Note we provide the value of the partition key for this item, which is "TestSqlName".
                ItemResponse<SampleSqlData> sampleResponse = await container.CreateItemAsync<SampleSqlData>(sample, new PartitionKey(sample.Name));
                // Note that after creating the item, we can access the body of the item with the Resource property of the ItemResponse. We can also access the RequestCharge property to see the amount of RUs consumed on this request.
                return "Created item in database with id: " + sampleResponse.Resource.id + " Operation consumed " + sampleResponse.RequestCharge + " RUs.\n";
            }
            catch (CosmosException ex)
            {
                return ex.Message;               
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static async Task<string> selectData(string cosConnString)
        {
            var result = "";
            // Using temporary CosmosClient here as an example.  In real code this will be created ONCE for the entire class 
            // (and reused in all functions).  
            CosmosClient cosmosClient = new CosmosClient(cosConnString);
            string dbName = "sample-db";
            string collectionName = "sample-collection";
            
            Container container = cosmosClient.GetContainer(dbName, collectionName);

            var sqlQueryText = "SELECT * FROM c WHERE c.Name = 'TestSqlName'";

            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            FeedIterator<SampleSqlData> queryResultSetIterator = container.GetItemQueryIterator<SampleSqlData>(queryDefinition);

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<SampleSqlData> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (SampleSqlData data in currentResultSet)
                {
                    result += data + "\n";
                }
            }

            return result;
        }
    }
}
