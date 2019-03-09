use mysql;

DROP SCHEMA IF EXISTS account_balance;

CREATE SCHEMA IF NOT EXISTS account_balance
  DEFAULT CHARACTER SET utf8mb4;

DROP USER IF EXISTS 'account_balance'@'localhost';
DROP USER IF EXISTS 'account_balance'@'%';

CREATE USER IF NOT EXISTS 'account_balance'@'localhost'
  IDENTIFIED BY 'account_balance123';

CREATE USER IF NOT EXISTS 'account_balance'@'%'
  IDENTIFIED BY 'account_balance123';

GRANT ALL PRIVILEGES ON account_balance.* TO 'account_balance'@'localhost'
IDENTIFIED BY 'account_balance123';

GRANT ALL PRIVILEGES ON account_balance.* TO 'account_balance'@'%'
IDENTIFIED BY 'account_balance123';

FLUSH PRIVILEGES;