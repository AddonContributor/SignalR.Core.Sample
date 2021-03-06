FROM alpine:3.8 as base
RUN echo http://mirror.yandex.ru/mirrors/alpine/v3.8/main > /etc/apk/repositories; \
    echo http://mirror.yandex.ru/mirrors/alpine/v3.8/community >> /etc/apk/repositories

RUN apk update \
    && apk add --no-cache \
        ca-certificates \
        \
        # .NET Core dependencies
        krb5-libs \
        libgcc \
        libintl \
        libssl1.0 \
        libstdc++ \
        tzdata \
        userspace-rcu \
        zlib \
    && apk -X https://dl-cdn.alpinelinux.org/alpine/edge/main add --no-cache \
        lttng-ust

# Configure Kestrel web server to bind to port 80 when present
ENV ASPNETCORE_URLS=http://+:80 \
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true \
    # Set the invariant mode since icu_libs isn't included (see https://github.com/dotnet/announcements/issues/20)
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk-alpine3.7 AS build
WORKDIR /src
COPY signalrchat.sln ./
COPY signalrchat.csproj ./
COPY docker-compose.dcproj ./
RUN dotnet restore -nowarn:msb3202,nu1503 signalrchat.csproj
COPY . .
WORKDIR /src/
RUN dotnet build -c Release -o /app -r alpine.3.7-x64 signalrchat.csproj

FROM build AS publish
RUN dotnet publish -c Release -o /app -r alpine.3.7-x64 signalrchat.csproj

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["/app/signalrchat"]

