FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["Factoria.Proyectos.Api.csproj", "./"]
RUN dotnet restore "Factoria.Proyectos.Api.csproj"

COPY . .
RUN dotnet publish "Factoria.Proyectos.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Factoria.Proyectos.Api.dll"]