# Introduction 

WeatherForecastService is a net7.0 Minimal REST API that supports the following operations:

- Store longitude and latitude coordinates
- Retreive a Weather Forecast for a given set of longitude and latitude coordinates
- List previously stored longitude and latitude coordinates
- Remove stored longitude and latitude coordinates

# Getting Started

 ### Software dependencies

 - [Docker](https://www.docker.com/get-started/)
 - [.Net 7.0](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)

## Build and Test

### From cmd line
- `git clone https://github.com/PavoGrande/WeatherForecastService.git`
- `cd .\WeatherForecastService`
- `dotnet build .\src`
- `dotnet test .\src`

## Run the API

### From cmd line:

- `docker compose up -d`
- `dotnet run --project .\src\WeatherForecast.Api\WeatherForecast.Api.csproj`

## Interact with the API using Postman

- Import `WeatherForecastService.postman_collection` from the root of the repo to interact with the API from Postman

## Run the tests from IDE:

- Integration test will automatically start and stop PostgreSQL container using docker compose


## Debug the API from IDE:

- Remember to `docker compose up -d` from the root of the repo before debugging the API in the IDE
- Remember to `docker compose down -v` when finished to stop PostgreSQL container

### Swagger API Refernce

- https://localhost:5001/swagger/index.html
