FROM docker.io/postgres:latest

ENV POSTGRES_DB='weather_forceast_storage'
ENV POSTGRES_USER='postgres'
ENV POSTGRES_PASSWORD='postgres'
ENV PG_TRUST_LOCALNET=1

WORKDIR /

COPY ./content ./docker-entrypoint-initdb.d

EXPOSE 5432