FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["./NotebookAPI/NotebookAPI.csproj", "NotebookAPI/"]
RUN dotnet restore "NotebookAPI/NotebookAPI.csproj"
COPY . .
WORKDIR "/src/NotebookAPI"
RUN dotnet build "NotebookAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NotebookAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NotebookAPI.dll"]
