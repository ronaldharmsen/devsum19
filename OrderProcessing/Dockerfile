FROM mcr.microsoft.com/dotnet/core/runtime:2.2 AS base
WORKDIR /app

ENV CLUSTER_SEEDS "[]"
ENV CLUSTER_IP ""
ENV CLUSTER_PORT "5213"

# 9110 - Petabridge.Cmd
# 4053 - Akka.Cluster
EXPOSE 9110 4053

FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /src
COPY ["OrderProcessing/OrderProcessing.csproj", "OrderProcessing/"]
COPY ["OrderSystem/OrderSystem.csproj", "OrderSystem/"]
RUN dotnet restore "OrderProcessing/OrderProcessing.csproj"
COPY . .
WORKDIR "/src/OrderProcessing"
RUN dotnet build "OrderProcessing.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "OrderProcessing.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "OrderProcessing.dll"]