# ==================== Build ====================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy solution and all csproj fails (cache layer)
COPY *.sln .
COPY **/*.csproj ./temp/
RUN for file in $(find ./temp -name '*.csproj'); do \
      dir=$(basename $(dirname $file)); \
      mkdir -p ./$dir; \
      mv $file ./$dir/; \
    done && rm -rf ./temp

# Restore (cache if csproj fails don't change)
RUN dotnet restore

# Copy full code and build
COPY . .
RUN dotnet publish Efficio.WebApp/Efficio.WebApp.csproj -c Release -o /app/out --no-restore

# ==================== Runtime ====================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
EXPOSE 8080

COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "Efficio.WebApp.dll"]