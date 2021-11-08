#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["DemoWebCam/DemoWebCam.csproj", "DemoWebCam/"]
COPY ["NiqabCommonLibrary/NiqabCommonLibrary.csproj", "NiqabCommonLibrary/"]
RUN dotnet restore "DemoWebCam/DemoWebCam.csproj"
COPY . .
WORKDIR "/src/DemoWebCam"
RUN dotnet build "DemoWebCam.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DemoWebCam.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DemoWebCam.dll"]