#!/bin/sh

CONF_DIR=$(pwd)/conf
SQL_DIR=$(pwd)/sql
echo $CONF_DIR
echo $SQL_DIR

docker run --rm -v $SQL_DIR:/flyway/sql -v $CONF_DIR:/flyway/conf boxfuse/flyway:5.2 migrate