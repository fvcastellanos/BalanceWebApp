ALTER TABLE `account_balance`.`provider` 
ADD UNIQUE INDEX `UQ_NAME_COUNTRY` (`name` ASC, `country` ASC);
