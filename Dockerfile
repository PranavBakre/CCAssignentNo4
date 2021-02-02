FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /dockerAssmnt

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o dist

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime
WORKDIR /dockerAssmnt
COPY --from=build-env /dockerAssmnt/dist .
ENTRYPOINT ["dotnet" , "AssignmentNo4.dll"]