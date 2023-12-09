#This image is used for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build-env
WORKDIR /app
COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o out

#This image is used to run the application
FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine
EXPOSE 23
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "IoTBridge.dll"]

