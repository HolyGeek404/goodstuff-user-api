# -------------------------
# 1️⃣ Build Stage
# -------------------------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy only csproj files first for caching restore
COPY GoodStuff.UserApi.Presentation/*.csproj GoodStuff.UserApi.Presentation/
COPY GoodStuff.UserApi.Domain/*.csproj GoodStuff.UserApi.Domain/
COPY GoodStuff.UserApi.Infrastructure/*.csproj GoodStuff.UserApi.Infrastructure/
COPY GoodStuff.UserApi.Application/*.csproj GoodStuff.UserApi.Application/
COPY GoodStuff.UserApi.Application.Tests/*.csproj GoodStuff.UserApi.Application.Tests/

# Restore dependencies (cached if csproj unchanged)
RUN dotnet restore GoodStuff.UserApi.Presentation/GoodStuff.UserApi.Presentation.csproj

# Copy all source code
COPY GoodStuff.UserApi.Presentation/ GoodStuff.UserApi.Presentation/
COPY GoodStuff.UserApi.Application/ GoodStuff.UserApi.Application/
COPY GoodStuff.UserApi.Domain/ GoodStuff.UserApi.Domain/
COPY GoodStuff.UserApi.Infrastructure/ GoodStuff.UserApi.Infrastructure/
COPY GoodStuff.UserApi.Application.Tests/ GoodStuff.UserApi.Application.Tests/

# Optional: run tests in separate stage (do not include test artifacts in final image)
FROM build AS test
RUN dotnet test GoodStuff.UserApi.Application.Tests/GoodStuff.UserApi.Application.Tests.csproj --no-restore

# Publish app for runtime
FROM build AS publish
RUN dotnet publish GoodStuff.UserApi.Presentation/GoodStuff.UserApi.Presentation.csproj \
    --no-restore -c Release -o /app/publish

# -------------------------
# 2️⃣ Runtime Stage
# -------------------------
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Copy published output only
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "GoodStuff.UserApi.Presentation.dll"]