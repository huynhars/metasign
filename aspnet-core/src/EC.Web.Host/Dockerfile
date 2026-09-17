# Stage 1: Build bằng .NET 6 SDK
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY . .
WORKDIR "/src/EC.Web.Host"
RUN dotnet publish "EC.Web.Host.csproj" -c Release -o /app/out

# Stage 2: Runtime với .NET 6 Alpine (Siêu nhẹ & không bị lỗi repo)
FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS final
WORKDIR /app

# Cài đặt icu-libs trên Alpine
RUN apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

COPY --from=build /app/out .

EXPOSE 80
ENTRYPOINT ["dotnet", "EC.Web.Host.dll"]