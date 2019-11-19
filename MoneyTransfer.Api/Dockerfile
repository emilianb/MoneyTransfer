FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["MoneyTransfer.Runner/MoneyTransfer.Runner.csproj", "MoneyTransfer.Runner/"]
RUN dotnet restore "MoneyTransfer.Runner/MoneyTransfer.Runner.csproj"
COPY . .
WORKDIR "/src/MoneyTransfer.Runner"
RUN dotnet build "MoneyTransfer.Runner.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "MoneyTransfer.Runner.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MoneyTransfer.Runner.dll"]