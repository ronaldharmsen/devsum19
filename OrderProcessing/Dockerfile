FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS base
WORKDIR /app
ENV CLUSTER_SEEDS "[]"
ENV CLUSTER_IP ""
ENV CLUSTER_PORT "5213"

# 9110 - Petabridge.Cmd
# 4053 - Akka.Cluster
EXPOSE 9110 4053

FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /src
COPY ["AmazingWebshop.csproj", ""]
#RUN dotnet restore "AmazingWebshop.csproj"
COPY . .

#WORKDIR /src
#RUN dotnet build "AmazingWebshop.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "AmazingWebshop.csproj" -c Release -o /app

FROM base AS final

WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "AmazingWebshop.dll"]