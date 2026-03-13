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

# Optional: set globalization invariant for smaller images if needed
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true

# Informational only
EXPOSE 8080

# Use shell form so $PORT is expanded at runtime (Render sets PORT env var)
ENTRYPOINT ["sh","-c","dotnet Main-Project-1.dll --urls http://0.0.0.0:${PORT:-8080}"]