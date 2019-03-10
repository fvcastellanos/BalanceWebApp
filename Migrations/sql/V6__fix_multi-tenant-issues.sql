ALTER TABLE account_balance.provider DROP INDEX UQ_NAME_COUNTRY;

CREATE INDEX provider_name_IDX USING BTREE ON account_balance.provider (name,country,tenant);
