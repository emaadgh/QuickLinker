# QuickLinker: URL Shortening API
<img src="https://github.com/emaadgh/QuickLinker/assets/10380342/5c3b3f6f-8207-4e27-8124-0924675082cd" width="150" height="150">

QuickLinker.API is a backend service built using ASP.NET Core, designed to simplify URL management by providing API endpoints for generating and resolving shortened URLs.

## Features

- **URL Shortening**: Generate short links for lengthy web addresses.
- **Link Resolution**: Resolve shortened URLs to their original destinations.
- **Caching**: Utilize distributed caching for improved performance.
- **API Endpoints**: Exposes RESTful API endpoints for interaction.
- **CI/CD Pipeline**: Automate the build, test, and deployment processes for the QuickLinker project.
- **Dockerizing**: Containerize the QuickLinker API using DockerFile and Docker Compose for simplified deployment and management.

## Getting Started

1. **Clone the Repository**:
    ```bash
    git clone https://github.com/emaadgh/quicklinker.git
    ```

2. **Install Dependencies**:
    - Ensure you have **.NET SDK 8** installed.
    - Open a terminal and navigate to the project folder:
        ```bash
        cd quicklinker
        ```
    - Restore dependencies:
        ```bash
        dotnet restore
        ```

    **QuickLinker.API Dependencies**:
    - Microsoft.AspNetCore.Mvc.NewtonsoftJson
    - Microsoft.EntityFrameworkCore.SqlServer
    - Microsoft.EntityFrameworkCore.Tools
    - Microsoft.Extensions.Caching.StackExchangeRedis
    - Swashbuckle.AspNetCore

    **QuickLinker.Test Dependencies**:
    - coverlet.collector
    - Microsoft.NET.Test.Sdk
    - Moq
    - xunit
    - xunit.runner.visualstudio

3. **Run the Application**:
    - Start the API by running the following command in your terminal:
        ```bash
        dotnet run
        ```
4. **Database Migration**:
    - After building the project, run the following command to create or update the database based on migration files:
        ```bash
        dotnet ef database update
        ```
        If you don't have the Entity Framework Core tools (`dotnet ef`) installed globally, you can install them by running the following command:
        ```bash
        dotnet tool install --global dotnet-ef
        ```

5. **Access the API**:
    - By default, the API will be hosted on `localhost` with a randomly assigned port. You can access the API using the following URL format:
        ```
        https://localhost:<PORT>/api
        ```
    - Replace `<PORT>` with the port number assigned to the API during startup.

6. **Explore the API**:
    - Once the API is running, you can use tools like **Swagger** or **Postman** to interact with the endpoints.
    - Visit the Swagger UI at `https://localhost:<PORT>/swagger/index.html` to explore the API documentation interactively.

## Exploring the API with Postman

1. **Import Postman Collection**:
    - Locate the `QuickLinker.postman_collection.json` file in the project repository.
    - Import this collection into Postman using the following steps:
        - Open Postman.
        - Click on the "Import" button in the top left corner.
        - Select the option to "Import From File" and choose the `QuickLinker.postman_collection.json` file.
        - Click "Import" to add the collection to your Postman workspace.

2. **Set Base URL**:
    - Once the collection is imported, navigate to the collection settings in Postman.
    - In the "Variables" section, locate the variable named `base_url`.
    - Set the value of this variable to the base URL of your API. You can specify the URL and port here, e.g., `https://localhost:5001`.
    - This base URL will be automatically used for all requests within the collection.

3. **Explore Endpoints**:
    - Now you can explore and interact with the API endpoints conveniently using the imported Postman collection.
    - Each request in the collection will utilize the base URL configured in the variables, making it easy to test different endpoints with your local setup
      
4. **Written Tests**:
    - The Postman collection includes written tests for various requests to validate their responses.
    - These tests ensure that the responses meet expected criteria, such as status codes, data types, and content format.
    - You can run these tests within Postman to verify the correctness of API responses.

## AppSettings.json Configuration

Please note that `appsettings.json` files are not included in the Git repository. These files typically contain sensitive information such as database connection strings, API keys, and other configuration settings specific to the environment.

For local testing, you can create your own `appsettings.json` file in the root directory of the QuickLinker project and add the necessary configurations. Make sure to include the `QuickLinkerDbContextConnection` connection string for the SQL Server database and the `QuickLinkerRedisConnection` connection string for the Redis distributed cache if you're using one locally.

