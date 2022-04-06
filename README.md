[![Open in Visual Studio Code](https://img.shields.io/badge/VSCode-Open%20in%20VSCode-blue?logo=visualstudiocode)](https://open.vscode.dev/joelbyford/ApiPatternsClient) [![Build on Pull Request](https://github.com/joelbyford/ApiPatternsClient/actions/workflows/BuildOnPullRequest.yml/badge.svg)](https://github.com/joelbyford/ApiPatternsClient/actions/workflows/BuildOnPullRequest.yml)

# ApiPatternsClient
A simple set of example code in dotnet 5.x to show patterns on sending and receiving data on a client with Azure EventHubs, BlobStorage and CosmosDB (Mongo API & SQL API).

## Azure Resources Required
In order to build and use this code, the developer will need the following:
- **Azure Storage Account Created** - [https://docs.microsoft.com/en-us/azure/storage/common/storage-account-create](https://docs.microsoft.com/en-us/azure/storage/common/storage-account-create?toc=%2Fazure%2Fstorage%2Fblobs%2Ftoc.json&tabs=azure-portal)
    - **A Blob Storage Container** - Added to the Storage Acount referenced above and named `sample-container`
- **Azure EventHubs Namespace Created** - https://docs.microsoft.com/en-us/azure/event-hubs/event-hubs-create
    - **An Event Hub** - Added to the Event Hubs referenced above and named `telemetryhub`
- **Azure CosmosDB (MongoAPI) Created** - https://docs.microsoft.com/en-us/azure/cosmos-db/create-mongodb-dotnet
- **Azure CosmosDB (Sql API) Created** - https://docs.microsoft.com/en-us/azure/cosmos-db/create-cosmosdb-resources-portal 
    - **A Database** - Added to the SQL CosmosDB referenced above and named `sample-db`
    - **A Collection (Container)** - Added to `sample-db` and named `sample-collection`
    - **A Partition (Index)** - Added to `sample-collection` for the JSON element `/Name`

## Connection Strings Required
This code leverages the [Configuration](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration?view=dotnet-plat-ext-5.0) nuget package and functionality, which in turn looks at the `appsettings.json` (or `secrets.json`) file for each of the following connection strings.  
- **Blob Storage Connection String** - Stored as `SecretStrings:BlobConnectionString`, this can be found in the [Azure Portal](https://docs.microsoft.com/en-us/azure/storage/common/storage-account-keys-manage?tabs=azure-portal).  Alternatively, this can be also obtained through the [AZ CLI](https://docs.microsoft.com/en-us/cli/azure/storage/account?view=azure-cli-latest#az_storage_account_show_connection_string). 
- **EventHubs Connection String** - Stored as `SecretStrings:EhConnectionString`, this can be found in the [Azure Portal](https://docs.microsoft.com/en-us/azure/event-hubs/event-hubs-get-connection-string). 
- **CosmosMongoDB Connection String** - Stored as `SecretStrings:MongoConnectionString`, this can be found in the [Azure Portal](https://docs.microsoft.com/en-us/azure/cosmos-db/connect-mongodb-account)
- **CosmosSqlDB Connection String** - Stored as `SecretStrings:SqlCosmosConnectionString`, this can be found in the [Azure CLI](https://docs.microsoft.com/en-us/azure/cosmos-db/scripts/cli/common/keys) or also in the Azure Portal under the 'Keys' blade. 

## Important
It is **HIGHLY** recommended that developers use the `dotnet user-secrets` command (e.g. `dotnet user-secrets set SecretStrings:BlobConnectionString "xxxxxx"`) on local machines to cache the two connection strings listed above instead of adding them to the `appsettings.json`.  This practice helps to prevent accidentally checking confidential information into repos and exposing that information inadvertently to peers or the public.  Please see https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets for more information.
