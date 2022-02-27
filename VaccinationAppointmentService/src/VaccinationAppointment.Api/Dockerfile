#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["VaccinationAppointmentService/src/VaccinationAppointment.Api/VaccinationAppointment.Api.csproj", "VaccinationAppointmentService/src/VaccinationAppointment.Api/"]
RUN dotnet restore "VaccinationAppointmentService/src/VaccinationAppointment.Api/VaccinationAppointment.Api.csproj"
COPY . .
WORKDIR "/src/VaccinationAppointmentService/src/VaccinationAppointment.Api"
RUN dotnet build "VaccinationAppointment.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VaccinationAppointment.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VaccinationAppointment.Api.dll"]