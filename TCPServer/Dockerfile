FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["tcpservice/tcpservice.csproj", "tcpservice/"]
RUN dotnet restore "tcpservice/tcpservice.csproj"
COPY . .
WORKDIR "/src/tcpservice"
RUN dotnet build "tcpservice.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "tcpservice.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "tcpservice.dll"]
