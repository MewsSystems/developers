-- Table exchange_rates
CREATE TABLE IF NOT EXISTS exchange_rates
(
    id              BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    source_currency VARCHAR(3)      NOT NULL,
    target_currency VARCHAR(3)      NOT NULL,
    rate            NUMERIC(20, 10) NOT NULL,
    date            DATE            NOT NULL
);
