# FROM dotnet.2.1

# COPY source dest

# ENTRYPOINT [ "dotnet run consumer-test.dll" ]
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["consumer-test.csproj", "./"]
RUN dotnet restore "consumer-test.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "consumer-test.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "consumer-test.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "consumer-test.dll"]
