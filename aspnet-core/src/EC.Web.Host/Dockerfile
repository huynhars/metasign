# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /src

COPY . .

WORKDIR "/src/EC.Web.Host"

RUN dotnet publish "EC.Web.Host.csproj" -c Release -o /app/out


# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final

WORKDIR /app

COPY --from=build /app/out .

EXPOSE 80

ENTRYPOINT ["dotnet", "EC.Web.Host.dll"]