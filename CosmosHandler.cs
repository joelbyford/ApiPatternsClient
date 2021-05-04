using System; 
using MongoDB.Driver;
using MongoDB.Bson;

namespace ApiPatternsClient
{

    // Examples primarily from: https://docs.microsoft.com/en-us/azure/cosmos-db/create-mongodb-dotnet
    class CosmosHandler
    {
        public static string insertData(string mongoConnString)
        {
            // Create a connection to the mongo with the right connection string
            var client = new MongoClient(mongoConnString);
            string dbName = "sample-db";
            string collectionName = "sample-data";

            var db = client.GetDatabase(dbName);
            var sampleCollection = db.GetCollection<SampleData>(collectionName);

            //make a sample to add
            SampleData sample = new SampleData("TestName", DateTime.Now);

            try
            {
                sampleCollection.InsertOne(sample);
                return "Successfully Added Item";
            }
            catch (MongoCommandException ex)
            {
                return ex.Message;
            }
            catch (System.Exception ex)
            {
                return ex.Message;
            }

        }

        public static string selectData(string mongoConnString)
        {
            // Create a connection to the mongo with the right connection string
            var client = new MongoClient(mongoConnString);
            string dbName = "sample-db";
            string collectionName = "sample-data";
            string result = "";

            var db = client.GetDatabase(dbName);
            var sampleCollection = db.GetCollection<SampleData>(collectionName);

            var sampleDataCollection = sampleCollection.Find(new BsonDocument()).ToList();

            foreach(SampleData data in sampleDataCollection)
            {
                result += data.ToJson() + "\n";
            }

            return result;
        }
    }
}