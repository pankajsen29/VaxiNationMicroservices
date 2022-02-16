#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#begins with the ASP.NET image (includes runtime) and create an intermediate image base.
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

#build stage starts from a different original image 'sdk', rather than continuing from base.The sdk image has all the build tools and will be used for compiling the project.
#this image will be temporary and it is made only to compile our application (this is one main benefit of Docker multi-stage build).
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
#means the following COPY and RUN instructions will be executed in /src directory. If it doesn’t exist it will be created (even if it wouldn’t be used).
WORKDIR /src

#below lines can be skipped which are basically used to restore nuget dependencies to simplify, because restore is included in 'dotnet publish' cmd which is done next.
#this copies a file, and it is copied in the path specified inside the image.
#[In case of file copy, source and destination path (absolute/relative) has to be specified]
#[the host folder strucure is not preserved when COPY is used to copy a file]
#---------
COPY ./VaccineInfo.API/*.csproj VaccineInfo.API/
COPY ./VaccineInfo.Core/*.csproj VaccineInfo.Core/
COPY ./VaccineInfo.Infrastructure/*.csproj VaccineInfo.Infrastructure/
RUN dotnet restore VaccineInfo.API/VaccineInfo.Api.csproj
RUN dotnet restore VaccineInfo.Core/VaccineInfo.Core.csproj
RUN dotnet restore VaccineInfo.Infrastructure/VaccineInfo.Infrastructure.csproj
#---------

#this means, all files from Docker build context will be copied (with existing folder structure) to workdir (/src) directory inside Docker image.
#build context is the directory from host machine, pointed in Docker build command. If you point . path, the directory where you execute the command is taken. 
#this copies directory including all files, i.e. existing host folder structure is preserved inside the image.
COPY . ./

#this compiles the project (i.e. VaccineInfo.API, this is basically the folder where .csproj to be compiled is present) with release configuration.
#and publishes results in VaccineInfo.API/publish directory (in our case /src/VaccineInfo.API/publish).
#(hint: If a relative path is specified when publishing a project, the output directory generated is relative to the current working directory, not to the project file location.)
RUN dotnet publish VaccineInfo.API -c Release -o VaccineInfo.API/publish


#The final stage starts again from base which has the runtime needed for running the application in production environment.
#(hint: this time we are preparing our real image - the one which will be deployed to production, 
#and this one doesn’t base on SDK (doesn't include the build tools), only on production runtime, which gives smaller size in result.  
FROM base AS final
#means the following COPY and ENTRYPOINT instructions will be executed in /app directory.
WORKDIR /app
#means from temporary image, the compiled files (binaries) will be copied to current workdir (/app) directory inside Docker image
COPY --from=build /src/VaccineInfo.API/publish .
#this command will be executed when the container starts and will run dotnet with PROJECT_NAME.dll parameter inside /app folder
ENTRYPOINT ["dotnet", "VaccineInfo.Api.dll"]