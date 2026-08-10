FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /library_api

COPY ["./library_api.csproj", "."]
RUN dotnet restore

COPY [".", "."]
RUN dotnet build -c Release -o /app/build

FROM build as publish
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0
ENV ASPNETCORE_HTTP_PORTS=5001
EXPOSE 5001
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "library_api.dll"]
