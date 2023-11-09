DROP ROLE IF EXISTS weather_forecast_user;

CREATE ROLE weather_forecast_user WITH
  LOGIN
  SUPERUSER
  NOINHERIT
  CREATEDB
  CREATEROLE
  NOREPLICATION
  PASSWORD 'password';