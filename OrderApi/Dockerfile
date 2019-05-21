FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS base
WORKDIR /app
ENV CLUSTER_SEEDS "[]"
ENV CLUSTER_IP ""
ENV CLUSTER_PORT "5213"

# 4053 - Akka.Cluster
EXPOSE 80 4053

FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /src
COPY ["OrderApi/OrderApi.csproj", "OrderApi/"]
COPY ["OrderSystem/OrderSystem.csproj", "OrderSystem/"]
RUN dotnet restore "OrderApi/OrderApi.csproj"
COPY . .
WORKDIR "/src/OrderApi"
RUN dotnet build "OrderApi.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "OrderApi.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "OrderApi.dll"]