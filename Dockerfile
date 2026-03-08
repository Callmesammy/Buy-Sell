# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy all project files
COPY ["Buy&Sell/Buy&Sell.csproj", "Buy&Sell/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infastructure/Infastructure.csproj", "Infastructure/"]

# Restore dependencies
RUN dotnet restore "Buy&Sell/Buy&Sell.csproj"

# Copy source code
COPY . .

# Build the project
RUN dotnet build "Buy&Sell/Buy&Sell.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "Buy&Sell/Buy&Sell.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
EXPOSE 8080
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Buy&Sell.dll"]
