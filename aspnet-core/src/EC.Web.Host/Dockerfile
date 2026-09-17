# Stage 1: Build bằng .NET 6 SDK
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY . .
WORKDIR "/src/EC.Web.Host"
RUN dotnet publish "EC.Web.Host.csproj" -c Release -o /app/out

# Stage 2: Runtime với ASP.NET 6 (Debian Slim)
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app

# Fix triệt để lỗi repo 404 và cài libicu
USER root
RUN apt-get update --fix-missing && \
    apt-get install -y --no-install-recommends libicu70 || apt-get install -y --no-install-recommends libicu-dev && \
    rm -rf /var/lib/apt/lists/*

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

COPY --from=build /app/out .

EXPOSE 80
ENTRYPOINT ["dotnet", "EC.Web.Host.dll"]