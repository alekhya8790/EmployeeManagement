FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY EmployeeManagement.API/EmployeeManagement.API.csproj EmployeeManagement.API/
RUN dotnet restore EmployeeManagement.API/EmployeeManagement.API.csproj

COPY EmployeeManagement.API/ EmployeeManagement.API/
WORKDIR /src/EmployeeManagement.API
RUN dotnet publish EmployeeManagement.API.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "EmployeeManagement.API.dll"]
