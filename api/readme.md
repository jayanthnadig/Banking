# Development Setup

## Software
Install VS 2017, Postgres

## Init
create a css database on your machine
check the connection string in appsettings.json
in package manager console: run update-database
run the solution. this will seed data

# Database Migrations

## Adding Migrations
add-migration v1

## Generating Script
script-migration -idempotent -output api/migrations/schema.sql

## Updating DB on local
update-database -migration V1