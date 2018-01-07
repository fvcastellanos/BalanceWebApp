# Balance Web Application

This is a web application that is aimed to provide a service to help people to control their saving and monetary accounts, control their expenses.

## Initial URL

```ssh
http://localhost:5000/

```

## Database Migrations

To run SQL scripts (database migrations) https://flywaydb.org/documentation/commandline/ was used, would be great if we have a .net port of this great tool

### Migrating

Just go to ``` Migrations ``` directory and execute ``` migrate.sh ``` script

```bash
cd src/BalanceWebApp/Migrations
./migrate.sh
```

### Cleaning db

Just go to ``` Migrations ``` directory and execute ``` clean.sh ``` script

```bash
cd src/BalanceWebApp/Migrations
./clean.sh
```
