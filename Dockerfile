FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["GymTracker.csproj", "./"]
RUN dotnet restore "GymTracker.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "GymTracker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GymTracker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GymTracker.dll"]
