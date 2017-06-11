# Balance Web Application

This is a web application that is aimed to provide a service to help people to control their saving and monetary accounts, control their expenses.

Right now it is just the first steps, but hopefully it will grow soon.

It is done using ASP.NET Core 1.1 (we wanted to try the multi platform approach that Microsoft claims that this new version of ASP has)

## Initial URL

```ssh

http://localhost:5000/

```

## Database Migrations

To run SQL scripts (database migrations) https://flywaydb.org/documentation/commandline/ was used, would be great if we have a .net port of this great tool

### Migrating

Just go to ``` Migrations ``` directory and execute ``` migrate.sh ``` script

```bash
cd src/BalanceApi/Migrations
./migrate.sh
```

### Cleaning db

Just go to ``` Migrations ``` directory and execute ``` clean.sh ``` script

```bash
cd src/BalanceApi/Migrations
./clean.sh
```

## Unit tests

In order to run the set of unit tests you need to follow this instructions:

* Move to the test project ``` cd test/BalanceApi.Tests ```
* Restore the dependencies ``` dotnet restore ```
* Run the tests ``` dotnet test ```

