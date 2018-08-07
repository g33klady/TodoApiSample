FROM microsoft/dotnet:sdk AS build-env
WORKDIR /app


COPY *.csproj ./
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o out

#FROM microsoft/dotnet:2.1-aspnetcore-runtime
FROM microsoft/dotnet:2.1-aspnetcore-runtime-alpine AS runtime
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "TodoApi.dll"]
