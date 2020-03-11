
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 5001
ENV POSTGRES__CSTR Host=postgres;Port=5432;Database=locationservice;Username=integrator;Password=inteword
ENV TRANSIENT false
ENV ASPNETCORE_URLS http://*:5001
COPY . .
ENTRYPOINT ["dotnet", "LocationService.dll"]