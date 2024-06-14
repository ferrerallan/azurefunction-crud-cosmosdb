
# Azure Function CRUD with CosmosDB Example

## Description

This repository provides an example of using Azure Functions with CosmosDB to create a CRUD (Create, Read, Update, Delete) application. It demonstrates how to set up and use Azure Functions for serverless computing and CosmosDB for a NoSQL database in a Node.js application, which is useful for developers looking to build scalable, event-driven applications.

## Requirements

- Node.js
- Azure Account with CosmosDB and Azure Functions setup
- Azure Functions Core Tools
- Azure CLI
- Yarn or npm for package management

## Mode of Use

1. Clone the repository:
   ```bash
   git clone https://github.com/ferrerallan/azurefunction-crud-cosmosdb.git
   ```
2. Navigate to the project directory:
   ```bash
   cd azurefunction-crud-cosmosdb
   ```
3. Install the dependencies:
   ```bash
   yarn install
   ```
4. Ensure you have an Azure account with CosmosDB and Azure Functions setup.
5. Install Azure Functions Core Tools and Azure CLI.
6. Deploy the Azure Function:
   ```bash
   func azure functionapp publish <FunctionAppName>
   ```

## Implementation Details

- **functions/**: Contains the Azure Functions code for CRUD operations.
- **package.json**: Configuration file for the Node.js project, including dependencies.
- **local.settings.json**: Configuration file for local development settings.

### Example of Use

Here is an example of how to create an item in CosmosDB using an Azure Function:

```javascript
const {{ CosmosClient }} = require("@azure/cosmos");

const endpoint = process.env.COSMOS_DB_ENDPOINT;
const key = process.env.COSMOS_DB_KEY;
const databaseId = process.env.COSMOS_DB_DATABASE_ID;
const containerId = process.env.COSMOS_DB_CONTAINER_ID;

const client = new CosmosClient({{ endpoint, key }});

module.exports = async function (context, req) {{
  const item = req.body;

  try {{
    const {{ resource: createdItem }} = await client
      .database(databaseId)
      .container(containerId)
      .items.create(item);

    context.res = {{
      status: 201,
      body: createdItem
    }};
  }} catch (error) {{
    context.res = {{
      status: 500,
      body: `Error creating item: ${{error.message}}`
    }};
  }}
}};
```

This code initializes a CosmosDB client, connects to a specified database and container, and creates a new item based on the request body.

## License

This project is licensed under the MIT License.

You can access the repository [here](https://github.com/ferrerallan/azurefunction-crud-cosmosdb).
