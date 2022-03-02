#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#creates 'base' image from ASP.NET image (includes runtime)
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

#build stage starts from dotnet sdk image 
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

#restores nuget dependencies
COPY ./WeatherForecastRemote.Api/*.csproj WeatherForecastRemote.Api/
COPY ./WeatherForecastRemote.Infrastructure/*.csproj WeatherForecastRemote.Infrastructure/
RUN dotnet restore WeatherForecastRemote.Api/WeatherForecastRemote.Api.csproj
RUN dotnet restore WeatherForecastRemote.Infrastructure/WeatherForecastRemote.Infrastructure.csproj

#all files from Docker build context is copied (with existing folder structure) to workdir (/src) directory inside Docker image.
COPY . ./

#this compiles the project with release configuration and publishes results in WeatherForecastRemote.Api/publish directory (in our case /src/WeatherForecastRemote.Api/publish).
RUN dotnet publish WeatherForecastRemote.Api -c Release -o WeatherForecastRemote.Api/publish


#T#real production image preparation
FROM base AS final
WORKDIR /app
COPY --from=build /src/WeatherForecastRemote.Api/publish .
ENTRYPOINT ["dotnet", "WeatherForecastRemote.Api.dll"]