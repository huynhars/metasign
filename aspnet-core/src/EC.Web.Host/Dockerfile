# Stage 1: Build bằng .NET 6 SDK
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY . .
WORKDIR "/src/EC.Web.Host"
RUN dotnet publish "EC.Web.Host.csproj" -c Release -o /app/out

# Stage 2: Runtime dùng bản Ubuntu (Tích hợp sẵn ICU, không bị lỗi 139 hay 404)
FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal AS final
WORKDIR /app

# Tắt Invariant Mode để ABP Framework đọc được Culture "en", "vi"...
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

COPY --from=build /app/out .

EXPOSE 80
ENTRYPOINT ["dotnet", "EC.Web.Host.dll"]