FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["fundit-server.csproj", "./"]
RUN dotnet restore "fundit-server.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "fundit-server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "fundit-server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "fundit-server.dll"]
