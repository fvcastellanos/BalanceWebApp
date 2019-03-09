#!/bin/sh

mysql -h 127.0.0.1 -u root -pr00t -P 3306 < scripts/sql/init-db.sql
