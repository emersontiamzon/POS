FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Point.Of.Sale/Point.Of.Sale.csproj", "Point.Of.Sale/"]
RUN dotnet restore "Point.Of.Sale/Point.Of.Sale.csproj"
COPY . .
WORKDIR "/src/Point.Of.Sale"
RUN dotnet build "Point.Of.Sale.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Point.Of.Sale.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Point.Of.Sale.dll"]
