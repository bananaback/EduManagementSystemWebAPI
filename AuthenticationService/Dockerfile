# Use .NET SDK image to build the solution
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy the Domain project and restore dependencies
COPY AuthenticationService.Domain/AuthenticationService.Domain.csproj ./AuthenticationService.Domain/
RUN dotnet restore AuthenticationService.Domain/AuthenticationService.Domain.csproj

# Copy the Application project and restore dependencies
COPY AuthenticationService.Application/AuthenticationService.Application.csproj ./AuthenticationService.Application/
RUN dotnet restore AuthenticationService.Application/AuthenticationService.Application.csproj

# Copy the Infrastructure project and restore dependencies
COPY AuthenticationService.Infrastructure/AuthenticationService.Infrastructure.csproj ./AuthenticationService.Infrastructure/
RUN dotnet restore AuthenticationService.Infrastructure/AuthenticationService.Infrastructure.csproj

# Copy the API project and restore dependencies
COPY AuthenticationService.API/AuthenticationService.API.csproj ./AuthenticationService.API/
RUN dotnet restore AuthenticationService.API/AuthenticationService.API.csproj

# Copy the entire solution and build
COPY . ./
RUN dotnet publish AuthenticationService.API/AuthenticationService.API.csproj -c Release -o out

# Use .NET runtime image for production
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

# Expose the port your app runs on (optional)
EXPOSE 80

ENTRYPOINT ["dotnet", "AuthenticationService.API.dll"]
