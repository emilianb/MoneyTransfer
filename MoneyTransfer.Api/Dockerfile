FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["MoneyTransfer.Api/MoneyTransfer.Api.csproj", "MoneyTransfer.Api/"]
COPY ["MoneyTransfer.Messages/MoneyTransfer.Messages.csproj", "MoneyTransfer.Messages/"]
COPY ["MoneyTransfer.Services/MoneyTransfer.Services.csproj", "MoneyTransfer.Services/"]
RUN dotnet restore "MoneyTransfer.Api/MoneyTransfer.Api.csproj"
COPY . .
WORKDIR "/src/MoneyTransfer.Api"
RUN dotnet build "MoneyTransfer.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "MoneyTransfer.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MoneyTransfer.Api.dll"]
