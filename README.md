# 						**Basic Description of the Microservice solution **

*** Disclaimer: This solution is made solely as part of my studies/practices. The examples data used here are all fictitious and don't carry any factual correctness, these are used only for demonstration purposes. ***

This solution consists of many different basic microservices, all are built following the Clean Architecture and DDD (Domain Driven Design) pattern, which use/demonstrate many different concepts (or building blocks) of ASP.NET Core REST APIs.

I have chosen an apt use case related to the ongoing pandemic, the “mass vaccination program” to demonstrate the fundamentals. Basically, my solution consists of few possible APIs to run a mass vaccination program (by a small organization/institute or a State for example) in an organized way, may not be that efficient though. Also, the client apps are not part of this solution. It shows how the backend can be handled and if suitable, then the client apps (web app, mobile app etc.) can be created to consume these backend services to efficiently manage the vaccination program.  

**All projects are built on ASP.NET 6 minimal hosting model using Visual Studio 2022.**

### **Overall architecture of the demonstrated microservice solution**:

_TO BE UPDATED: will show how the API gateway routes the request to API, how the APIs talk to each other_


### **Basic layering of each API/microservice**:

_(Clean Architecture and Dependency Inversion Principle)_
[Ref: https://www.youtube.com/watch?v=lkmvnjypENw]
![image](https://user-images.githubusercontent.com/85120101/154347337-ef651750-b181-47b3-be56-72365c880026.png)

**UI/API Layer**: _This is the client facing UI project or an API project (what UI directly consumes) which aggregates both domain and infrastructure layers._

**Domain/Business/Core Layer**: _this is an independent business logic layer, meaning doesn’t have dependencies to any of the other layers in the microservice. It just defines the contracts which are implemented by other layers (e.g., infrastructure layer). The main advantage of this kind of architecture is, any change in the database would need only the infrastructure layer to be adapted, and not the business layer._

**Infrastructure/Data Access layer**: _It implements the database access logic. And can be adapted anytime in case there are changes required in the backend DB (e.g., table/column change or the change of entire DB itself.)_

###                                                **API/microservice:  VaccineInfoService:**

**API Project Name**: VaccineInfo.Api

**API Project Description**: Web API project with basic APIs for managing various types of vaccine information.

**This project Demonstrates the below concepts/building blocks**:

**C# record type**

**Data annotations**

**Dependency Injection**:

_(using “ConfigureDependencyInjection” extension method)
(Below lifetimes are used for registering the services under DI container:
Singleton: the class is instantiated once for the whole application.
Transient: the class is instantiated once per service resolution, means the class is instantiated every time a link (which uses the class/service) is loaded/refreshed.
Scoped: the class is instantiated once per scope, meaning the instantiation happens only once for a request/ a single connection. Think of it like a browser tab. This is useful because this actually can be treated as current user’s scope.)_

**DTO – data transfer objects**:

_(not to expose the models/entities to external world)
this is to make an independent contract with the client, so that it doesn’t get affected when there is any addition/deletion to the internal entity/data store._

**Automapper**: 

_we need a mapper to map the DTO object to the actual model in the PATCH action method, to be able to accept the partial changes in the form DTO and applying them to the actual model entity. 
(Nuget: AutoMapper.Extensions.Microsoft.DependencyInjection)_


**Connecting to mongodb**:

(Nuget: MongoDB.Driver)

(Assumptions: there is a user created named:
user: "mongodbadmin",
pwd: “Pass#word1”,
with ‘readwrite’ access)


**CRUD operations**:

_(Http Verbs: GET, POST, PUT, PATCH, DELETE etc.) – async calls

(Swagger can be used to test these operations. Also, the complete URLs are shown below which are usable to test these using POSTMAN)_

GET (API version v1): https://localhost:7028/api/v1/vaccines


GET (API version v2): https://localhost:7028/api/v2/vaccines


GET: https://localhost:7028/api/v1/vaccines/298e980c-fdb6-4df5-a11c-2a8dfe62094b


POST: https://localhost:7028/api/v1/vaccines

_(Postman: Verb-> POST, Body: raw: JSON to set the values in JSON as below)_

{
  "name": "covaxin",
  "maxPrice": 1000,
  "numberOfDoses": 2,
  "minDaysBetweenDoses": 28,
  "manufacturerName": "bharath biotech",
  "manufacturerWebsite": "https://www.bharatbiotech.com/",
  "localApprovalDate": "2021-02-15T19:48:56.420Z",
  "approvedBy": [
    "ICMR","WHO"
  ]
}


PUT: https://localhost:7028/api/v1/vaccines/c3bb5c84-d582-45a5-b49f-48807d22c537

_(Postman: Verb-> POST, Body: raw: JSON to set the values in JSON as below)_

{
  "name": "sputnikV",
  "maxPrice": 900,
  "numberOfDoses": 1,
  "minDaysBetweenDoses": 0,
  "manufacturerWebsite": "https://sputnikv.com",
  "approvedBy": [
    "WHO"
  ]
}


PATCH: https://localhost:7028/api/v1/vaccines/8175e9b1-6d66-4783-8dd3-d158f9ca52ad

_(Postman: Verb-> POST, Body: raw: JSON to set the values)_

[
  {
    "value": "covaxin single",
    "path": "/name",
    "op": "replace",
    "from": "string"
  }
]

_(PATCH is useful for incremental/partial update, it uses JsonPatchDocument. The code shows how it is used to accept the data in DTO and gets mapped to the actual database entity with the help of AutoMapper)_

_(Nuget_: 
_Microsoft.AspNetCore.JsonPatch,
Microsoft.AspNetCore.Mvc.NewtonsoftJson, 
Swashbuckle.AspNetCore.Newtonsoft)_



DELETE: https://localhost:7028/api/v1/vaccines/23b99c6e-8d98-4a8e-8a90-88aac6ad5236


DB Screenshot:
![image](https://user-images.githubusercontent.com/85120101/154348170-5166934d-b491-441c-8261-0004e61e7e76.png)


**Secrets management using .NET Secret Manager**:

_(Hint - In Visual Studio -> Solution Explorer -> Project right click -> ‘Manage User Secrets’)
Notice “MongoDbSettings” section in appsettings.json and the class to read these configurations in “Settings” folder. The “Password” is not added as plain text in the appsettings.json file, instead it is stored using .NET Secret Manager._


**Global Exception handling**:

Exception is handled globally using custom exception handling middleware in the API layer (Middlewares -> ExceptionHandlingMiddleware.cs), which rules out the need of handling exception in each and every method with a Try-Catch block. 
Notice how the exceptions thrown by deeper layers (i.e., core layer or infrastructure layer exceptions which are defined and thrown from the “Core” layer or Infrastructure layer respectively) are caught, logged and communicated to the UI/client.
Also, a custom type (Exceptions->ApiError) is used to return the error to the client in JSON format.



**Logging**:


***Serilog***: 

_Used Serilog for the logging. It is chosen for these benefits specially: 
1.It supports variety of log targets, as sinks.
2.It has an additional check to avoid unnecessary memory allocation for the logging parameters being passed, in case the log message doesn’t actually get written to log target due to different configured log level. 
3.It can serialize the object passed in using its default formatter instead of calling ToString() for each value.
4.It has variety of enrichers, which are useful to add additional information to the logs for better debugging.
5.The integration is very easy with existing application too, and default Asp.net logger can work very well with it._

_Nuget: Serilog.AspNetCore
(WriteTo.Console and WriteTo.File are built-in in this package)_


***Serilog Enrichers***:

_Used below enrichers for adding additional information to the log messages:
Nugets:
Serilog.Enrichers.CorrelationId  (it allows us to group actions together with an Id, useful to identify the log of a particular API call, when an API is called twice at the same time. Hint: to access the HttpContext of the request add this: services.AddHttpContextAccessor();)
Serilog.Enrichers.Environment  (logs the machine name, useful when the API is running in multiple servers.)_


***Log targets/sinks***:

_Console,
File (in two different files, one with normal string log and other one in JSON format),
Serilog.Sinks.Seq (It is configured to be able to see the log messages in a structured way externally using browser. It’s easy to query and filter those logs in Seq Browser URL.)_


***Logging the timed operations***:

_It helps in logging the time taken for a particular operation, e.g. execution of a database query.
Nuget: SerilogTimings_

_All of the above are configured in appsetiings.json for better handling and maintenance._


**Health Checks**:

_The service has a health check API endpoint (e.g., HTTP /health) that returns the status of service (Healthy/Unhealthy/Degraded).
Extensions -> HealthCheckConfiguration.cs_

_Nuget_: _AspNetCore.HealthChecks.MongoDb
This is added to check the health of backend mongo db._

_Multiple endpoints are added to see if the service is UP and ready for accepting requests.

***api/health/ready*** => this endpoint tells if the service is ready to receive requests_

(Complete URLs: https://localhost:7028/api/health/ready)

_***api/health/live*** => is the service alive?_

(Complete URLs: https://localhost:7028/api/health/live)


**API Versioning**:

_It is the effective communication around the changes to the API, so the consumers know what to expect from it. It is a best practice for managing/maintaining the API in a transparent way._

_Nuget_: 
_Microsoft.AspNetCore.Mvc.Versioning
Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer
Note: Both versions of the controllers are having the same API definitions though, because the main intention is to demonstrate how the versioning works. The implementation of the API including the core domain layer can be changed as needed._

_Notice the below attributes on the controllers in both V1 and V2 folders, to see how versioning is applied to the controller_:

[Route("api/v{version:apiVersion}/[controller]")]

[ApiVersion("1.0")]

_And also, the other challenge was to enable swagger with versioning_:
_Notice the changes in Extensions->SwaggerConfiguration.cs where versioning is made enabled and defined for the controller.
ConfigureSwaggerOptions is the configuration option class which is responsible to create the JSON documents what swagger uses to create the UI._

_And in the other overloaded ConfigureSwagger() method after the service is built, it is shown how the middleware is configured to create the endpoints for swagger for each version._


**Configuring different profiles in Properties** : launchSettings.json

_e.g. 
"VaccineInfo.Api.Dev"
"VaccineInfo.Api.Prod"_


**Creation of Docker image**:

_The below article describes nicely the docker image building process for a multi-project solution: (as our API is divided into different layers (projects) following Clean Architecture)_
https://www.softwaredeveloper.blog/multi-project-dotnet-core-solution-in-docker-image


***docker build command***:

_Below shown the special way of building docker image for a project with dependencies (other projects)_:

_In our case, the “docker build” is executed from “VaxiNationMicroservices\VaccineInfoService\src" path_:

docker build -f VaccineInfo.API/Dockerfile -t vaccineinfoapiimage:v1 . 

Here the build context is passed as "." , which is basically the directory ("VaxiNationMicroservices\VaccineInfoService\src") where we run this command.
(i.e., where all the projects (main project and its dependencies) are present)
Keep the "Dockerfile" inside the main project, which actually helps in having more than one Dockerfile in solution (for different projects) and then specify which "Dockerfile" is to be read with --file (-f) option.

 

***Where to keep .dockerignore file***:

_Docker CLI looks for .dockerignore file in root directory of the build context, so we move it to solution directory. It’s also better, because we don’t need to create and maintain many .dockerignore files for many projects, we keep one for all of them._


***Steps to use mongodb container along with our API container***:

1.create a network:

docker network create vaccineapiservicenetwork

2.Then run the mongo db container along with the network name:
  
docker run -d --rm --name mongo -p 27017:27017 -v mongodbdata:/data/db -e MONGO_INITDB_ROOT_USERNAME=mongodbadmin -e MONGO_INITDB_ROOT_PASSWORD=Pass#word1 --network=vaccineapiservicenetwork mongo

3.Now run the container of the application: (notice how the MongoDbSettings are passed)

docker run -it --rm -p 8080:80 -e MongoDbSettings:Host=mongo -e MongoDbSettings:Password=Pass#word1 --network=vaccineapiservicenetwork vaccineinfoapiimage:v1

4.Then test the API:

GET: http://localhost:8080/api/v1/vaccines

(Hint: It should list nothing, as the db is empty)

POST: http://localhost:8080/api/v1/vaccines

(Postman: Verb-> POST, Body: raw: JSON to set the values in JSON as below)

{
  "name": "covaxin",
  "maxPrice": 1000,
  "numberOfDoses": 2,
  "minDaysBetweenDoses": 28,
  "manufacturerName": "bharath biotech",
  "manufacturerWebsite": "https://www.bharatbiotech.com/",
  "localApprovalDate": "2021-02-15T19:48:56.420Z",
  "approvedBy": [
    "ICMR","WHO"
  ]
}

GET: http://localhost:8080/api/v1/vaccines/296e0d30-388b-44f0-a474-c868b6c8d99a

Response:
{
    "id": "296e0d30-388b-44f0-a474-c868b6c8d99a",
    "name": "covaxin",
    "maxPrice": 1000.0,
    "numberOfDoses": 2,
    "minDaysBetweenDoses": 28,
    "manufacturerName": "bharath biotech",
    "manufacturerWebsite": "https://www.bharatbiotech.com/",
    "localApprovalDate": "2021-02-15T19:48:56.42+00:00",
    "approvedBy": [
        "ICMR",
        "WHO"
    ],
    "createdDate": "2022-02-16T18:54:23.1830803+00:00"
}

5.Press Ctrl+C to stop the docker container.


***Steps to push the API image to Docker hub***:

To share the docker image, we need to push to dockerhub.

1. That’s why, 1st login using:

docker login
Username:
Password:

2. Then re-tag the image as below:

docker tag vaccineinfoapiimage:v1 pankajsen29/vaccineinfoapiimage:v1

3. Then push the docker image to docker hub using:

docker push pankajsen29/vaccineinfoapiimage:v1

4. Delete the local images, logout from docker hub and then run the container for the same image. Then it will get pulled from docker hub and start running.

5. Use below to logout from docker hub:

docker logout


***Get this iamge from Docker Hub***:

This image is pushed to Docker Hub and made available for public access/use, use the below command to pull it to local system:

docker pull pankajsen29/vaccineinfoapiimage


**Domain layer Project Name**: VaccineInfo.Core

API Project Description: This is a Class Library project. This is basically the core domain layer which implements the business logic for the API layer to consume for managing various types of vaccine information. It doesn’t have any dependencies to any other layer of the of API service.


**Database layer Project Name**: VaccineInfo.Infrastructure

API Project Description: This is a Class Library project. This is basically the data access layer which implements the database operations which are called by the API layer indirectly though the Core layer for managing various types of vaccine information in the database.


**Test Project Layer**: VaccineInfo.FunctionalTests

Tests the VaccineInfo.API project.

**Test Project Layer**: VaccineInfo.IntegrationTests

Tests the VaccineInfo.Infrastructure project.

**Test Project Layer**: VaccineInfo.UnitTests

Tests the VaccineInfo.Core project (which doesn’t have any dependencies to other project components)

