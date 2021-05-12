using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;


namespace ApiPatternsClient
{
    class Program
    {
        private static IConfigurationRoot Configuration;

        static void Main(string[] args)
        {
            BootstrapConfiguration();
            
            // Obtain Connection String from appsettings.json or user-secrets (secrets.json)
            var blobConnString = Configuration["SecretStrings:BlobConnectionString"]; 
            var ehConnString = Configuration["SecretStrings:EhConnectionString"];
            var mongoConnString = Configuration["SecretStrings:MongoConnectionString"];
            var cosmosConnString = Configuration["SecretStrings:SqlCosmosConnectionString"];


            //Upload a File
            Console.WriteLine("Sending ./ExampleFiles/GitMsftLogo.png to Blob Storage");
            Console.WriteLine(BlobHandler.uploadFile(blobConnString));
            Console.WriteLine("\n");

            //Download a File
            Console.WriteLine("Downloading what we just uploaded");
            Console.WriteLine(BlobHandler.downloadFile(blobConnString));
            Console.WriteLine("\n");

            //Add Something to an Event Hub Queue
            Console.WriteLine("Sending 3 messages to EventHub");
            //Calling an async method requires we get an awaiter inside of main() if we want the results
            string produceResult = EventHubHandler.produceEvents(ehConnString).GetAwaiter().GetResult();
            Console.WriteLine(produceResult);
            Console.WriteLine("\n");

            //Read Something from an Event Hub Queue
            Console.WriteLine("Retrieve  messages to EventHub");
            //Calling an async method requires we get an awaiter inside of main() if we want the results
            string processResult = EventHubHandler.processEvents(ehConnString, blobConnString).GetAwaiter().GetResult();
            Console.WriteLine(processResult);
            Console.WriteLine("\n");

            //Write something to Cosmos
            Console.WriteLine("Writing Something to Cosmos-Mongo");
            Console.WriteLine(CosmosMongoHandler.insertData(mongoConnString));
            Console.WriteLine("\n");

            //Read something from Cosmos
            Console.WriteLine("Reading Something from Cosmos-Mongo");
            Console.WriteLine(CosmosMongoHandler.selectData(mongoConnString));
            Console.WriteLine("\n");

            //Write something to Cosmos SQL
            Console.WriteLine("Writing Something to Cosmos-SQL");
            string insertCosmosResult = CosmosSqlHandler.insertData(cosmosConnString).GetAwaiter().GetResult();
            Console.WriteLine(insertCosmosResult);
            Console.WriteLine("\n");

            //Read something from Cosmos SQL
            Console.WriteLine("Reading Something from Cosmos-SQL");
            string selectCosmosResult = CosmosSqlHandler.selectData(cosmosConnString).GetAwaiter().GetResult();
            Console.WriteLine(selectCosmosResult);
            Console.WriteLine("\n");
        }

        // ===============================================================================================
        // This Boot-strap section is needed for Local Development Settings
        // Used to protect secrets on local machines not checked into code, like connection strings.
        // https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-5.0
        // ===============================================================================================
        private static void BootstrapConfiguration()
        {
            string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var isDevelopment = string.IsNullOrEmpty(env) ||  env.ToLower() == "development";

            var builder = new ConfigurationBuilder();

            //set the appsettings file for general configuration (stuff that's not secret)
            builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            //if development, load user secrets as well
            if (isDevelopment)
            {
                builder.AddUserSecrets<Program>();
            }

            Configuration = builder.Build();
            return;
            // ===============================================================
        }
    }
}
