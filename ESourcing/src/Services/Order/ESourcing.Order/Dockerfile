#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Services/Order/ESourcing.Order/ESourcing.Order.csproj", "src/Services/Order/ESourcing.Order/"]
COPY ["src/Services/Common/EventBusRabbitMQ/EventBusRabbitMQ.csproj", "src/Services/Common/EventBusRabbitMQ/"]
COPY ["src/Services/Order/Ordering.Application/Ordering.Application.csproj", "src/Services/Order/Ordering.Application/"]
COPY ["src/Services/Order/Ordering.Domain/Ordering.Domain.csproj", "src/Services/Order/Ordering.Domain/"]
COPY ["src/Services/Order/Ordering.Infrastructure/Ordering.Infrastructure.csproj", "src/Services/Order/Ordering.Infrastructure/"]
RUN dotnet restore "src/Services/Order/ESourcing.Order/ESourcing.Order.csproj"
COPY . .
WORKDIR "/src/src/Services/Order/ESourcing.Order"
RUN dotnet build "ESourcing.Order.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ESourcing.Order.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ESourcing.Order.dll"]