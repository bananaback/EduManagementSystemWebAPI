# Use .NET SDK image to build the solution
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy the Domain project and restore dependencies
COPY ClassManagement.Domain/ClassManagement.Domain.csproj ./ClassManagement.Domain/
RUN dotnet restore ClassManagement.Domain/ClassManagement.Domain.csproj

# Copy the Application project and restore dependencies
COPY ClassManagement.Application/ClassManagement.Application.csproj ./ClassManagement.Application/
RUN dotnet restore ClassManagement.Application/ClassManagement.Application.csproj

# Copy the Infrastructure project and restore dependencies
COPY ClassManagement.Infrastructure/ClassManagement.Infrastructure.csproj ./ClassManagement.Infrastructure/
RUN dotnet restore ClassManagement.Infrastructure/ClassManagement.Infrastructure.csproj

# Copy the API project and restore dependencies
COPY ClassManagement.API/ClassManagement.API.csproj ./ClassManagement.API/
RUN dotnet restore ClassManagement.API/ClassManagement.API.csproj

# Copy the entire solution and build
COPY . ./
RUN dotnet publish ClassManagement.API/ClassManagement.API.csproj -c Release -o out

# Use .NET runtime image for production
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

# Expose the port your app runs on (optional)
EXPOSE 80

ENTRYPOINT ["dotnet", "ClassManagement.API.dll"]
