# **********************************************
# ALIASES
# **********************************************
FROM mcr.microsoft.com/dotnet/core/sdk:3.1.300-alpine AS DotNetSDK
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.4-alpine AS AspNetRuntime
FROM mcr.microsoft.com/dotnet/core/runtime:3.1.4-alpine AS DotNetRuntime

# **********************************************
# Build temp image - Compile the solution, but don't run tests.
# **********************************************
FROM DotNetSDK AS build
WORKDIR /app

# Copy all the `csproj`s, and restore it into the correct folder structure. (One folder at a time so we know where to put them)
COPY ./global.json ./
COPY *.sln ./

COPY src/*/*.csproj ./src/
RUN for file in $(ls ./src/*.csproj); do mkdir -p ${file%.*}/ && mv $file ${file%.*}/; done

# RUN find . # Uncomment and build docker to list everything in docker context.
RUN dotnet restore

COPY . ./
# RUN find . # Uncomment and build docker to list everything in docker context.

ARG version=0.1.0
RUN dotnet build /p:"Version=${version}" -c Release --no-restore

# **********************************************
# Publish temp image - generate the final files after tests run successfully
# **********************************************
FROM build AS publish

RUN dotnet publish src/BugTracker.Api --no-build -c Release -o /app/out/api

# **********************************************
# runnable image
# **********************************************
FROM AspNetRuntime AS api-runtime

WORKDIR /app
ENV ASPNETCORE_URLS http://+:5000
ENV ASPNETCORE_ENVIRONMENT Production
EXPOSE 5000
COPY --from=publish /app/out/api .
ENTRYPOINT ["dotnet", "./BugTracker.Api.dll"]
CMD []
