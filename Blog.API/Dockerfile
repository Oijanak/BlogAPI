FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5058

ENV ASPNETCORE_URLS=http://+:5058
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ConnectionStrings__DefaultConnection="Server=host.docker.internal;Database=BlogApiDbNew;User Id=sa;Password=Janak123;TrustServerCertificate=True;Connection Timeout=120;"
ENV ConnectionStrings__HangfireConnection="Server=host.docker.internal;Database=HangfireDb;User Id=sa;Password=Janak123;TrustServerCertificate=True;Connection Timeout=120;"

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Blog.API/Blog.API.csproj", "Blog.API/"]
COPY ["Blog.Application/Blog.Application.csproj", "Blog.Application/"]
COPY ["Blog.Domain/Blog.Domain.csproj", "Blog.Domain/"]
COPY ["Blog.Infrastructure/Blog.Infrastructure.csproj", "Blog.Infrastructure/"]
RUN dotnet restore "Blog.API/Blog.API.csproj"
COPY . .
WORKDIR "/src/Blog.API"
RUN dotnet build "./Blog.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Blog.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Blog.API.dll"]
