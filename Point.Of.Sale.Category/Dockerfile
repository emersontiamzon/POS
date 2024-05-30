FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Point.Of.Sale.Category/Point.Of.Sale.Category.csproj", "Point.Of.Sale.Category/"]
RUN dotnet restore "Point.Of.Sale.Category/Point.Of.Sale.Category.csproj"
COPY . .
WORKDIR "/src/Point.Of.Sale.Category"
RUN dotnet build "Point.Of.Sale.Category.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Point.Of.Sale.Category.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Point.Of.Sale.Category.dll"]
