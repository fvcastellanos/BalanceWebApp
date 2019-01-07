docker run --rm -v sql:/flyway/sql -v ~/projects/balance-webapp/Migrations/conf:/flyway/conf boxfuse/flyway:5.2 clean
