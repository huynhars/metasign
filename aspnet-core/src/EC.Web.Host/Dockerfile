# Stage 1: Build dự án
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY . .
WORKDIR "/src/EC.Web.Host"
RUN dotnet publish "EC.Web.Host.csproj" -c Release -o /app/out

# Stage 2: Runtime chính thức từ Microsoft (dùng Debian Slim ổn định)
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app

# Tắt phụ thuộc thư viện Globalization để ngăn hoàn toàn lỗi Segmentation Fault (139)
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1

COPY --from=build /app/out .

EXPOSE 80
ENTRYPOINT ["dotnet", "EC.Web.Host.dll"]