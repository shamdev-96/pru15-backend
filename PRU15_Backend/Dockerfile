 FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
 WORKDIR /app
 EXPOSE 80
 FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
 WORKDIR /src
 COPY ["PRU15_Backend.csproj", ""]
 RUN dotnet restore "./PRU15_Backend.csproj"
 COPY . .
 WORKDIR "/src/."
 RUN dotnet build "PRU15_Backend.csproj" -c Release -o /app/build
 FROM build AS publish
 RUN dotnet publish "PRU15_Backend.csproj" -c Release -o /app/publish
 FROM base AS final
 WORKDIR /app
 COPY --from=publish /app/publish .
 ENTRYPOINT ["dotnet", "PRU15_Backend.dll"]