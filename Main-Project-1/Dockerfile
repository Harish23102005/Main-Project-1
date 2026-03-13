FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project file
COPY Main-Project-1/Main-Project-1.csproj Main-Project-1/

# Restore dependencies
RUN dotnet restore Main-Project-1/Main-Project-1.csproj

# Copy everything else
COPY . .

# Publish app
WORKDIR /src/Main-Project-1
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "Main-Project-1.dll"]