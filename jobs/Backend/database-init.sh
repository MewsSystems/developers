set -e

psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" --dbname "$POSTGRES_DB" <<-EOSQL
    -- USERS
    CREATE USER admin WITH SUPERUSER PASSWORD '$ADMIN_USER_PASSWORD';
    CREATE USER db_user WITH PASSWORD '$DB_USER_PASSWORD';

    -- Create database
    CREATE DATABASE exchange_rates_db;
    GRANT CONNECT ON DATABASE exchange_rates_db TO db_user;

    \c exchange_rates_db

    -- Table exchange_rates
    CREATE TABLE exchange_rates (
        id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
        source_currency VARCHAR(3) NOT NULL,
        target_currency VARCHAR(3) NOT NULL,
        rate NUMERIC(20, 10) NOT NULL,
        date date NOT NULL
    );

    GRANT SELECT, INSERT ON exchange_rates TO db_user;
    GRANT USAGE, SELECT ON SEQUENCE exchange_rates_id_seq TO db_user;
EOSQL

cat > /var/lib/postgresql/data/pg_hba.conf <<- EOF
# This configuration may be enhanced using this link https://www.postgresql.org/docs/current/auth-pg-hba-conf.html

# From local, no connections available
local all all reject

# Default user is not allowed
host all "$POSTGRES_USER" all reject

# Users allowed to log from anywhere - NOT SECURE
host all admin 0.0.0.0/0 scram-sha-256
host all db_user 0.0.0.0/0 scram-sha-256
EOF