Additionally, you need to add a configuration for `QuickLinkerDomain:Domain` to your `appsettings.json` file. This configuration is required to specify the base URL to which the generated short code will be appended, resulting in the shortened URL.

Here's an example of how you can add the `QuickLinkerDomain:Domain` configuration:

```json
{
  "QuickLinkerDomain": {
    "Domain": "https://example.com/"
  },
  "ConnectionStrings": {
    "QuickLinkerDbContextConnection": "your-sql-server-connection-string",
    "QuickLinkerRedisConnection": "your-redis-connection-string"
  },
  // Other configurations...
}
```
If you deploy the QuickLinker API to Azure App Service, you can add the connection strings in the Connection Strings section of the Configuration settings in the Azure portal. Azure App Service securely encrypts connection strings at rest and transmits them over an encrypted channel, ensuring the security of your sensitive information.

## Usage

1. Send POST requests to `/api/ShortLink` with a JSON payload containing the original URL to generate a shortened link.
2. Receive the shortened URL in the response.
3. Use the shortened URL for sharing or accessing the original content.
4. Send GET requests to `/{shortCode}` with the short code to resolve and redirect to the original URL.

## CI/CD Pipeline

This repository includes a CI/CD pipeline that automates the build, test, and deployment processes for the QuickLinker project.

### Pipeline Configuration

The repository contains a configuration file named `azure-pipeline.yml` which defines the CI/CD pipeline. This file can be used with Microsoft Azure DevOps's Pipeline service.

The pipeline configuration orchestrates the following tasks:
- Dependency restoration
- Building the project
- Running tests
- Publishing artifacts for deployment

### Usage

The CI/CD pipeline ensures that changes to the project are automatically validated, built, and prepared for deployment, streamlining the development process and maintaining code quality.

In addition, we utilize Azure DevOps Pipeline Releases to continuously deploy artifacts from the CI pipeline to an Azure app service in the cloud. This allows for seamless and automated deployment of updates to the application environment.

<img src="https://github.com/emaadgh/mybook/assets/10380342/d503ac6a-b6b7-46f7-9024-cd05444577ce" width="585" height="333.5">

Users can leverage this pipeline configuration in Microsoft Azure DevOps's Pipeline service to automate the software delivery process.

## Dockerizing the API with Dockerfile and Docker Compose

The QuickLinker API can be easily containerized using DockerFile and Docker Compose to orchestrate multiple containers. Docker provides a consistent environment across different systems, making it easier to deploy and manage applications.

To Dockerize the QuickLinker API and run it with Docker Compose, follow these steps:

1. Ensure you have Docker installed on your system.
2. Navigate to the root directory of the QuickLinker project.
3. Create a `.env` file beside the `docker-compose.yml` file.
4. Add the following environment variable to the `.env` file:
```
SA_PASSWORD=preferedpassword
```
Replace `preferedpassword` with your preferred SQL Server SA password.

5. Don't forget to update connection strings `QuickLinkerDbContextConnection` and `QuickLinkerRedisConnection` in the app's configuration file like `appsettings.json` before running the QuickLinker API in a Docker container.

6. Run the following command in the terminal to start the containers:
```
docker-compose up
```
This will build the QuickLinker API Docker image and start the containers for the API, the SQL Server database, and a Redis cache instance.

With Docker and Docker Compose, you can easily deploy and manage the QuickLinker API in a containerized environment, ensuring consistency and scalability across different systems.

## Testing

Unit tests are provided in the `QuickLinker.Test` project. These tests cover various scenarios to ensure the functionality of the API endpoints.

## Database Reset for Testing Purposes

For testing purposes, you can add the following code snippet to your `Program.cs` file. This code will reset the database and its data every time the application is run:

```
using (var scope = app.Services.CreateScope())
{
    try
    {
        var context = scope.ServiceProvider.GetService<QuickLinkerDbContext>();
        if (context != null)
        {
            await context.Database.EnsureDeletedAsync();
            await context.Database.MigrateAsync();
        }
    }
    catch (Exception ex)
    {
        // Handle any exceptions if necessary
    }
}
```
Make sure to replace `QuickLinkerDbContext` with your actual DbContext class if it's named differently.

## Creator

- [Emaad Ghorbani](https://github.com/emaadgh)

## Contributing

Contributions are welcome! If you'd like to enhance QuickLinker, feel free to submit pull requests or open issues.

## License

This project is licensed under the MIT License.
