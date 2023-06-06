#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ProjectManagementService.csproj", "."]
RUN dotnet restore "./ProjectManagementService.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "ProjectManagementService.csproj" -c Release -o /app/build

RUN dotnet tool install --global dotnet-ef
#ENV PATH="${PATH}:/root/.dotnet/tools"
#RUN dotnet ef database update

FROM build AS publish
RUN dotnet publish "ProjectManagementService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProjectManagementService.dll"]

CMD ["dotnet", "ef", "database", "update"]