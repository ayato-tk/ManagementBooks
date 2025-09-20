FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY . .

RUN dotnet restore src/GestaoLivros/GestaoLivros.Presentation/GestaoLivros.Presentation.csproj

RUN dotnet publish src/GestaoLivros/GestaoLivros.Presentation/GestaoLivros.Presentation.csproj -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out ./
EXPOSE 80
ENTRYPOINT ["dotnet", "GestaoLivros.Presentation.dll"]