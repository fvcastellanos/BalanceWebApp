$MIGRATIONS_DIR = Get-Location
$CONF_DIR = $MIGRATIONS_DIR.Path + "\conf"
$SQL_DIR = $MIGRATIONS_DIR.Path + "\sql"
Write-Output $CONF_DIR
Write-Output $SQL_DIR

docker run --rm -v ${SQL_DIR}:/flyway/sql -v ${CONF_DIR}:/flyway/conf boxfuse/flyway:5.2 migrate
