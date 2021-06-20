FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build-env
WORKDIR /app
RUN apk update -yq 
RUN apk add nodejs
RUN apk add npm
COPY *sln ./
COPY . .
RUN dotnet restore ./ProductManager.Web/ProductManager.Web.csproj
RUN dotnet publish ./ProductManager.Web -c Release -o out



FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /home
RUN apt-get update -yq
RUN apt-get install nodejs -yq
COPY --from=build-env /app/out .
ENTRYPOINT dotnet ProductManager.Web.dll