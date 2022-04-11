FROM public.ecr.aws/lambda/dotnet:6 AS base

FROM mcr.microsoft.com/dotnet/sdk:6.0-bullseye-slim as build
WORKDIR /src
COPY ["RepoDashboard.csproj", "RepoDashboard/"]
RUN dotnet restore "RepoDashboard/RepoDashboard.csproj"

WORKDIR "/src/RepoDashboard"
COPY . .
RUN dotnet build "RepoDashboard.csproj" --configuration Release --output /app/build

FROM build AS publish
RUN dotnet publish "RepoDashboard.csproj" \
            --configuration Release \ 
            --runtime linux-x64 \
            --self-contained true \ 
            --output /app/publish \
            -p:PublishReadyToRun=true  

FROM base AS final
WORKDIR /var/task
COPY --from=publish /app/publish .
CMD ["RepoDashboard::RepoDashboard.LambdaEntryPoint::FunctionHandlerAsync"]a