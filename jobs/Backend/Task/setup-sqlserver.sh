#!/bin/bash
# Script to install and start SQL Server using Docker on Mac/Linux
# LocalDB is Windows-only, so we use Docker with SQL Server for Linux

CONTAINER_NAME="sqlserver-localdb"
SA_PASSWORD="YourStrong@Passw0rd"
DATABASE_NAME="ExchangeRateDB"
SQL_PORT="1433"
SQLCMD_PATH="/opt/mssql-tools18/bin/sqlcmd"

echo "=== SQL Server Docker Setup Script (Mac/Linux) ==="
echo ""
echo "Note: LocalDB is Windows-only. This script sets up SQL Server in Docker."
echo ""

# Check if Docker is installed
if ! command -v docker &> /dev/null; then
    echo "Error: Docker is not installed."
    echo ""
    echo "Please install Docker Desktop:"
    if [[ "$OSTYPE" == "darwin"* ]]; then
        echo "  Mac: https://www.docker.com/products/docker-desktop"
    else
        echo "  Linux: https://docs.docker.com/engine/install/"
    fi
    exit 1
fi

# Check if Docker is running
if ! docker info &> /dev/null; then
    echo "Error: Docker is not running."
    echo "Please start Docker Desktop and try again."
    exit 1
fi

echo "Docker is installed and running."
echo ""

# Check if port is already in use
if lsof -Pi :$SQL_PORT -sTCP:LISTEN -t >/dev/null 2>&1 || netstat -an 2>/dev/null | grep -q ":$SQL_PORT.*LISTEN"; then
    echo "Warning: Port $SQL_PORT is already in use."
    echo "This may cause issues if not used by an existing SQL Server container."
    echo ""
fi

# Check if container already exists
if docker ps -a --format '{{.Names}}' | grep -q "^${CONTAINER_NAME}$"; then
    echo "SQL Server container already exists."

    # Check if it's running
    if docker ps --format '{{.Names}}' | grep -q "^${CONTAINER_NAME}$"; then
        echo "Container is already running."
    else
        echo "Starting existing container..."
        docker start $CONTAINER_NAME
        echo "Container started successfully."
    fi

    # Detect sqlcmd path for existing container
    if docker exec $CONTAINER_NAME test -f /opt/mssql-tools18/bin/sqlcmd 2>/dev/null; then
        SQLCMD_PATH="/opt/mssql-tools18/bin/sqlcmd"
    elif docker exec $CONTAINER_NAME test -f /opt/mssql-tools/bin/sqlcmd 2>/dev/null; then
        SQLCMD_PATH="/opt/mssql-tools/bin/sqlcmd"
    else
        SQLCMD_PATH="/opt/mssql-tools18/bin/sqlcmd"
    fi
else
    echo "Creating and starting SQL Server container..."
    echo ""

    # Pull and run SQL Server
    docker run -e "ACCEPT_EULA=Y" \
        -e "MSSQL_SA_PASSWORD=$SA_PASSWORD" \
        -p $SQL_PORT:1433 \
        --name $CONTAINER_NAME \
        --hostname sqlserver \
        -d mcr.microsoft.com/mssql/server:2022-latest

    if [ $? -eq 0 ]; then
        echo "SQL Server container created and started successfully."
    else
        echo "Error creating container."
        exit 1
    fi

    # Wait for SQL Server to start
    echo "Waiting for SQL Server to start (this may take 30-60 seconds)..."
    sleep 30

    # Detect sqlcmd path (SQL Server 2022 uses mssql-tools18)
    if docker exec $CONTAINER_NAME test -f /opt/mssql-tools18/bin/sqlcmd 2>/dev/null; then
        SQLCMD_PATH="/opt/mssql-tools18/bin/sqlcmd"
    elif docker exec $CONTAINER_NAME test -f /opt/mssql-tools/bin/sqlcmd 2>/dev/null; then
        SQLCMD_PATH="/opt/mssql-tools/bin/sqlcmd"
    else
        echo "Warning: Could not find sqlcmd in container."
        SQLCMD_PATH="/opt/mssql-tools18/bin/sqlcmd"
    fi

    # Test connection
    for i in {1..10}; do
        if docker exec $CONTAINER_NAME $SQLCMD_PATH \
            -S localhost -U sa -P "$SA_PASSWORD" -C \
            -Q "SELECT @@VERSION" &> /dev/null; then
            echo "SQL Server is ready!"
            break
        fi
        echo "Waiting... ($i/10)"
        sleep 5
    done
fi

echo ""
echo "Checking SQL Server status..."
docker exec $CONTAINER_NAME $SQLCMD_PATH \
    -S localhost -U sa -P "$SA_PASSWORD" -C \
    -Q "SELECT @@VERSION" 2>/dev/null || echo "Note: Unable to verify SQL Server status. Container may still be starting."

echo ""
echo "=== Setup Complete ==="
echo ""
echo "Connection details:"
echo "  Server: localhost,$SQL_PORT"
echo "  Username: sa"
echo "  Password: $SA_PASSWORD"
echo "  Database: $DATABASE_NAME"
echo ""
echo "Connection string:"
echo "  Server=localhost,$SQL_PORT;User Id=sa;Password=$SA_PASSWORD;Database=$DATABASE_NAME;TrustServerCertificate=True;"
echo ""
echo "Container management commands:"
echo "  View logs:    docker logs $CONTAINER_NAME"
echo "  Stop:         docker stop $CONTAINER_NAME"
echo "  Start:        docker start $CONTAINER_NAME"
echo "  Remove:       docker rm -f $CONTAINER_NAME"
echo ""
echo "To connect using sqlcmd:"
echo "  docker exec -it $CONTAINER_NAME $SQLCMD_PATH -S localhost -U sa -P '$SA_PASSWORD' -C"
echo ""
