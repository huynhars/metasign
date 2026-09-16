# Stage 1: Build ứng dụng
FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src

# Copy toàn bộ code vào container để restore & publish
COPY . .
RUN dotnet publish "EC.Web.Host.csproj" -c Release -o /app/out

# Stage 2: Run ứng dụng
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS runtime
WORKDIR /app
COPY --from=build /app/out ./

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "EC.Web.Host.dll"]