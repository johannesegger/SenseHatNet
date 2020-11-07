FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as builder
WORKDIR /usr/src/

# Copy just the project file(s) and restore to take advantage of the docker multi-layer cache
COPY SenseTest/*.csproj .
RUN dotnet restore

# Copy the remainder of the source files and build
COPY SenseTest .
RUN dotnet publish -o /usr/src/dist --no-restore

FROM mcr.microsoft.com/dotnet/core/runtime:3.1

WORKDIR /usr/app/
COPY --from=builder /usr/src/dist .

ENTRYPOINT [ "./SenseTest" ]