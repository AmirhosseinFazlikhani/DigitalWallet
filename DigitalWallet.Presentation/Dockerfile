#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["DigitalWallet.Presentation/DigitalWallet.Presentation.csproj", "DigitalWallet.Presentation/"]
COPY ["DigitalWallet.Application/DigitalWallet.Application.csproj", "DigitalWallet.Application/"]
COPY ["DigitalWallet.Domain/DigitalWallet.Domain.csproj", "DigitalWallet.Domain/"]
COPY ["DigitalWallet.Infrastructure/DigitalWallet.Infrastructure.csproj", "DigitalWallet.Infrastructure/"]
RUN dotnet restore "DigitalWallet.Presentation/DigitalWallet.Presentation.csproj"
COPY . .
WORKDIR "/src/DigitalWallet.Presentation"
RUN dotnet build "DigitalWallet.Presentation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DigitalWallet.Presentation.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DigitalWallet.Presentation.dll"]
