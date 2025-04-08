FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["SeconAPI.Api/SeconAPI.Api.csproj", "SeconAPI.Api/"]
COPY ["SeconAPI.Application/SeconAPI.Application.csproj", "SeconAPI.Application/"]
COPY ["SeconAPI.Domain/SeconAPI.Domain.csproj", "SeconAPI.Domain/"]
COPY ["SeconAPI.Infrastructure/SeconAPI.Infrastructure.csproj", "SeconAPI.Infrastructure/"]
RUN dotnet restore "SeconAPI.Api/SeconAPI.Api.csproj"
COPY . .
WORKDIR "/src/SeconAPI.Api"
RUN dotnet build "SeconAPI.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SeconAPI.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SeconAPI.Api.dll"]