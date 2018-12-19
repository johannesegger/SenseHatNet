FROM microsoft/dotnet:2.1-sdk as builder
WORKDIR /usr/src/
COPY SenseTest .
RUN dotnet publish -r linux-arm -o /usr/src/dist

FROM microsoft/dotnet:2.1-runtime
WORKDIR /usr/app/
COPY --from=builder /usr/src/dist .

# https://github.com/dotnet/cli/issues/3390
RUN apt-get update && \
    apt-get install libunwind8

ENTRYPOINT [ "./SenseTest" ]

