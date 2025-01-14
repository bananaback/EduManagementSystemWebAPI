# Use .NET SDK image to build the solution
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy the Domain project and restore dependencies
COPY StudentManagement.Domain/StudentManagement.Domain.csproj ./StudentManagement.Domain/
RUN dotnet restore StudentManagement.Domain/StudentManagement.Domain.csproj

# Copy the Application project and restore dependencies
COPY StudentManagement.Application/StudentManagement.Application.csproj ./StudentManagement.Application/
RUN dotnet restore StudentManagement.Application/StudentManagement.Application.csproj

# Copy the Infrastructure project and restore dependencies
COPY StudentManagement.Infrastructure/StudentManagement.Infrastructure.csproj ./StudentManagement.Infrastructure/
RUN dotnet restore StudentManagement.Infrastructure/StudentManagement.Infrastructure.csproj

# Copy the API project and restore dependencies
COPY StudentManagement.API/StudentManagement.API.csproj ./StudentManagement.API/
RUN dotnet restore StudentManagement.API/StudentManagement.API.csproj

# Copy the entire solution and build
COPY . ./
RUN dotnet publish StudentManagement.API/StudentManagement.API.csproj -c Release -o out

# Use .NET runtime image for production
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

# Expose the port your app runs on (optional)
EXPOSE 80

ENTRYPOINT ["dotnet", "StudentManagement.API.dll"]
