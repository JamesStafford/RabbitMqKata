﻿FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER root
RUN apt-get update && apt-get install -y iputils-ping && rm -rf /var/lib/apt/lists/*
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["RabbitMQKata/RabbitMQKata.csproj", "RabbitMQKata/"]
RUN dotnet restore "RabbitMQKata/RabbitMQKata.csproj"
COPY . .
WORKDIR "/src/RabbitMQKata"
RUN dotnet build "./RabbitMQKata.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./RabbitMQKata.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RabbitMQKata.dll"]
